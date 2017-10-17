using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtensionMethods
{
	public static class ComponentExtensions
	{
		public static void SendMessageDownwards(this Component component, string methodName, SendMessageOptions options)
		{
			component.SendMessage(methodName: methodName, options: options);
			foreach (Transform transform in component.transform)
			{
				transform.SendMessageDownwards(methodName: methodName, options: options);
			}
		}

		public static void SendMessageDownwards(this Component component, string methodName)
		{
			component.SendMessage(methodName: methodName);
			foreach (Transform transform in component.transform)
			{
				transform.SendMessageDownwards(methodName: methodName);
			}
		}

		public static void SendMessageDownwards(this Component component, string methodName, object value, SendMessageOptions options)
		{
			component.SendMessage(methodName: methodName, value: value, options: options);
			foreach (Transform transform in component.transform)
			{
				transform.SendMessageDownwards(methodName: methodName, value: value, options: options);
			}
		}

		public static void SendMessageDownwards(this Component component, string methodName, object value)
		{
			component.SendMessage(methodName: methodName, value: value);
			foreach (Transform transform in component.transform)
			{
				transform.SendMessageDownwards(methodName: methodName, value: value);
			}
		}
	}
}
