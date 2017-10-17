using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Planet : MonoBehaviour {

	[HideInInspector]
	public Rigidbody2D body;

	[HideInInspector]
	public List<Cannon> cannons = new List<Cannon>();
	
	private void Awake() {
		body = GetComponent<Rigidbody2D>();
	}

}
