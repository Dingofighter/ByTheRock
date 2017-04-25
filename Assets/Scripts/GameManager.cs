﻿using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    
    public GameObject canvas;
    public GameObject invCanvas;
    RectTransform rt;
    public bool paused;
    public bool talking;
    public bool shoulderView;
    public int currScene;
    public int currSecondScene;
    public bool secondSceneLoaded;
    public bool crouching;
    public bool showingInventory;

    public int itemID1 = -1;
    public int itemID2 = -1;
    public int itemID3 = -1;
    public int itemID4 = -1;

    bool started;
    int invTimer;

    public FMOD.Studio.System _fmodSS;

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }
        //If instance already exists and it's not this:
        else if (instance != this)
        {
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        }
        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        rt = invCanvas.GetComponent<RectTransform>();
        rt.anchoredPosition = rt.anchoredPosition;
        FmodInitialize();

    }

    public void Update()
    {

        if (!started)
        {
            invCanvas.transform.position = new Vector3(invCanvas.transform.position.x - 250, invCanvas.transform.position.y, invCanvas.transform.position.z);
            started = true;
        }

        if (Input.GetButtonDown("Pause"))
        {
            TogglePause();
        }

        if (Input.GetButtonDown("Inventory") && !showingInventory)
        {
            invTimer = 0;
            showingInventory = true;
            Debug.Log("pressed");
            ShowInventory();
        }

        if (showingInventory)
        {
            invTimer++;
            if (itemID1 == -1 && itemID2 == -1 && itemID3 == -1 && itemID4 == -1) invTimer = 127;
            if (invTimer <= 25) invCanvas.transform.position = new Vector3(invCanvas.transform.position.x + 10, invCanvas.transform.position.y, invCanvas.transform.position.z);
            else if (invTimer > 100 && invTimer <= 125) invCanvas.transform.position = new Vector3(invCanvas.transform.position.x - 10, invCanvas.transform.position.y, invCanvas.transform.position.z);
            else if (invTimer > 126) { showingInventory = false; invTimer = 0; }
        }
    }

    public void changeItem(int slot, int itemID)
    {
        if (slot == 0) itemID1 = itemID;
        Debug.Log(itemID1);
        invCanvas.GetComponent<itemManager>().addItem(slot, itemID);
    }

    public void TogglePause()
    {
        //Debug.Log(canvas.activeInHierarchy());
        if (canvas.activeInHierarchy)
        {
            canvas.SetActive(false);
            Time.timeScale = 1.0f;
            paused = false;
            if (!talking) Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            canvas.SetActive(true);
            Time.timeScale = 0f;
            paused = true;
        }
    }

    public void ShowInventory()
    {
        if (!showingInventory)
        {
            showingInventory = true;
        }
    }

    private void FmodInitialize()
    {
        _fmodSS = FMODUnity.RuntimeManager.StudioSystem; //Script enabler
        FMOD.Studio.CPU_USAGE _fmodCPU;
        _fmodSS.getCPUUsage(out _fmodCPU); //Shows cpu usage
    }

}