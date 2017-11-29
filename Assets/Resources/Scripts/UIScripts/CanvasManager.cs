using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour {

	//public GameObject targetCanvas;
	//public GameObject currentCanvas;
	public Text gemCountText;
	private bool activated;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	// This function sets the activated bool variable to true if
	// the canvas is currently active in the scene and sets it to false
	// if the canvas is currently inactive
	public void disableCanvas(GameObject currentCanvas)
	{
		activated = currentCanvas.activeInHierarchy;
		currentCanvas.SetActive(!activated);
	}

	// This function does the same as the above function but is supposed to activate the
	// target canvas They are named differently so that setting up games UI is easier.
	public void loadCanvas(GameObject targetCanvas)
	{
		activated = targetCanvas.activeInHierarchy;
		targetCanvas.SetActive(!activated);
	}
}