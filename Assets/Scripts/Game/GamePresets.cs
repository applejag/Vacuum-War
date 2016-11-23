using UnityEngine;
using System.Collections;

[DisallowMultipleComponent]
public sealed class GamePresets : SingletonBase<GamePresets> {

	[Header("Prefabs")]
	public GameObject _cannon;
	public GameObject _cannonball;
	public GameObject _cannonballExplosion;
	public GameObject _cannonUI;
	[Header("Existing objects")]
	public Canvas _canvas;
	private RectTransform _canvasRect;
	[Header("Public variables")]
	public LayerMask _mouseLayerMask = 1;
	public float _raycastLengthLimit = 200;

	// Prefabs
	public static GameObject cannon { get { return instance._cannon; } }
	public static GameObject cannonball { get { return instance._cannonball; } }
	public static GameObject cannonballExplosion { get { return instance._cannonballExplosion; } }
	public static GameObject cannonUI { get { return instance._cannonUI; } }
	// Existing objects
	public static Canvas canvas { get { return instance._canvas; } }
	public static RectTransform canvasRect { get { return instance._canvasRect; } }
	// Public variables
	public static LayerMask mouseLayerMask { get { return instance._mouseLayerMask; } }
	public static float raycastLengthLimit { get { return instance._raycastLengthLimit; } }

	protected override void Awake() {
		base.Awake();
		_canvasRect = _canvas.GetComponent<RectTransform>();
	}
}
