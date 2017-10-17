using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ExtensionMethods;
using JetBrains.Annotations;

public class Cannon : MonoBehaviour {

	public Transform mount;
	public LineRenderer aim;
	[Header("Prefab")]
	public GameObject cannonBallPrefab;
	public float cannonBallRadius = 0.5f;

	[Header("Forces")]
	public float fireForce = 200f;
	public float fireOffset = 0.2f;
	public float recoil = 100f;
	[Header("Angles")]
	public float angleLimit = 100;
	public float angleSpeed = 300;
	
	[NonSerialized]
	public bool isAiming;
	[NonSerialized]
	public float aimForceMultiplier = 0;

	private float currentAngle;
	private float targetAngle;

	public Vector3 FireFrom => mount.position + mount.up * fireOffset;

	[HideInInspector]
	public Planet planet;

	[UsedImplicitly]
	private void Awake() {
		planet = GetComponentInParent<Planet>();
		planet.cannons.Add(this);
	}

#if UNITY_EDITOR
	[UsedImplicitly]
	private void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		Gizmos.DrawRay(mount.position + mount.up * fireOffset, mount.up);
		Gizmos.DrawRay(mount.position, (-transform.eulerAngles.z + angleLimit + 90).FromDegrees());
		Gizmos.DrawRay(mount.position, (-transform.eulerAngles.z - angleLimit + 90).FromDegrees());
	}
#endif

	[UsedImplicitly]
	private void Update() {
		// Turn
		currentAngle = Mathf.Lerp(currentAngle, targetAngle, angleSpeed * Time.deltaTime);
		mount.localEulerAngles = new Vector3(0, 0, currentAngle);

		aim.enabled = isAiming;
		if (isAiming)
		{
			aim.useWorldSpace = true;
			Vector3[] path = CalculatePath(cannonBallRadius).Select(v => new Vector3(v.x, v.y)).ToArray();
			aim.positionCount = path.Length;
			aim.SetPositions(path);
		}
	}

	private List<Vector2> CalculatePath(float radius)
	{
		return CircularGravityBody.CircleRayCastPath(FireFrom, radius, mount.up * fireForce * aimForceMultiplier,
			Mathf.PI * radius);
	}

	public void FireCannonBall() {
		GameObject clone = Instantiate(cannonBallPrefab, FireFrom, Quaternion.identity);

		// Calculate path
		var ball = clone.GetComponent<CannonBall>();
		ball.radius = cannonBallRadius;
		ball.path = CalculatePath(cannonBallRadius);

		// Add force to planet
		planet.body.AddForceAtPosition(-mount.up * fireForce * recoil, mount.position, ForceMode2D.Impulse);
	}

	public void SetLocalTargetAngle(float localAngles) {
		targetAngle = Mathf.Clamp(Mathf.DeltaAngle(0, localAngles), -angleLimit, angleLimit);
	}

	public void SetWorldTargetPosition(Vector3 position)
	{
		Vector3 deltaPos = position - transform.position;
		aimForceMultiplier = Mathf.Min(deltaPos.magnitude / 20f + 0.5f, 2);

		float degrees = ((Vector2)deltaPos).ToDegrees();
		float localDegrees = degrees - transform.eulerAngles.z;
		SetLocalTargetAngle(localDegrees - 90);
	}

}
