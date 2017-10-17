using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using ExtensionMethods;
using JetBrains.Annotations;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
public class CannonUI : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler {

	[HideInInspector]
	public Cannon cannon;

	private Image image;
	private RectTransform rect;
	private int pointerID = -2;

	private Vector2 pointerPosition = Vector2.zero;
	public Plane plane = new Plane(Vector3.forward, Vector3.zero);

	[UsedImplicitly]
	private void Awake() {
		image = GetComponent<Image>();
		rect = GetComponent<RectTransform>();
	}

	[UsedImplicitly]
	private void Start() {
		image.fillAmount = 1f / cannon.planet.cannons.Count;
	}

#if UNITY_EDITOR
	[UsedImplicitly]
	private void OnDrawGizmos()
	{
		if (pointerID == -2) return;

		Ray ray = Camera.main.ScreenPointToRay(pointerPosition);
		float distance;
		if (!plane.Raycast(ray, out distance)) return;

		Vector3 pos = ray.GetPoint(distance);
		Gizmos.DrawSphere(pos, 0.5f);
		Gizmos.DrawLine(cannon.FireFrom, pos);
	}
#endif

	[UsedImplicitly]
	private void Update() {
		cannon.isAiming = false;

		if (pointerID != -2) {
			Ray ray = Camera.main.ScreenPointToRay(pointerPosition);
			float distance;
			if (plane.Raycast(ray, out distance)) {
				// Set target to the hit
				cannon.isAiming = true;
				cannon.SetWorldTargetPosition(ray.GetPoint(distance));
			}
		}

		// Move so it overlays the cannon
		rect.anchoredPosition = Camera.main.WorldToCanvasPoint(cannon.planet.transform.position);
		rect.localEulerAngles = new Vector3(0, 0, cannon.transform.eulerAngles.z - image.fillAmount * 360);
	}

	public void OnBeginDrag(PointerEventData eventData) {
		image.color = new Color(1,0,0,.2f);
		pointerID = eventData.pointerId;

		pointerPosition = eventData.position;
	}

	public void OnDrag(PointerEventData eventData) {
		// Skip if its another pointer
		if (pointerID != eventData.pointerId) return;

		pointerPosition = eventData.position;
	}

	public void OnEndDrag(PointerEventData eventData) {
		// Skip if its another pointer
		if (pointerID != eventData.pointerId) return;
		
		image.color = Color.clear;
		pointerID = -2;

		if (!eventData.hovered.Contains(gameObject)) {
			// Only fire if not hovering the same object
			cannon.FireCannonBall();
		}

		// Reset target angle
		cannon.SetLocalTargetAngle(0);
	}

}
