using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Notes at the bottom of this script

public class ClueItemController : MonoBehaviour {

	#region Events

	#endregion

	#region Components
	private Rigidbody 				_rigidbody;
	private Collider 				collider;
	private Renderer 				_renderer;
	#endregion

	#region Fields
	// clue details
	[SerializeField]
	ClueItem clueItem;

	[SerializeField]
	private int 						_xmlIndex;
	[SerializeField]
	private int 						_pairedItemXmlIndex;

	[SerializeField]
	private float 						_rotateSpeed = 25f;

	bool isBeingInspected = false;
	Vector3 originalPos;
	Quaternion originalRotation;
	#endregion

	#region Properties to access those clue details.

	public Rigidbody rigidbody 
	{
		get { return _rigidbody;}
		set { _rigidbody = value; }
	}

	public Renderer renderer 
	{
		get { return _renderer;}
		set { _renderer = value; }
	}

	public string 						itemName
	{
		get { return (clueItem.name); }

		set { clueItem.name = value; }
	}

	public int 							rating 
	{
		get { return ( clueItem.rating); }

		set { clueItem.rating = value; }
	}

	public string 						description
	{
		get { return (clueItem.description); }

		set { clueItem.description = value; }
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
		get { return clueItem.isInspectable; }
		set { clueItem.isInspectable = value; }
	}

	public float rotateSpeed {
		get { return _rotateSpeed; }
		set { _rotateSpeed = value; }
	}
		
	Vector3 centerOfItem {get { return collider.bounds.center;}}
	// ^ better to have this as a property, so we don't have to update a var constantly in Update.
	// Can reduce how many calls on the collider are done, too, so potential performance boost!
	#endregion

	#region Other properties
	public bool isVisible
	{
		get { return renderer.enabled; }
	}

	#endregion

	#region Methods
	void Awake()
	{
		// better to set up component refs in Awake, since it executes before Start
		_rigidbody = 					GetComponent<Rigidbody> ();
		collider = 						GetComponent<Collider> ();
		renderer = 						GetComponent<Renderer> ();
		originalPos = 					transform.position;
		originalRotation = 				transform.rotation;
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
			isBeingInspected = true;

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

			if (Input.GetMouseButtonDown (1)) // right click to stop inspecting the item
				PutBackInOriginalPlace ();
			
		}
	}

	public override string ToString ()
	{
		// returns a string with the clue's basic info.
		string messageFormat = "We hit: {0}\nName: {1}\nRating: {2}\nDescription: {3}";

		return string.Format(messageFormat, this.gameObject, name, rating, description);
	}
		
	void OnMouseUp()
	{
		if (isInspectable) 
			Debug.Log (this.name + " was clicked!");
		
	}
		
	public void SetVisibility(bool visibility, bool recursive = false)
	{
		GetComponent<Renderer>().enabled = visibility;

		if (recursive)
			foreach (Transform transform in gameObject.transform)
				transform.gameObject.SetVisibility (visibility, recursive);

	}

	public void PutBackInOriginalPlace()
	{
		isInspectable = false;
		transform.rotation = originalRotation;
		transform.position = originalPos;
	}

	#endregion

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