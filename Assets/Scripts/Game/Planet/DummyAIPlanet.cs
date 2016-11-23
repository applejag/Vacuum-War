using UnityEngine;
using System.Collections;
using ExtensionMethods;

[DisallowMultipleComponent]
public sealed class DummyAIPlanet : Planet {

	public Planet target;
	
	void Start () {
		Invoke("Fire", 3);
	}
	
	void Update () {
		foreach (var cannon in cannons) {
			cannon.SetWorldTargetAngle((target.transform.position - cannon.transform.position).xz().ToDegrees());
		}
	}

	void Fire() {
		cannons[Random.Range(0, cannons.Count)].FireCannonBall();
		Invoke("Fire", Random.Range(0.2f, 0.5f));
	}
}
