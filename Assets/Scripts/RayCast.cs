using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour
{




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
