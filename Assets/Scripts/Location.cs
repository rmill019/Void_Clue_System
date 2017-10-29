using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


[System.Serializable]
public class Location {

	[SerializeField]
	Vector3 _coords;
	[SerializeField]
	string _name = "";

	public Vector3 coords { get { return _coords; } set { _coords = value; } }
	public string name { get { return _name; } set { _name = value; } }

	public Location()
	{
		coords = Vector3.zero;
		name = "";
	}

	public Location(string name, Vector3 coords)
	{
		this.name = name;
		this.coords = coords;
	}
}
