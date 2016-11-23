using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public abstract class Planet : MonoBehaviour {

	[System.NonSerialized]
	public Rigidbody body;

	[System.NonSerialized]
	public List<Cannon> cannons = new List<Cannon>();
	
	void Awake() {
		body = GetComponent<Rigidbody>();
	}

}
