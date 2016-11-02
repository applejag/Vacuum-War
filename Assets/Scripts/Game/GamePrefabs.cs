using UnityEngine;
using System.Collections;

public sealed class GamePrefabs : SingletonBase<GamePrefabs> {

	public GameObject _cannon;
	public GameObject _cannonBall;

	public static GameObject cannon { get { return instance._cannon; } }
	public static GameObject cannonBall { get { return instance._cannonBall; } }

}
