using System;
using UnityEngine;
using System.Collections;
using ExtensionMethods;

public class MessageOnEnter : StateMachineBehaviour {

	public string messageName = "AnimationStart";
	public bool requireReceiver;
	[EnumFlags]
	public Option sendTo = Option.Self;

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
	{
		SendMessageOptions require = requireReceiver ? SendMessageOptions.RequireReceiver : SendMessageOptions.DontRequireReceiver;

		if (sendTo.HasFlag(Option.Self))
		{
			animator.SendMessage(messageName, require);
		}

		if (sendTo == Option.Upwards)
		{
			animator.transform.parent?.SendMessageUpwards(messageName, require);
		}

		if (sendTo.HasFlag(Option.Downwards))
		{
			foreach (Transform child in animator.transform) { 
				child.SendMessageDownwards(messageName, require);
			}
		}
	}
	
	[Flags]
	public enum Option {
		Self = 0x01,
		Upwards = 0x02,
		Downwards = 0x04,
	}
}
