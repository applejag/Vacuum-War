using UnityEngine;
using System.Collections;
using ExtensionMethods;

public class Cannon : MonoBehaviour {

	public Transform mount;
	[Header("Forces")]
	public float fireForce = 200f;
	public float fireOffset = 0.2f;
	public float recoil = 100f;
	[Header("Angles")]
	public float angleLimit = 100;
	public float angleSpeed = 300;

	private float currentAngle;
	private float targetAngle;

	[System.NonSerialized]
	public Planet planet;

	void Awake() {
		planet = GetComponentInParent<Planet>();
		planet.cannons.Add(this);
	}

#if UNITY_EDITOR
	void OnDrawGizmosSelected() {
		Gizmos.color = Color.red;
		Gizmos.DrawRay(mount.position, mount.forward);
		Gizmos.DrawRay(mount.position, (-transform.eulerAngles.y + angleLimit + 90).FromDegrees().xzy(0));
		Gizmos.DrawRay(mount.position, (-transform.eulerAngles.y - angleLimit + 90).FromDegrees().xzy(0));
	}
#endif

	void Update() {
		// Turn
		currentAngle = Mathf.Lerp(currentAngle, targetAngle, angleSpeed * Time.deltaTime);
		mount.localEulerAngles = Vector3.up * currentAngle;
	}

	public void FireCannonBall() {
		GameObject clone = Instantiate(GamePresets.cannonball, mount.position + mount.forward * fireOffset, Quaternion.Euler(mount.forward)) as GameObject;

		Rigidbody2D body = clone.GetComponent<Rigidbody2D>();
		body.AddForce(mount.forward * fireForce, ForceMode2D.Impulse);

		Destroy(clone, 15);

		// Add force to planet
		planet.body.AddForceAtPosition(-mount.forward * fireForce * recoil, mount.position, ForceMode2D.Impulse);
	}

	public void SetLocalTargetAngle(float localAngles) {
		targetAngle = Mathf.Clamp(Mathf.DeltaAngle(0, localAngles), -angleLimit, angleLimit);
	}

	public void SetWorldTargetAngle(float worldAngles) {
		var degrees = transform.forward.xz().ToDegrees();
		targetAngle = Mathf.Clamp(Mathf.DeltaAngle(0, degrees-worldAngles), -angleLimit, angleLimit);
	}

}
