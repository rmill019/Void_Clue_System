using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueItem : MonoBehaviour {

	public string name;
	public int rating;
	public string description;
	public int xmlIndex;
	public int pairedItemXmlIndex;
	public bool isInspectable = false;
	public float rotateSpeed = 25f;

	// Properties for each variable
	public string Name
	{
		get { return (name); }

		set { name = value; }
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

	void Update ()
	{
		InspectItem ();
	}

	private void InspectItem ()
	{
		// If the item is inspectable
		if (isInspectable)
		{
			// Rotate object based on the button pressed
			// TODO Objects not rotating as preferred.
			if (Input.GetKey(KeyCode.W))
			{
				print ("Inspecting Item");
				transform.rotation *= Quaternion.Euler (new Vector3(rotateSpeed * Time.deltaTime, 0f, 0f));
			}
			if (Input.GetKey (KeyCode.S))
			{
				transform.rotation *= Quaternion.Euler (new Vector3(-rotateSpeed * Time.deltaTime, 0f, 0f));
			}
			if (Input.GetKey (KeyCode.A))
			{
				transform.rotation *= Quaternion.Euler (new Vector3(0f, rotateSpeed * Time.deltaTime, 0f));
			}
			if (Input.GetKey (KeyCode.D))
			{
				transform.rotation *= Quaternion.Euler (new Vector3(0, -rotateSpeed * Time.deltaTime, 0f));
			}
		}
	}
		
}
