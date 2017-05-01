using UnityEngine;
using System.Collections;

public class Crosshair1 : MonoBehaviour {
    
    public Texture2D crosshairTexture;
    public float crosshairScale = 1;
    void OnGUI()
    {
        //if not paused
        if (Time.timeScale != 0 && GameManager1.instance.shoulderView)
        {
            
            //Debug.Log(WorldCamera.shoulderDistance);
            if (crosshairTexture != null)
                GUI.DrawTexture(new Rect((Screen.width - crosshairTexture.width * crosshairScale) / 2 - 60 + WorldCamera.shoulderDistance, 
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
