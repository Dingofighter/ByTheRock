using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

    float startTimer;
    bool started;
    int sceneID;

    public GameObject theCamera;

    void Update()
    {
        if (started)
        {
            startTimer += Time.deltaTime * 60;

            if (startTimer >  85 * 60 + 350)
            {
                Debug.Log("starting game");
                Time.timeScale = 1.0f;
                //GameManager.instance.paused = true;
                SceneManager.LoadScene(sceneID);
            }
        }
    }

	public void loadByIndex(int sceneIndex)
    {
        started = true;
        sceneID = sceneIndex;
        Debug.Log("zooming");
        FindObjectOfType<cameraScript>().startMoving();
    } 
}
