﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasClueObject : MonoBehaviour {

    public static CanvasClueObject S;
    public GameObject cloneClue;
    public ClueItem cloneClueItem;

    //---------------**TEMP**---------------//
    public GameObject screenBack;
    public GameObject currentRoom;

    //---------------**TEMP**---------------//


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Awake() {
        S = this;
    }

    void OnEnable()
    {
        //---------------**TEMP**---------------//
        //screenBack.SetActive(true);
        currentRoom.SetActive(false);
        //---------------**TEMP**---------------//
    }

    public void SetCloneClue(ref GameObject passedClone)
    {
        cloneClue = passedClone;
        cloneClueItem = passedClone.GetComponent<ClueItem>();
    }

    public void Close()
    {
        if (cloneClue != null)
            ClueItemInspector.S.ResetCurrentClue();
        //Destroy(cloneClue);

        //screenBack.SetActive(false);
        currentRoom.SetActive(true);
        gameObject.SetActive(false);
    }
}
