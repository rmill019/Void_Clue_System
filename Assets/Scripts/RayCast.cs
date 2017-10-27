using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour {

	private Camera mainCam;
	public float rayCastDistance = 20;
	private Vector3 _lightOffset;

	GameObject inspectionLight = null; // cache, so we can destroy and recreate it as needed

	void Awake ()
	{
		mainCam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
		_lightOffset = new Vector3 (0, 2, -3);
	}

	void Update ()
	{
		ClickItem ();
	}


	void ClickItem ()
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
				
				// If the object we hit is tagged as a clue then bring it up for inspection
				HandleClueViewing(ref objectHit);

			}
		}
	}



	GameObject CreateLight (Vector3 position)
	{
		GameObject lightObject = new GameObject ("Inspection Light");
		Light lightComponent = lightObject.AddComponent<Light> ();
		lightComponent.type = LightType.Spot;
		lightComponent.intensity = 15f;
		lightComponent.range = 5f;
		lightComponent.spotAngle = 50f;
		lightComponent.color = Color.white;
		lightObject.transform.position = position;
		lightObject.transform.rotation = Quaternion.Euler (35, 0, 0);

		return lightObject;
	}

	// helper functions

	void HandleClueViewing(ref RaycastHit objectHit)
	{
		// If the object we hit is tagged as a clue then bring it up for inspection;
		if (objectHit.collider.gameObject.tag == "Clue") {
			// Set clueItem to the object that we just hit with the Raycast
			GameObject clueObject = objectHit.collider.gameObject;

			// cache the ClueItem script for a performance boost.
			// also, we should probably rename it to ClueItemController.
			ClueItem clueItemController = clueObject.GetComponent<ClueItem> ();

			// Return the ClueItem information stored in the Clue Item we just clicked on 
			// and log it to the console.
			print (clueItemController.ToString ()); // overrode its ToString method. 

			clueItemController.isInspectable = true;

			// Set the desired position for viewing/inspecting the clicked on ClueItem
			Vector3 desiredViewingLocation = mainCam.transform.position;
			desiredViewingLocation.x -= 1;
			desiredViewingLocation.z += 3;

			// Set up position to set up the light for inspecting the clueItem
			Vector3 clueLightLocation = desiredViewingLocation + _lightOffset;

			// Create a light to view inspectable clueItem
			if (inspectionLight == null)
				inspectionLight = CreateLight (clueLightLocation);
			clueObject.transform.position = desiredViewingLocation;
		} 
		else if (inspectionLight != null)
			// for when we're not inspecting anything, we won't need the light
			Destroy (inspectionLight);

	}

}





//		if (Input.GetMouseButtonUp (0))
//		{
//			print ("Mouse Button Released");
//			Ray pos = mainCam.ScreenPointToRay (Input.mousePosition);
//			print ("Position: " + pos);
//			RaycastHit objectHit;
//			Debug.DrawRay (pos.origin, pos.direction * rayCastDistance, Color.red, 5f);
//			if (Physics.Raycast (pos.origin, pos.direction, out objectHit, rayCastDistance))
//			{
//				GameObject clueItem = objectHit.collider.gameObject;
//				print ("We hit: " + objectHit.collider.gameObject.name + "\n"
//					+ "Name: " + clueItem.GetComponent<ClueItem>().Name + "\n"
//					+ "Rating: " + clueItem.GetComponent<ClueItem>().Rating + "\n"
//					+ "Description: " + clueItem.GetComponent<ClueItem>().Description);
//
//				// Change ClueItem isInspectable to true
//				clueItem.GetComponent<ClueItem>().isInspectable = true;
//
//				Vector3 desiredViewingLocation = mainCam.transform.position;
//				desiredViewingLocation.x -= 1;
//				desiredViewingLocation.z += 3;
//
//				objectHit.collider.gameObject.transform.position = desiredViewingLocation;
//			}
//
//		}
