using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour {

    public string scene;
    public bool loadScene;
    bool canLoad;
    bool canUnload;

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
	
        if (canLoad)
        {
            SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            GameManager.instance.secondSceneLoaded = true;
            canLoad = false;
        }
        if (canUnload)
        {
            SceneManager.UnloadScene(scene);
            GameManager.instance.secondSceneLoaded = false;
            canUnload = false;
        }

	}

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Player")
        {
            if (loadScene && !GameManager.instance.secondSceneLoaded)
            {
                canLoad = true;
            }
            else if (!loadScene && GameManager.instance.secondSceneLoaded)
            {
                canUnload = true;
            }

            Debug.Log("player hit sceneboxz");

        }


    }
}

