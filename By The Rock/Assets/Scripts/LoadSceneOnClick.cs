using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadSceneOnClick : MonoBehaviour {

    float startTimer;

    public GameObject theCamera;

	public void loadByIndex(int sceneIndex)
    {
        startTimer += Time.deltaTime * 60;
        theCamera.transform.position = new Vector3(theCamera.transform.position.x, theCamera.transform.position.y, theCamera.transform.position.z + 50);


        Time.timeScale = 1.0f;
        //GameManager.instance.paused = true;
        //SceneManager.LoadScene(sceneIndex);
    } 
}
