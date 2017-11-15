using System;
using UnityEngine;

public static class GameObjectExtensions
{
	public static void SetVisibility(this GameObject gameObject, bool visibility, bool recursive = false)
	{
		gameObject.GetComponent<Renderer>().enabled = visibility;

		if (recursive)
			foreach (Transform transform in gameObject.transform)
				transform.gameObject.SetVisibility (visibility, recursive);
			
	}
}

