using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour {

	private Camera mainCam;
	public float rayCastDistance = 20;

	void Start ()
	{
		mainCam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
		Vector3 vectorToLamp = GameObject.Find ("Lamp").transform.position - mainCam.transform.position;
		print ("Vector to Lamp: " + vectorToLamp);
		Debug.DrawRay (mainCam.transform.position, vectorToLamp, Color.green, 2f);
		RaycastHit objectHit;
//		if (Physics.Raycast (mainCam.transform.position, vectorToLamp, out objectHit))
//		{
//			print ("We hit: " + objectHit.collider.gameObject.name + "\n"
//				+ "Name: " + objectHit.collider.gameObject.GetComponent<ClueItem>().Name + "\n"
//				+ "Rating: " + objectHit.collider.gameObject.GetComponent<ClueItem>().Rating + "\n"
//				+ "Description: " + objectHit.collider.gameObject.GetComponent<ClueItem>().Description);
//		}
	}

	void Update ()
	{
		if (Input.GetMouseButtonUp (0))
		{
			print ("Mouse Button Released");
			Ray pos = mainCam.ScreenPointToRay (Input.mousePosition);
			print ("Position: " + pos);
			RaycastHit objectHit;
			Debug.DrawRay (pos.origin, pos.direction * rayCastDistance, Color.red, 5f);
			if (Physics.Raycast (pos.origin, pos.direction, out objectHit, rayCastDistance))
			{
				print ("We hit: " + objectHit.collider.gameObject.name + "\n"
					+ "Name: " + objectHit.collider.gameObject.GetComponent<ClueItem>().Name + "\n"
					+ "Rating: " + objectHit.collider.gameObject.GetComponent<ClueItem>().Rating + "\n"
					+ "Description: " + objectHit.collider.gameObject.GetComponent<ClueItem>().Description);
			}

		}
	}

}
