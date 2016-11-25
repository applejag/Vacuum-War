using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using ExtensionMethods;

[RequireComponent(typeof(Image))]
[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
public class CannonUI : EventTrigger {

	[System.NonSerialized]
	public Cannon cannon;

	private Image image;
	private RectTransform rect;
	private int? pointerID = null;

	private Vector2 pointerPosition = Vector2.zero;

	void Awake() {
		image = GetComponent<Image>();
		rect = GetComponent<RectTransform>();
	}

	void Start() {
		image.fillAmount = 1f / cannon.planet.cannons.Count;
	}

	void Update() {
		if (pointerID.HasValue) {
			// Calculate X and Z where Y is 0
			Ray ray = Camera.main.ScreenPointToRay(pointerPosition);
			RaycastHit hit;
			
			if (Physics.Raycast(ray, out hit, GamePresets.raycastLengthLimit, GamePresets.mouseLayerMask)) {
				// Set target to the hit
				cannon.SetWorldTargetAngle((hit.point - cannon.transform.position).xz().ToDegrees());
			}
			
		}

		// Move so it overlays the cannon
		rect.anchoredPosition = Camera.main.WorldToCanvasPoint(cannon.planet.transform.position);
		rect.localEulerAngles = -Vector3.forward * (cannon.transform.eulerAngles.y + image.fillAmount * 360);
	}

	public override void OnBeginDrag(PointerEventData eventData) {
		image.color = new Color(1,0,0,.2f);
		pointerID = eventData.pointerId;

		pointerPosition = eventData.position;
	}

	public override void OnDrag(PointerEventData eventData) {
		// Skip if its another pointer
		if (pointerID != eventData.pointerId) return;

		pointerPosition = eventData.position;
	}

	public override void OnEndDrag(PointerEventData eventData) {
		// Skip if its another pointer
		if (pointerID != eventData.pointerId) return;
		
		image.color = Color.clear;
		pointerID = null;

		if (!eventData.hovered.Contains(gameObject)) {
			// Only fire if not hovering the same object
			cannon.FireCannonBall();
		}

		// Reset target angle
		cannon.SetLocalTargetAngle(0);
	}

}
