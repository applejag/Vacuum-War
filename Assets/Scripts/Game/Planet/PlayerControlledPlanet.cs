using UnityEngine;
using System.Collections;
using ExtensionMethods;
using JetBrains.Annotations;

public sealed class PlayerControlledPlanet : Planet {
	
	[UsedImplicitly]
	private void Start() {
		// Spawn UI Controllers
		foreach(Cannon cannon in cannons) {
			GameObject clone = Instantiate(GamePresets.cannonUI, GamePresets.canvas.transform, false);
			clone.GetComponent<CannonUI>().cannon = cannon;
		}
	}
	
}
