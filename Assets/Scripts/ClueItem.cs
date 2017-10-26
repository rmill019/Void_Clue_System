using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueItem : MonoBehaviour {

	private string name;
	private int rating;
	private string description;
	private int xmlIndex;
	private int pairedItemXmlIndex;

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

	void Awake () {
		 
		Name = "Lamp";
		Rating = 4;
		Description = "This is an ordinary Lamp";
		XMLIndex = 1;
		PairedItemXMLIndex = 3;

		print ("Name: " + this.Name + "\n" + "Rating: " + this.Rating + "\nDescription: " + this.Description
		+ "\n" + "XML Index: " + this.XMLIndex + "\n" + "Paired Item Index: " + this.PairedItemXMLIndex);

		//print ("Camera Position: " + Camera.main.transform.position);
	}
		
}
