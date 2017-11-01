using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Notes at the bottom of this script

public class ClueItem : MonoBehaviour {



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