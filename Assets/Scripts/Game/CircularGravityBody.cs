using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CircularGravityBody : MonoBehaviour
{

	private static readonly HashSet<Rigidbody2D> _allBodies = new HashSet<Rigidbody2D>();
	public static IReadOnlyCollection<Rigidbody2D> AllBodies => _allBodies;

	private Rigidbody2D body;

	private void Awake()
	{
		body = GetComponent<Rigidbody2D>();
		_allBodies.Add(body);
	}

	private void OnDestroy()
	{
		_allBodies.Remove(body);
	}

	public static List<Vector2> CircleRayCastPath(Vector2 origin, Vector2 velocity, float radius)
	{
		var path = new List<Vector2> {origin};

		

		return path;
	}
}
