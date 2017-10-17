using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using JetBrains.Annotations;

[DisallowMultipleComponent]
public class CannonBall : MonoBehaviour {
	
	public float selfDestruct = 5f;

	[HideInInspector]
	public float radius = 0.5f;

	[HideInInspector]
	public List<Vector2> path;

	private float lived;

#if UNITY_EDITOR
	[UsedImplicitly]
	private void OnValidate() {
		selfDestruct = Mathf.Max(0, selfDestruct);
	}

	[UsedImplicitly]
	private void OnDrawGizmos() {
		Gizmos.color = new Color(1, 0, 0, 0.5f);
		Gizmos.DrawWireSphere(transform.position, radius);
	}
#endif

	[UsedImplicitly]
	private void Update()
	{
		if (path == null || lived >= CircularGravityBody.rayCastDeltaTime * path.Count)
			DestroyWithExplosion();
		else
		{
			lived += Time.deltaTime;
			int indexA = Mathf.Min(Mathf.FloorToInt(lived / CircularGravityBody.rayCastDeltaTime), path.Count - 1);
			int indexB = Math.Min(indexA + 1, path.Count - 1);

			float t = lived / CircularGravityBody.rayCastDeltaTime - indexA;

			Vector2 posA = path[indexA];
			Vector2 posB = path[indexB];

			transform.position = Vector2.Lerp(posA, posB, t);
		}
	}

	private void DestroyWithExplosion() {
		// Save all the trails and particle sysyems
		foreach (Transform child in transform) {
			var trail = child.GetComponent<TrailRenderer>();
			var part = child.GetComponent<ParticleSystem>();

			float? time = null;
			if (trail && part) time = Mathf.Max(part.main.startLifetime.constant, trail.time);
			else if (trail) time = trail.time;
			else if (part) time = part.main.startLifetime.constant;

			// Set a death timer
			if (time.HasValue) {
				child.SetParent(null);
				Destroy(child.gameObject, time.Value);
			}

			// Disable the particle system
			if (part) {
				var em = part.emission;
				em.enabled = false;
			}
		}

		// Self destruct
		Destroy(gameObject);

		// Create explosion
		Instantiate(GamePresets.cannonballExplosion, transform.position, transform.rotation);
	}
}
