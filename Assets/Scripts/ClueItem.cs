using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[System.Serializable]
public class ClueItem  
{

	#region Backing fields visible in the inspector

	[SerializeField]
	GameObject _model;

	[SerializeField]
	string _name;

	[SerializeField]
	string _description;

	[SerializeField]
	int _rating;

	[SerializeField]
	string _roomName;

	[SerializeField]
	Vector3 _coords;

	//Location _location;

	[SerializeField]
	bool _isInspectable;

	[SerializeField]
	List<ClueItem> _pairsWith;

	#endregion

	#region Properties you access those fields with through code
	public GameObject model 	{ get {	return _model; 	} set { _model = value; } }
	public string name 			{ get { return _name; 	} set { _name = value; } }
	public string description 	{ get { return _description; } set { _description = value; } }
	public int rating 			{ get { return _rating; } set { _rating = value; } }
	public string roomName 		{ get { return _roomName; } set { _roomName = value; } }
	public Vector3 coords 		{ get { return _coords; } set { _coords = value; } }
	public bool isInspectable 	{ get { return _isInspectable; } set { _isInspectable = value; } }
	//public Location location 	{ get { return _location; } set { _location = value; } }

	#endregion

}
