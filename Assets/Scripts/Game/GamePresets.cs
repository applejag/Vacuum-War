using UnityEngine;
using System.Collections;
using JetBrains.Annotations;

[DisallowMultipleComponent]
public sealed class GamePresets : SingletonBase<GamePresets> {

	[Header("Prefabs")]
	public GameObject _cannon;
	public GameObject _cannonballExplosion;
	public GameObject _cannonUI;
	[Header("Existing objects")]
	public Canvas _canvas;
	private RectTransform _canvasRect;
	[Header("Public variables")]
	public float _raycastTimeLimit = 200;
	public float _raycastGravity = 2;

	// Prefabs
	public static GameObject cannon => instance._cannon;
	public static GameObject cannonballExplosion => instance._cannonballExplosion;
	public static GameObject cannonUI => instance._cannonUI;

	// Existing objects
	public static Canvas canvas => instance._canvas;
	public static RectTransform canvasRect => instance._canvasRect;

	// Public variables
	public static float RaycastTimeLimit => instance._raycastTimeLimit;
	public static float RaycastGravity => instance._raycastGravity;

	[UsedImplicitly]
	private void Awake() {
		_canvasRect = _canvas.GetComponent<RectTransform>();
	}
}
