using UnityEngine;
using System.Collections;
using ExtensionMethods;
using JetBrains.Annotations;

public sealed class DummyAIPlanet : Planet {

	public Planet target;

	[UsedImplicitly]
	private void Start () {
		Invoke(nameof(Fire), 3);
	}

	[UsedImplicitly]
	private void Update () {
		foreach (Cannon cannon in cannons) {
			cannon.SetWorldTargetPosition(target.transform.position);
		}
	}
	
	private void Fire() {
		cannons[Random.Range(0, cannons.Count)].FireCannonBall();
		Invoke("Fire", Random.Range(0.2f, 0.5f));
	}
}
