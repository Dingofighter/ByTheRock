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

            if (Input.GetKeyDown(KeyCode.P))
            {
                startTimer = 85 * 60 + 350;
            }

            startTimer += Time.deltaTime * 60;

            if (startTimer >  85 * 60 + 350)
            {
                Debug.Log("starting game");
                Time.timeScale = 1.0f;
                if (FindObjectOfType<movieHandler>()._EventInstance != null)
                    FindObjectOfType<movieHandler>().stopAudio();
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

    public void backToMenu(int i)
    {
        AllFlags.Instance.Reset();
        SceneManager.LoadScene(0);
    }

    public void justStop()
    {
        var fireemitter = FindObjectOfType<FireEmitter>().GetComponent<FireEmitter>();
        var musicemitter = FindObjectOfType<MusicEmitter>().GetComponent<MusicEmitter>();

        fireemitter.StartFadeOut();
        musicemitter.StartFadeOut();
    }
}
