using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class fadeManager : MonoBehaviour {

    public Image blackImage;

    float fadeTimer;
    bool fading;
    bool faded;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (fading)
        {
            if (fadeTimer < 150)
            {
                fadeTimer += Time.deltaTime * 60;
                if (!faded) blackImage.color = new Color(0, 0, 0, fadeTimer / 120);
                else blackImage.color = new Color(0, 0, 0, 1 - (fadeTimer / 120));
            }
            else
            {
                fading = false;
                fadeTimer = 0;
                if (faded)
                {
                    GameManager.instance.fadeToBlack = false;
                    GameManager.instance.changeToDayTwo = false;
                    faded = false;
                }
                else
                {
                    GameManager.instance.changeToDayTwo = true;
                    faded = true;
                }
                
            }
        }
	
	}

    public void startFade()
    {
        Debug.Log("fading");
        fading = true;
    }
}
