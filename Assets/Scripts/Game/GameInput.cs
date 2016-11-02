using UnityEngine;
using System.Collections;

public sealed class GameInput : SingletonBase<GameInput> {

	public LayerMask mouseRaycastLayer = 0;
	private static Vector3? _mousePosition = null;
	public static Vector3 mousePosition { get { return _mousePosition.HasValue ? _mousePosition.Value : Vector3.zero; } }
	public static bool mousePresent { get { return _mousePosition.HasValue; } }

	void Update() {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, Mathf.Infinity, mouseRaycastLayer)) {
			_mousePosition = hit.point;
		} else {
			_mousePosition = null;
		}

	}

}
