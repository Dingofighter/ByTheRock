using UnityEngine;
using System.Collections;

public class GameManager1 : MonoBehaviour {

    public static GameManager1 instance = null;
    
    public GameObject canvas;
    public bool paused;
    public bool talking;
    public bool shoulderView;
    public int currScene;
    public int currSecondScene;
    public bool secondSceneLoaded;
    public bool crouching;
    public bool playerDead;
    public int currentWave;
    public int currentCash;
    public bool inShop;

    public bool bought1;
    public bool bought2;
    public bool bought3;
    public bool bought4;
    public int dmg = 1;

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
    }

    public void Update()
    {

        if (Input.GetButtonDown("Pause"))
        {
            TogglePause();
        }
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

    public void toggleShop()
    {
        if (inShop)
        {
            inShop = false;
            Time.timeScale = 1.0f;
            paused = false;
        }
        else
        {
            inShop = true;
            Time.timeScale = 0f;
            paused = true;
        }
    }

}
