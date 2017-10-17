using UnityEngine;

[DisallowMultipleComponent]
public abstract class SingletonBase<T> : MonoBehaviour where T : SingletonBase<T> {

	public static T instance;

	private void OnEnable() {
		instance = this as T;
	}

}
