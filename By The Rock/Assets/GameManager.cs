using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    public UIManager UI;
    public GameObject canvas;
    public bool paused;

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
        //DontDestroyOnLoad(gameObject);
    }

    public void TogglePauseMenu()
    {
        //Debug.Log(canvas.activeInHierarchy());
        if (canvas.activeInHierarchy)
        {
            canvas.SetActive(false);
            Time.timeScale = 1.0f;
            paused = false;
        }
        else
        {
            canvas.SetActive(true);
            Time.timeScale = 0f;
            paused = true;
        }
        /*
        if (UI.GetComponentInChildren<Canvas>().enabled)
        {
            UI.GetComponentInChildren<Canvas>().enabled = false;
            Time.timeScale = 1.0f;
        }
        else
        {
            UI.GetComponentInChildren<Canvas>().enabled = true;
            Time.timeScale = 0f;
        }*/

        Debug.Log("GAMEMANAGER:: TimeScale: " + Time.timeScale);
    }

}
