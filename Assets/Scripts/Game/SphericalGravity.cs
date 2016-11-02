using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class SphericalGravity : SingletonBase<SphericalGravity> {

	public float gravity = 0.06674f;
	HashSet<Rigidbody> bodies = new HashSet<Rigidbody>();

	void Start() {
		Physics.gravity = Vector3.zero;

		// Add all rigidbodies
		foreach (var body in FindObjectsOfType<Rigidbody>())
			bodies.Add(body);
	}

	void Update() {
		bodies.RemoveWhere(b => b == null);

		foreach (var a in bodies) {
			foreach (var b in bodies) {
				if (a == b) continue;
				if (!a.useGravity) continue;
				if (a.isKinematic) continue;

				// Gravity formula
				// F = G * m₁ * m₂ / r²

				Vector3 delta = b.position - a.position;
				float sqrDist = Vector3.SqrMagnitude(delta);
				float force = gravity * a.mass * b.mass / sqrDist;

				a.AddForce(delta.normalized * force * Time.deltaTime, ForceMode.Acceleration);
			}
		}
	}

	public static void RegisterRigidbody(Rigidbody body) {
		instance.bodies.Add(body);
	}

}
