using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    
    public GameObject canvas;
    public GameObject invCanvas;
    RectTransform rt;
    public bool paused;
    public bool talking;
    public bool farTalking;
    public bool givingItem;
    public bool shoulderView;
    public int currScene;
    public int currSecondScene;
    public bool secondSceneLoaded;
    public bool crouching;
    public bool showingInventory;
    public bool autoCloseInv;
    public bool fadeToBlack;
    float fadeTimer;
    
    public int itemID1 = -1;
    public int itemID2 = -1;
    public int itemID3 = -1;
    public int itemID4 = -1;

    //Game game;

    bool started;
    int invTimer;

    public FMOD.Studio.System _fmodSS;

    void Awake()
    {
        if (itemID1 == 0 && itemID2 == 0 && itemID3 == 0 && itemID4 == 0)
        {
            itemID1 = -1;
            itemID2 = -1;
            itemID3 = -1;
            itemID4 = -1;
        }

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

        Game.current = new Game();
        SaveLoad.Load();

        /*
        itemID1 = Game.current.invSlot1;
        itemID2 = Game.current.invSlot2;
        itemID3 = Game.current.invSlot3;
        itemID4 = Game.current.invSlot4;
        */

        /*
        changeItem(1, Game.current.invSlot2);
        changeItem(2, Game.current.invSlot3);
        changeItem(3, Game.current.invSlot4);
        */

        //game = new Game();

        //game.loadValues();


        FmodInitialize();
    }

    public void Update()
    {

        if (fadeToBlack)
        {
            fadeTimer += Time.deltaTime * 60;



        }


        if (!started)
        {
            bool remove = false;
            invCanvas.transform.position = new Vector3(invCanvas.transform.position.x - 350, invCanvas.transform.position.y, invCanvas.transform.position.z);

            if (Game.current.invSlot1 == -1) remove = true;
            changeItem(Game.current.invSlot1, true, remove);
            remove = false;
            
            if (Game.current.invSlot2 == -1) remove = true;
            changeItem(Game.current.invSlot2, true, remove);
            remove = false;
            
            if (Game.current.invSlot3 == -1) remove = true;
            changeItem(Game.current.invSlot3, true, remove);
            remove = false;

            if (Game.current.invSlot4 == -1) remove = true;
            changeItem(Game.current.invSlot4, true, remove);
            remove = false;

            started = true;
        }

        if (Input.GetButtonDown("Pause"))
        {
            TogglePause();
        }

        if (Input.GetButtonDown("Inventory"))
        {
            if (!showingInventory)
            {
                invTimer = 0;
                showingInventory = true;
                Debug.Log("pressed");
                ShowInventory(false);
            }
            else
            {
                if (invTimer > 25 && invTimer <= 100) invTimer = 100;
                autoCloseInv = true;
            }
        }

        if (showingInventory)
        {
            if (autoCloseInv || invTimer < 100)
            {
                invTimer++;
            }
            if (itemID1 == -1 && itemID2 == -1 && itemID3 == -1 && itemID4 == -1) invTimer = 127;
            if (invTimer <= 25) invCanvas.transform.position = new Vector3(invCanvas.transform.position.x + 14, invCanvas.transform.position.y, invCanvas.transform.position.z);
            else if (invTimer > 100 && invTimer <= 125) invCanvas.transform.position = new Vector3(invCanvas.transform.position.x - 14, invCanvas.transform.position.y, invCanvas.transform.position.z);
            else if (invTimer > 126)
            {
                showingInventory = false;
                invTimer = 0;
                autoCloseInv = false;
                SaveLoad.Save();
            }
        }
    }

    public void changeItem(int itemID, bool load, bool remove)
    {
        if (itemID != -1 && !load) ShowInventory(true);

        /*
        if (slot == 0) itemID1 = itemID;
        if (slot == 1) itemID2 = itemID;
        if (slot == 2) itemID3 = itemID;
        if (slot == 3) itemID4 = itemID;
        */
        //Debug.Log(itemID1);
        
        if (!remove) invCanvas.GetComponent<itemManager>().addItem(itemID);
        else invCanvas.GetComponent<itemManager>().removeItem(itemID);

        /*
        Game.current.invSlot1 = itemID1;
        Game.current.invSlot2 = itemID2;
        Game.current.invSlot3 = itemID3;
        Game.current.invSlot4 = itemID4;
        */
         
        /*
        game.invSlot1 = itemID1;
        game.invSlot2 = itemID2;
        game.invSlot3 = itemID3;
        game.invSlot4 = itemID4;
        game.updateValues();*/
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

    public void ShowInventory(bool automatic)
    {
        if (!showingInventory)
        {
            if (automatic) autoCloseInv = true;
            showingInventory = true;
        }
    }

    public void CloseInventory()
    {
        if (showingInventory) autoCloseInv = true;
    }

    private void FmodInitialize()
    {
        _fmodSS = FMODUnity.RuntimeManager.StudioSystem; //Script enabler
        FMOD.Studio.CPU_USAGE _fmodCPU;
        _fmodSS.getCPUUsage(out _fmodCPU); //Shows cpu usage
    }

}
