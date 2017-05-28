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
            //SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
            StartCoroutine("LoadSubScene");
            GameManager.instance.secondSceneLoaded = true;
            canLoad = false;
        }
        if (canUnload)
        {
            //SceneManager.UnloadScene(scene);
            StartCoroutine("UnloadSubScene");
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

    IEnumerator LoadSubScene()
    {
        yield return new WaitForEndOfFrame();
        SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive);
    }

    IEnumerator UnloadSubScene()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Bilder till story board"));
        yield return new WaitForEndOfFrame();
        SceneManager.UnloadScene(scene);
    }
}

