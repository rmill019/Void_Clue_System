using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueItem : MonoBehaviour {

	public string name;
	public int rating;
	private string description;
	public int xmlIndex;
	public int pairedItemXmlIndex;

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
		
		
}
