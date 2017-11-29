﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Yarn.Unity;

[System.Serializable]
public enum CaptainsLogMenus
{
    all,
    main, 
    clueView,
    clueLog, 
    news, 
    crew
}

[System.Serializable]
public class UIController
{
    // submodule for the game controller that controls the UI
    public static UIController S; // singleton
    GameController controller;
    public Dictionary<CaptainsLogMenus, GameObject> captainsLogCanvases { get; private set; }

    [SerializeField]
    List<ClueInfo> _clueLog;

    [SerializeField]
    List<GameObject> _loggedCluePrefabs;

    public GameObject clueViewCanvas;
    public GameObject clueLogCanvas;

    public List<ClueInfo> clueLog
    {
        get { return _clueLog; }
        private set { _clueLog = value; }
    }
    public List<GameObject> loggedCluePrefabs
    {
        get { return _loggedCluePrefabs; }
        private set { _loggedCluePrefabs = value; }
    }

    public UIController()
    {
        Debug.Log("UI controller constructor!");
        S = this;
        clueLog = new List<ClueInfo>();
        loggedCluePrefabs = new List<GameObject>();
        
    }
    public void Initialize(GameController controller)
    {
        this.controller = controller;
        SetupCaptainsLog();
        /*
        clueLogCanvas.SetActive(true);
        //ClueViewManager.S = clueLogCanvas.GetComponent<ClueViewManager>();
        clueLogCanvas.SetActive(false);
        */
    }

    void SetupCaptainsLog()
    {
        captainsLogCanvases = new Dictionary<CaptainsLogMenus, GameObject>();
        captainsLogCanvases[CaptainsLogMenus.clueLog]  = clueLogCanvas;
        captainsLogCanvases[CaptainsLogMenus.clueView] = clueViewCanvas;
        
        Debug.Log("Set up the captain's log!");
    }

    /// <summary>
    /// Hides the part of the captain's log passed.
    /// </summary>
    /// <param name="menusToHide"></param>
    public void HideCaptainsLog(CaptainsLogMenus menusToHide = CaptainsLogMenus.all)
    {
        if (menusToHide == CaptainsLogMenus.all)
            foreach (GameObject logCanvas in captainsLogCanvases.Values)
                logCanvas.gameObject.SetActive(false);
        else
            captainsLogCanvases[menusToHide].SetActive(false);

    }

    /// <summary>
    /// Shows the part of the captain's log passed.
    /// </summary>
    /// <param name="menusToShow"></param>
    public void ShowCaptainsLog(CaptainsLogMenus menuToShow = CaptainsLogMenus.clueLog)
    {
        if (captainsLogCanvases.ContainsKey(menuToShow))
            captainsLogCanvases[menuToShow].SetActive(true);
        else
            throw new System.ArgumentException("Menu to show not yet implemented.");
    }
    
    public void AddToClueLog(string clueName)
    {
        ClueInfo clueInfo = ClueDatabase.S.GetClueInfo(clueName);

        // make sure this clue isn't already in the log
        if (!clueLog.Contains(clueInfo))
        {
            clueLog.Add(clueInfo);
            loggedCluePrefabs.Add(ClueDatabase.S.GetCluePrefab(clueName));
        }

        Debug.Log("Added new clue to log! Clue name: " + clueName);
    }

    public void ViewClue(string clueName)
    {
        ShowCaptainsLog(CaptainsLogMenus.clueView);

        // fetch the clue from the database, and use its clue item script to
        // help display it

        GameObject cluePrefab = ClueDatabase.S.GetCluePrefab(clueName.RemoveChar(' '));
        // ^We need all the spaces removed; prefab names cannot have spaces

        ClueItem itemScript = null;
        if (cluePrefab != null)
            itemScript = cluePrefab.GetComponent<ClueItem>();

        ClueItemInspector.S.DisplayClue(itemScript, false);
    }


}

public class GameController : MonoBehaviour 
{
	public static GameController S; // singleton

    [SerializeField]
    UIController uiController;

	DialogueRunner dialogueRunner;

	List<DrawerController> drawers = new List<DrawerController>();

	public bool gamePaused { get; private set; }
	public float timeScale = 1f;

	void Awake()
	{
		S = this;
		gamePaused = false;
        //uiController = new UIController();
        uiController.Initialize(this);
	}

	// Use this for initialization
	void Start () 
	{
		dialogueRunner = DialogueRunner.S;
	}
	
	// Update is called once per frame
	void Update () 
	{
        HandleControls();
	}

	void SetupDrawers()
	{
		/*
		 * Make it so when a drawer is opened, a textbox shows up describing what
		 * is inside the drawer before closing the drawer and allowing further 
		 * action from the player.
		 */

	}

	public bool RequestGamePause()
	{
		gamePaused = true;
		return gamePaused;
	}

	public bool RequestGameResume()
	{
		gamePaused = false;
		return !gamePaused;
	}

    /// <summary>
    /// Hides the part of the captain's log passed.
    /// </summary>
    /// <param name="menusToHide"></param>
    public void HideCaptainsLog(CaptainsLogMenus menusToHide = CaptainsLogMenus.all)
    {
        uiController.HideCaptainsLog(menusToHide);

    }

    /// <summary>
    /// Shows the part of the captain's log passed.
    /// </summary>
    /// <param name="menusToShow"></param>
    public void ShowCaptainsLog(CaptainsLogMenus menusToShow = CaptainsLogMenus.all)
    {
        uiController.ShowCaptainsLog(menusToShow);
    }

    [YarnCommand("AddClue")]
    public void AddToClueLog(string clueName)
    {
        uiController.AddToClueLog(clueName);
    }

    [YarnCommand("ViewClue")]
    public void ViewClue(string clueName)
    {
        // if the clueName is a var, treat its value as the actual clue name
        bool nameIsVar = clueName[0] == '$';
        Yarn.Value varVal;
        string effClueName;
        if (nameIsVar)
        {
            varVal = DialogueRunner.S.variableStorage.GetValue(clueName);
            effClueName = varVal.AsString;
        }
        else
            effClueName = string.Copy(clueName);
        uiController.ViewClue(effClueName);
    }

    void HandleControls()
    {
        // open the captain's log
        if (Input.GetKeyDown(KeyCode.Escape) && !ClueItemInspector.inspectingItem)
            uiController.ShowCaptainsLog(CaptainsLogMenus.clueLog);
        
    }
  
}
