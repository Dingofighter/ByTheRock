using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

	public void loadByIndex(int sceneIndex)
    {
        Time.timeScale = 1.0f;
        //GameManager.instance.paused = true;
        SceneManager.LoadScene(sceneIndex);
    } 
}
