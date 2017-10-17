using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(LineRenderer))]
public class TestingScript : MonoBehaviour
{

	private LineRenderer lineRenderer;

	private void Awake()
	{
		lineRenderer = GetComponent<LineRenderer>();
	}

	private void Update ()
	{
		Vector3 origin = transform.position;
		float radius = lineRenderer.startWidth * 0.5f;
		Vector2 velocity = Vector2.Scale(transform.up, transform.lossyScale);
		LayerMask layerMask = Physics2D.DefaultRaycastLayers;
		float mass = Mathf.PI*lineRenderer.startWidth;

		List<Vector2> path = CircularGravityBody.CircleRayCastPath(origin, radius, velocity, mass, layerMask);
		float z = transform.position.z;
		lineRenderer.positionCount = path.Count;
		lineRenderer.SetPositions(path.Select(v => new Vector3(v.x, v.y, z)).ToArray());
	}
}
