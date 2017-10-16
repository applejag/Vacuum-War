using UnityEngine;
using System.Collections;
using ExtensionMethods;

public sealed class PlayerControlledPlanet : Planet {
	
	void Start() {
		// Spawn UI Controllers
		foreach(Cannon cannon in cannons) {
			GameObject clone = Instantiate(GamePresets.cannonUI, GamePresets.canvas.transform, false) as GameObject;
			clone.GetComponent<CannonUI>().cannon = cannon;
		}
	}
	
}
