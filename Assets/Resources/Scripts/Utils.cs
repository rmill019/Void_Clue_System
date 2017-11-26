using System;
using UnityEngine;

public static class Utils
{
	/// <summary>
	/// Returns the rotation in euler angles you'd get if you made the first 
	/// transform LookAt the second transform.
	/// </summary>
	/// <returns>The look at rotation euler.</returns>
	/// <param name="a">The alpha component.</param>
	/// <param name="b">The blue component.</param>
	public static Vector3 GetLookAtRotationEuler(Transform first, Transform second)
	{
		GameObject thing = new GameObject ();
		thing.transform.position = first.position;
		thing.transform.rotation = first.rotation;

		thing.transform.LookAt (second);

		Vector3 result = thing.transform.rotation.eulerAngles;
		MonoBehaviour.Destroy (thing);

		return result;
	}

	public static Quaternion GetLookAtRotation(Transform first, Transform second)
	{
		GameObject thing = new GameObject ();
		thing.transform.position = first.position;
		thing.transform.rotation = first.rotation;

		thing.transform.LookAt (second);

		Quaternion result = thing.transform.rotation;
		MonoBehaviour.Destroy (thing);

		return result;
	}

	public static float FlexCeil(float num, int places)
	{
		num /= Mathf.Pow (10, places - 1);
		num = Mathf.Ceil (num);
		num /= Mathf.Pow (10, places - 1);
		return num;
	}
}
