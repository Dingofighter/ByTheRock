using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {
    
    public Texture2D crosshairTexture;
    public float crosshairScale = 1;
    void OnGUI()
    {
        //if not paused
        if (Time.timeScale != 0 && WorldCamera.shoulderView)
        {
            if (crosshairTexture != null)
                GUI.DrawTexture(new Rect((Screen.width - crosshairTexture.width * crosshairScale) / 2 - 50, 
                    (Screen.height - crosshairTexture.height * crosshairScale) / 2, 
                    crosshairTexture.width * crosshairScale, 
                    crosshairTexture.height * crosshairScale), 
                    crosshairTexture);
            else
                Debug.Log("No crosshair texture set in the Inspector");
        }
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
