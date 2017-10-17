using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public sealed class CircularGravityBody : MonoBehaviour
{
	public const float rayCastDeltaTime = 0.2f; // seconds
	public const float rayCastMaxTime = 10f; // seconds
	public const float rayCastCollisionFreeTime = 0.6f; // seconds

	private static readonly HashSet<Rigidbody2D> _allBodies = new HashSet<Rigidbody2D>();
	public static IReadOnlyCollection<Rigidbody2D> AllBodies => _allBodies;

	private Rigidbody2D body;

	[UsedImplicitly]
	private void Awake()
	{
		body = GetComponent<Rigidbody2D>();
	}

	[UsedImplicitly]
	private void OnEnable()
	{
		_allBodies.Add(body);
	}

	[UsedImplicitly]
	private void OnDestroy()
	{
		_allBodies.Remove(body);
	}

	public static List<Vector2> CircleRayCastPath(Vector2 origin, float radius, Vector2 velocity, float mass, int layerMask = Physics2D.DefaultRaycastLayers)
	{
		var path = new List<Vector2> {origin};

		for (float t = 0; t < rayCastMaxTime; t += rayCastDeltaTime)
		{
			Vector2 dtVelocity = velocity * rayCastDeltaTime;

			if (t < rayCastCollisionFreeTime)
			{
				origin += dtVelocity;
				path.Add(origin);
			}
			else
			{
				RaycastHit2D hit = Physics2D.CircleCast(origin, radius, dtVelocity, dtVelocity.magnitude, layerMask);

				if (hit.collider == null)
				{
					origin += dtVelocity;
					path.Add(origin);
				}
				else
				{
					origin = hit.point;
					path.Add(origin);
					return path;
				}
			}

			// Apply circular gravity
			foreach (Rigidbody2D body in AllBodies)
			{
				Vector2 diff = body.worldCenterOfMass - origin;
				float sqrDist = diff.sqrMagnitude;
				float force = GamePresets.RaycastGravity * mass * body.mass / sqrDist;

				velocity += diff.normalized * force * rayCastDeltaTime;
			}
		}

		return path;
	}
}
