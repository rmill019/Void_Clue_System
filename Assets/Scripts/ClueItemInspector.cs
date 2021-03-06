﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueItemInspector : MonoBehaviour
{
    public ClueItem currentClue;
    // components
    private Collider currentClueCol;

    Vector3 cluePrevPos;
    Quaternion cluePrevRot;

    Vector3 centerOfItem { get { return currentClueCol.bounds.center; } }
    // ^ better to have this as a property, so we don't have to update a var constantly in Update.
    // Can reduce how many calls on the collider are done, too, so potential performance boost!

    // methods

    private Camera mainCam;
    public float rayCastDistance = 20;
    private Vector3 _lightOffset;

    GameObject inspectionLight = null; // cache, so we can destroy and recreate it as needed

    void Awake()
    {
        // better to set up component refs in Awake, since it executes before Start
		mainCam = Camera.main;
        _lightOffset = new Vector3(0, 2, -3);
    }

    void Update()
    {
        ClickItem();
        //Does not bother calling HandleRotation if the currentClue has already been set
        if (currentClue != null)
            HandleRotation(ref currentClue);
    }

   

	/********************************************
	* HELPER FUNCTIONS							*
	*********************************************/
	 void ClickItem()
    {
		//TODO Look into making this mobile compatible
        if (Input.GetMouseButtonUp(0))
        {
            print("Mouse Button Released");
            Ray pos = mainCam.ScreenPointToRay(Input.mousePosition);
            print("Position: " + pos);
            RaycastHit objectHit;
            Debug.DrawRay(pos.origin, pos.direction * rayCastDistance, Color.red, 5f);
			//TODO Put clue's on a specific layer and have the Raycast search only that layer. That way our raycasting is more efficient.
            if (Physics.Raycast(pos.origin, pos.direction, out objectHit, rayCastDistance))
            {
                if (currentClue != null)
                {
                    if (objectHit.collider.GetComponent<ClueItem>() != currentClue)
                        ResetCurrentClue();
                }

                if (objectHit.collider.gameObject.tag == "Clue")
                {
                    // Set clueItem to the object that we just hit with the Raycast
                    // cache the ClueItem script for a performance boost.
                    SetCurrentClue(ref objectHit);
                    // If the object we hit is tagged as a clue then bring it up for inspection
                    HandleClueViewing(ref currentClue);
                }

            }
        }
    }

    void SetCurrentClue(ref RaycastHit objectHit)
    {
        //Store the default position of the clue clicked
		if (currentClue == null)
		{
			{
				currentClue = objectHit.collider.GetComponent<ClueItem> ();
				currentClueCol = objectHit.collider;
				cluePrevPos = currentClue.transform.position;
				cluePrevRot = currentClue.transform.localRotation;

			}
		}
			
        //If we already had an old clue, put it back, then set up the newly clicked one

		else if (objectHit.collider.GetComponent<ClueItem>() != currentClue)
        {
            ResetCurrentClue();
        }

    }

    //
    void ResetCurrentClue()
    {
        currentClue.transform.position = cluePrevPos;
        currentClue.transform.rotation = cluePrevRot;
        currentClue.isInspectable = false;
        currentClue = null;
    }

    GameObject CreateLight(Vector3 position)
    {
        GameObject lightObject = new GameObject("Inspection Light");
        Light lightComponent = lightObject.AddComponent<Light>();
        lightComponent.type = LightType.Spot;
        lightComponent.intensity = 15f;
        lightComponent.range = 5f;
        lightComponent.spotAngle = 50f;
        lightComponent.color = Color.white;
        lightObject.transform.position = position;
        lightObject.transform.rotation = Quaternion.Euler(35, 0, 0);

        return lightObject;
    }

    void HandleClueViewing(ref ClueItem currentClue)
    {

        // Return the ClueItem information stored in the Clue Item we just clicked on 
        // and log it to the console.
        print(currentClue.ToString()); // overrode its ToString method. 

        currentClue.isInspectable = true;

        // Set the desired position for viewing/inspecting the clicked on ClueItem
        Vector3 desiredViewingLocation = mainCam.transform.position;
        desiredViewingLocation.x -= 1;
        desiredViewingLocation.z += 3;

        // Set up position to set up the light for inspecting the clueItem
        Vector3 clueLightLocation = desiredViewingLocation + _lightOffset;

        currentClue.transform.position = desiredViewingLocation;

        // Create a light to view inspectable clueItem
        if (inspectionLight == null)
            inspectionLight = CreateLight(clueLightLocation);

        //when we're not inspecting anything, we won't need the light
        else if (inspectionLight != null)
            Destroy(inspectionLight);
    }

    // helper methods
	//TODO This needs to work with mobile as well. Look into Unity's touch class to get an idea of how we can implement this with a touch screen.
    void HandleRotation(ref ClueItem currentClue)
    {

        // If the item is inspectable we handle input to rotate the object. 
        // W and S rotate it around the x-axis while A and D rotate around the y-axis
        if (currentClue.isInspectable)
        {
            // Rotate object based on the button pressed
            // TODO Objects not rotating as preferred.

//			// The following handles code for inspecting (rotating) ClueItems on PC
//            if (Input.GetKey(KeyCode.W))
//            {
//                currentClue.transform.RotateAround(centerOfItem, Vector3.right, currentClue.rotateSpeed * Time.deltaTime);
//            }
//            else if (Input.GetKeyUp(KeyCode.W))
//            {
//                //rigidbody.constraints = RigidbodyConstraints.None;
//            }
//            else if (Input.GetKey(KeyCode.S))
//            {
//                //rigidbody.constraints = RigidbodyConstraints.FreezePosition;
//                currentClue.transform.RotateAround(centerOfItem, Vector3.right, -currentClue.rotateSpeed * Time.deltaTime);
//            }
//            else if (Input.GetKeyUp(KeyCode.S))
//            {
//                //rigidbody.constraints = RigidbodyConstraints.None;
//            }
//            else if (Input.GetKey(KeyCode.A))
//            {
//                //rigidbody.constraints = RigidbodyConstraints.FreezePosition;
//                currentClue.transform.RotateAround(centerOfItem, Vector3.up, currentClue.rotateSpeed * Time.deltaTime);
//            }
//            else if (Input.GetKeyUp(KeyCode.A))
//            {
//                //rigidbody.constraints = RigidbodyConstraints.None;
//            }
//            else if (Input.GetKey(KeyCode.D))
//            {
//                //rigidbody.constraints = RigidbodyConstraints.FreezePosition;
//                currentClue.transform.RotateAround(centerOfItem, Vector3.up, -currentClue.rotateSpeed * Time.deltaTime);
//            }
//            else if (Input.GetKeyUp(KeyCode.D))
//            {
//                //rigidbody.constraints = RigidbodyConstraints.None;
//            }


			// The following handles code for inspecting (rotating) clues on Mobile
			Touch myTouch = Input.GetTouch (0);

			float x = myTouch.deltaPosition.x;
			float y = myTouch.deltaPosition.y;

			if (Mathf.Abs(y) > Mathf.Abs(x))
			{
				// We are using the y variable in AngleAxis because moving our finger up and down would 
				// make the object rotate on it's x (right) axis

				currentClue.transform.RotateAround (centerOfItem, Vector3.right, y * currentClue.rotateSpeed * Time.deltaTime);
//				rb.constraints = RigidbodyConstraints.FreezePosition;
//				rb.constraints = RigidbodyConstraints.FreezeRotationY;
//				rb.constraints = RigidbodyConstraints.FreezeRotationZ;
			}

			if (Mathf.Abs(x) > Mathf.Abs(y))
			{
				// We are using the x variable in AngleAxis because moving our finger left and right would 
				// make the object rotate on it's y (world up) axis

				currentClue.transform.RotateAround (centerOfItem, Vector3.up, x * currentClue.rotateSpeed * Time.deltaTime);
//				rb.constraints = RigidbodyConstraints.FreezePosition;
//				rb.constraints = RigidbodyConstraints.FreezeRotationX;
//				rb.constraints = RigidbodyConstraints.FreezeRotationZ;
			}
        }
    }
}
