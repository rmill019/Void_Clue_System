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

	void Start () {
		 
		Name = "SquareBar";
		Rating = 4;
		Description = "This is an ordinary SquareBar";
		XMLIndex = 1;
		PairedItemXMLIndex = 3;

		print ("Name: " + this.Name + " Rating: " + this.Rating + "\n Description: " + this.Description
		+ "\n" + "XML Index: " + this.XMLIndex + " Paired Item Index: " + this.PairedItemXMLIndex);
	}
}
