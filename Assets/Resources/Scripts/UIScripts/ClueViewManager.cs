using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TeaspoonTools.Utils;


public class ClueViewManager : MonoBehaviour
{
    // manages how the clues are shown in the clue view

    public static ClueViewManager instance;

    public GameObject buttonGroup;

    void Awake()
    {
        instance = this;
    }

    void SetupViewGroup()
    {
        // looks through all the clues in the clue log, and sets them
        // up in the view group

        List<ClueInfo> clueLog = UIController.instance.clueLog;
        List<GameObject> loggedCluePrefabs = UIController.instance.loggedCluePrefabs;

        // will be used in the following loop
        GameObject clueButtonGo;
        InventoryClueButton clueButton;
        GameObject textGo;
        Text buttonText;
        RectTransform textRect;
        RectTransform buttonRect;
        GridLayoutGroup layoutGroup = buttonGroup.GetComponent<GridLayoutGroup>();

        float buttonWidth = layoutGroup.cellSize.x;

        // instantiate buttons, each having the name of their clues
        // on them
        for (int i = 0; i < clueLog.Count; i++)
        {
            // the button
            clueButtonGo = new GameObject("Button " + (i + 1));
            clueButton = clueButtonGo.AddComponent<InventoryClueButton>();
            clueButton.Init(loggedCluePrefabs[i].GetComponent<ClueItem>());
            clueButton.targetGraphic = clueButtonGo.AddComponent<Image>();
            clueButtonGo.transform.SetParent(buttonGroup.transform);
            buttonRect = clueButtonGo.GetComponent<RectTransform>();

            // the text
            textGo = new GameObject("Text");
            buttonText = textGo.AddComponent<Text>();
            textGo.transform.SetParent(clueButtonGo.transform);
            buttonText.text = clueButton.clue.ItemName;
            buttonText.color = Color.black;
            buttonText.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            buttonText.resizeTextForBestFit = true;

            // place the text at the bottom, adjust its width and alignments
            textRect = textGo.GetComponent<RectTransform>();
            textRect.ApplyAnchorPreset(TextAnchor.LowerCenter, true, true);
            buttonText.alignment = TextAnchor.LowerCenter;
            textRect.sizeDelta = new Vector2(buttonWidth, textRect.sizeDelta.y);
            
        }
        
    }

    void ClearViewGroup()
    {
        foreach (Transform t in buttonGroup.transform)
            Destroy(t.gameObject);
    }
    private void OnEnable()
    {
        SetupViewGroup();
    }

    private void OnDisable()
    {
        ClearViewGroup();
    }

}
