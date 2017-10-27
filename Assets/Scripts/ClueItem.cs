using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Notes at the bottom of this script

public class ClueItem : MonoBehaviour {

	// components
	private Rigidbody 				rigidbody;
	private Collider 				collider;

	// fields
	// clue details

	[SerializeField]
	private string 						_itemName;
	[SerializeField]
	private int 						_rating;
	[SerializeField]
	private string 						_description;
	[SerializeField]
	private int 						_xmlIndex;
	[SerializeField]
	private int 						_pairedItemXmlIndex;
	[SerializeField]
	private bool 						_isInspectable = false;
	[SerializeField]
	private float 						_rotateSpeed = 25f;


	/*
	 * Properties to access those clue details.
	 */

	public string 						itemName
	{
		get { return (_itemName); }

		set { _itemName = value; }
	}

	public int 							rating 
	{
		get { return (_rating); }

		set { _rating = value; }
	}

	public string 						description
	{
		get { return (_description); }

		set { _description = value; }
	}

	public int 							xMLIndex
	{
		get { return (_xmlIndex); }

		set { _xmlIndex = value; }
	}

	public int 							pairedItemXMLIndex
	{
		get { return (_pairedItemXmlIndex); }

		set { _pairedItemXmlIndex = value; }
	}

	public bool 						isInspectable
	{
		get { return _isInspectable; }
		set { _isInspectable = value; }
	}

	public float rotateSpeed {
		get { return _rotateSpeed; }
		set { _rotateSpeed = value; }
	}
		
	Vector3 centerOfItem {get { return collider.bounds.center;}}
	// ^ better to have this as a property, so we don't have to update a var constantly in Update.
	// Can reduce how many calls on the collider are done, too, so potential performance boost!

	// methods
	void Awake()
	{
		// better to set up component refs in Awake, since it executes before Start
		rigidbody 			= 			GetComponent<Rigidbody> ();
		collider 			= 			GetComponent<Collider> ();
	}

	void Start () {
		
	}


	void Update ()
	{
		InspectItem ();
	}
		

	private void InspectItem ()
	{
		HandleRotation ();

	}

	// helper methods
	void HandleRotation()
	{

		// If the item is inspectable we handle input to rotate the object. 
		// W and S rotate it around the x-axis while A and D rotate around the y-axis
		if (isInspectable)
		{
			// Remove Gravity so the item does not fall down while inspecting
			rigidbody.useGravity = false;

			// Rotate object based on the button pressed
			// TODO Objects not rotating as preferred.

			// if a rotation key is pressed or held down, freeze the position
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || 
				Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
				rigidbody.constraints  		= 		RigidbodyConstraints.FreezePosition;

			else // otherwise, let the clue object be
				rigidbody.constraints 	= RigidbodyConstraints.None;

			if (Input.GetKey (KeyCode.W))
			{

				transform.RotateAround (centerOfItem, Vector3.right, rotateSpeed * Time.deltaTime);
			} 
			else if (Input.GetKeyUp (KeyCode.W))
			{
				rigidbody.constraints 	= 	RigidbodyConstraints.None;
			} 
			else if (Input.GetKey (KeyCode.S))
			{
				rigidbody.constraints 	= 	RigidbodyConstraints.FreezePosition;
				transform.RotateAround (centerOfItem, Vector3.right, -rotateSpeed * Time.deltaTime);
			} 
			else if (Input.GetKeyUp (KeyCode.S))
			{
				rigidbody.constraints 	= 	RigidbodyConstraints.None;
			}
			else if (Input.GetKey (KeyCode.A))
			{
				rigidbody.constraints 	= 	RigidbodyConstraints.FreezePosition;
				transform.RotateAround (centerOfItem, Vector3.up, rotateSpeed * Time.deltaTime);
			}
			else if (Input.GetKeyUp (KeyCode.A))
			{
				rigidbody.constraints 	= 	RigidbodyConstraints.None;
			}
			else if (Input.GetKey (KeyCode.D))
			{
				rigidbody.constraints 	= 	RigidbodyConstraints.FreezePosition;
				transform.RotateAround (centerOfItem, Vector3.up, -rotateSpeed * Time.deltaTime);
			}
			else if (Input.GetKeyUp (KeyCode.D))
			{
				rigidbody.constraints = RigidbodyConstraints.None;
			}
		}
	}

	public override string ToString ()
	{
		// returns a string with the clue's basic info.

		string messageFormat = "We hit: {0}\nName: {1}\nRating: {2}\nDescription: {3}";

		return string.Format(messageFormat, this.gameObject, name, rating, description);
	}

		
}


/*
 * There was no need to have the clue details (like name, desc, xml index) be public vars with 
 * public properties since the point of properties is to be able to control variable access without
 * always needing to keep them all private. 
 * 
 * Hence, I made the public vars into private backing fields, and I removed the capitalization
 * form the property names.
 * 
 */ 