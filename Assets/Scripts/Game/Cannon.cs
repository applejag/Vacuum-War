using UnityEngine;
using System.Collections;

public class Cannon : MonoBehaviour {

	public Transform mount;
	public float fireForce = 200f;
	public float fireOffset = 0.2f;
	public float recoil = 100f;

	[System.NonSerialized]
	public Planet planet;

	void Awake() {
		planet = GetComponentInParent<Planet>();
		planet.cannons.Add(this);
	}

	void Update() {
		if (GameInput.mousePresent) {
			// Look at the mouse
			if (Input.GetMouseButton(0)) {
				Vector3 euler = mount.eulerAngles;
				euler.y = Mathf.Atan2(
					GameInput.mousePosition.z - mount.position.z,
					-GameInput.mousePosition.x - mount.position.x)
				* Mathf.Rad2Deg - 90;
				mount.eulerAngles = euler;
			}

			// Fire cannonball
			if (Input.GetMouseButtonUp(0)) {
				GameObject clone = Instantiate(GamePrefabs.cannonBall, mount.position + mount.forward * fireOffset, Quaternion.Euler(mount.forward)) as GameObject;

				Rigidbody body = clone.GetComponent<Rigidbody>();
				body.AddForce(mount.forward * fireForce, ForceMode.Impulse);
				SphericalGravity.RegisterRigidbody(body);

				Destroy(clone, 15);

				// Add force to planet
				planet.body.AddForceAtPosition(-mount.forward * fireForce * recoil, mount.position, ForceMode.Impulse);
			}
		}
	}

}
