using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TeaspoonTools.TextboxSystem;
using Yarn.Unity;

public class RayCast : MonoBehaviour {

	private Camera mainCam;
	public float rayCastDistance = 20;
	private Vector3 _lightOffset;

	GameObject inspectionLight = null; // cache, so we can destroy and recreate it as needed
	ClueItemController lastClueFound;
	Light sceneLight;

	IEnumerator darkenSceneCoroutine = null; 
	IEnumerator lightenSceneCoroutine = null;


	void Awake ()
	{
		mainCam = GameObject.FindGameObjectWithTag ("MainCamera").GetComponent<Camera>();
		_lightOffset = new Vector3 (0, 2, -3);
		sceneLight = GameObject.Find ("Directional Light").GetComponent<Light> ();
	}

	void Update ()
	{
		
		ClickItem ();

		if (lastClueFound != null && !lastClueFound.isInspectable && inspectionLight != null) 
		{
			if (darkenSceneCoroutine != null)
				StopCoroutine (darkenSceneCoroutine);

			if (lightenSceneCoroutine == null) 
			{
				Debug.Log ("Lightening things up!");
				lightenSceneCoroutine = ChangeSceneLight (1f, 1f);
				StartCoroutine (lightenSceneCoroutine);
			}


			Destroy (inspectionLight);
		}
		
	}


	void ClickItem ()
	{
		if (Input.GetMouseButtonUp (0))
		{
			print ("Mouse Button Released");
			Ray pos = mainCam.ScreenPointToRay (Input.mousePosition);
			print ("Position: " + pos);
			RaycastHit objectHit;
			RaycastHit[] objectsHit;
			Debug.DrawRay (pos.origin, pos.direction * rayCastDistance, Color.red, 5f);


			/*
			// find and inspect the first object hit that has a clue item controller
			objectsHit = Physics.RaycastAll(pos, rayCastDistance);
			ViewFirstClueItemHit (objectsHit);
			*/


			if (Physics.Raycast (pos.origin, pos.direction, out objectHit, rayCastDistance))
				HandleClueViewing(ref objectHit);


			
		}
	}

	void ViewFirstClueItemHit(RaycastHit[] hits)
	{
		GameObject objectHit;
		ClueItemController controller;

		RaycastHit nothing = new RaycastHit ();
		RaycastHit hitFound = nothing;

		foreach (RaycastHit hit in hits) 
		{
			objectHit = hit.collider.gameObject;
			controller = objectHit.GetComponent<ClueItemController> ();

			if (controller != null) 
				hitFound = hit;
			
		}

		if (hitFound.collider != nothing.collider)
			HandleClueViewing (ref hitFound);

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
		if (objectHit.collider.gameObject.CompareTag("Clue")) 
		{
			// Set clueItem to the object that we just hit with the Raycast
			GameObject clueObject = objectHit.collider.gameObject;

			lastClueFound = clueObject.GetComponent<ClueItemController> ();

			// Return the ClueItem information stored in the Clue Item we just clicked on 
			// and log it to the console.
			print (lastClueFound.ToString ()); // overrode its ToString method. 

			lastClueFound.isInspectable = true;

			// make the light on the scene less bright
			if (lightenSceneCoroutine != null)
				StopCoroutine(lightenSceneCoroutine);

			lightenSceneCoroutine = null;
			darkenSceneCoroutine = ChangeSceneLight(0.25f, 1f);
			StartCoroutine(darkenSceneCoroutine);

			// Set the desired position for viewing/inspecting the clicked on ClueItem, 
			// all based on that item's size
			Vector3 offset = Camera.main.transform.rotation * Vector3.forward * 2.5f;

			Vector3 desiredViewingLocation = Camera.main.transform.position + offset;

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

	IEnumerator ChangeSceneLight(float newIntensity, float changeTime)
	{
		float frameRate = 1f / Time.deltaTime;
		float framesToPass = frameRate * changeTime;
		float timer = 0;

		float baseIntensity = sceneLight.intensity;

		while (timer < framesToPass) 
		{
			sceneLight.intensity = Mathf.Lerp (baseIntensity, newIntensity, timer / framesToPass);
			timer++;
			yield return null;
		}

		yield return null;

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
