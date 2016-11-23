using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(Rigidbody))]
[DisallowMultipleComponent]
public class CannonBall : MonoBehaviour {
	
	public float noCollisionDuration = 0.5f;
	public float selfDestruct = 5;
	
	private List<Collider> disabledColliders = new List<Collider>();

#if UNITY_EDITOR
	void OnValidate() {
		noCollisionDuration = Mathf.Max(0, noCollisionDuration);
		selfDestruct = Mathf.Max(0, selfDestruct);
	}

	private SphereCollider sphereCollider;
	void OnDrawGizmos() {
		sphereCollider = sphereCollider ?? GetComponent<SphereCollider>();
		if (!sphereCollider) return;
		Gizmos.color = new Color(1, 0, 0, 0.5f);
		Gizmos.DrawWireSphere(transform.position, sphereCollider.radius);
	}
#endif

	void Start() {
		if (noCollisionDuration > float.Epsilon) {
			foreach (var col in GetComponentsInChildren<Collider>()) {
				col.enabled = false;
				disabledColliders.Add(col);
			}
			Invoke("ReenableColliders", noCollisionDuration);
		}

		Destroy(gameObject, selfDestruct);
	}

	void ReenableColliders() {
		foreach(var col in disabledColliders) {
			if (col != null)
				col.enabled = true;
		}
		disabledColliders = null;
	}

	void OnCollisionEnter() {
		// Save all the trails and particle sysyems
		foreach (Transform child in transform) {
			TrailRenderer trail = child.GetComponent<TrailRenderer>();
			ParticleSystem part = child.GetComponent<ParticleSystem>();

			float? time = null;
			if (trail && part) time = Mathf.Max(part.startLifetime, trail.time);
			else if (trail) time = trail.time;
			else if (part) time = part.startLifetime;

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
