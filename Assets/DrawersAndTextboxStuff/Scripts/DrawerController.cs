using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Yarn.Unity;

public class DrawerController : MonoBehaviour 
{
	public Vector3 slideMovement;

	public float slideTime = 2f;
	bool isSlidedOut = false;
	static bool sliding = false;

	Vector3 originalPos;

	public string textStartNode;
	GameController gameController;

	static int drawersSlidableAtOnce = 1;
	static int drawersSlided = 0;

	// Use this for initialization
	void Awake () 
	{
		originalPos = transform.position;	
	}

	void Start()
	{
		gameController = GameController.instance;
		DialogueUITest dialogueUI = DialogueRunner.instance.dialogueUI as DialogueUITest;

		dialogueUI.EndedDialogue.AddListener (this.SlideIn);
		//dialogueUI.EndedDialogue.AddListener (() => DrawerController.sliding = false);
		////Debug.Log ("SlideIn listener added!");
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void OnMouseOver()
	{
		if (Input.GetMouseButtonDown (0) && !ClueItemInspector.inspectingItem) 
		{
			////Debug.Log ("Left clicked!");
			if (!isSlidedOut)
				SlideOut ();
			else
				SlideIn ();
		}
	}

	public void SlideOut()
	{
		if (!sliding && !gameController.gamePaused && drawersSlided < drawersSlidableAtOnce) 
		{
			//gameController.RequestGamePause ();
			if (!isSlidedOut)
				StartCoroutine (SlideOutCoroutine ());
		}
	}

	public void SlideIn()
	{
		if (!gameController.gamePaused && !sliding && this.isSlidedOut)
			StartCoroutine (SlideInCoroutine ());
	}

	IEnumerator SlideInCoroutine()
	{
		//Debug.Log ("Sliding in!");
		sliding = true;
		float frameRate = 1f / Time.smoothDeltaTime;
		float timer = 0;
		float framesToPass = frameRate * slideTime;

		Vector3 targetPos = transform.localPosition - slideMovement;

		while (timer < framesToPass) 
		{
			transform.localPosition = Vector3.Lerp (transform.localPosition, targetPos, timer / framesToPass);
			timer++;
			yield return null;
		}

		transform.localPosition = targetPos;
		isSlidedOut = false;
		sliding = false;
		drawersSlided--;
		yield return null;

	}

	IEnumerator SlideOutCoroutine()
	{
		//Debug.Log ("Sliding out!");

		sliding = true;
		float frameRate = 1f / Time.smoothDeltaTime;
		float timer = 0;
		float framesToPass = frameRate * slideTime;

		Vector3 targetPos = transform.localPosition + slideMovement;

		while (timer < framesToPass) 
		{
			transform.localPosition = Vector3.Lerp (transform.localPosition, targetPos, timer / framesToPass);
			timer++;
			yield return null;
		}
		RunDialogue ();
		transform.localPosition = targetPos;
		this.isSlidedOut = true;
		sliding = false;
		drawersSlided++;

		yield return null;


	}

	void RunDialogue()
	{
		
		DialogueRunner.instance.StartDialogue (textStartNode);
	}
}
