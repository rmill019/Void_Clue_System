using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueItem : MonoBehaviour {

	public string itemName;
	public int rating;
	public string description;
	public int xmlIndex;
	public int pairedItemXmlIndex;
	public bool isInspectable = false;
	public float rotateSpeed = 25f;

	private Rigidbody _rb;

	void Start () {
		_rb = GetComponent<Rigidbody> ();
	}


	void Update ()
	{
		InspectItem ();
	}



	private void InspectItem ()
	{
		// Locate and assign the center of the ClueItem to centerOfItem
		Vector3 centerOfItem = GetComponent<Collider> ().bounds.center;

		// If the item is inspectable we handle input to rotate the object. 
		// W and S rotate it around the x-axis while A and D rotate around the y-axis
		if (isInspectable)
		{
			// Remove Gravity so the item does not fall down while inspecting
			_rb.useGravity = false;
			// Rotate object based on the button pressed
			// TODO Objects not rotating as preferred.
			if (Input.GetKey (KeyCode.W))
			{
				_rb.constraints = RigidbodyConstraints.FreezePosition;
				transform.RotateAround (centerOfItem, Vector3.right, rotateSpeed * Time.deltaTime);
			} 
			else if (Input.GetKeyUp (KeyCode.W))
			{
				_rb.constraints = RigidbodyConstraints.None;
			} 
			else if (Input.GetKey (KeyCode.S))
			{
				_rb.constraints = RigidbodyConstraints.FreezePosition;
				transform.RotateAround (centerOfItem, Vector3.right, -rotateSpeed * Time.deltaTime);
			} 
			else if (Input.GetKeyUp (KeyCode.S))
			{
				_rb.constraints = RigidbodyConstraints.None;
			}
			else if (Input.GetKey (KeyCode.A))
			{
				_rb.constraints = RigidbodyConstraints.FreezePosition;
				transform.RotateAround (centerOfItem, Vector3.up, rotateSpeed * Time.deltaTime);
			}
			else if (Input.GetKeyUp (KeyCode.A))
			{
				_rb.constraints = RigidbodyConstraints.None;
			}
			else if (Input.GetKey (KeyCode.D))
			{
				_rb.constraints = RigidbodyConstraints.FreezePosition;
				transform.RotateAround (centerOfItem, Vector3.up, -rotateSpeed * Time.deltaTime);
			}
			else if (Input.GetKeyUp (KeyCode.D))
			{
				_rb.constraints = RigidbodyConstraints.None;
			}
		}
	}


	/********************************************************
	 * 														*
	 * Defining Properties for each variable 				*
	 *														*
	 *******************************************************/
	public string ItemName
	{
		get { return (itemName); }

		set { itemName = value; }
	}

	public int Rating 
	{
		get { return (rating); }

		set { rating = value; }
	}

	public string Description
	{
		get { return (description); }

		set { description = value; }
	}

	public int XMLIndex
	{
		get { return (xmlIndex); }

		set { xmlIndex = value; }
	}

	public int PairedItemXMLIndex
	{
		get { return (pairedItemXmlIndex); }

		set { pairedItemXmlIndex = value; }
	}
		
}
