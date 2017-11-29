using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryClueButton : Button
{
    // Handles how the clue buttons in the inventory part of the clue
    // UI work.

    public new RectTransform rectTrans;
    public ClueItem clue { get; private set; } // clue mapped to this S
    bool clueBeingDisplayed = false;

    protected override void Awake()
    {
        base.Awake();
        rectTrans = GetComponent<RectTransform>();
    }

    protected override void Start()
    {
        base.Start();
        ClueItemInspector.S.DoneDisplayingClue.AddListener(() => clueBeingDisplayed = false);
    }
    public void Init(ClueItem clueItem)
    {
        clue = clueItem;
        onClick.AddListener(GetClueDisplayed);
    }
	
    void GetClueDisplayed()
    {
        if (!clueBeingDisplayed)
        {
            ClueItemInspector.S.DisplayClue(clue);
            clueBeingDisplayed = true;
            
        }
    }
	
}
