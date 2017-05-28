using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class fadeManager : MonoBehaviour {

    public Image blackImage;

    float fadeTimer;
    bool fading;
    bool faded;
    bool endFading;

	// Use this for initialization
	void Start () {

    

	}
	
	// Update is called once per frame
	void Update () {

        if (fading)
        {
            blackImage.gameObject.SetActive(true);
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
                    blackImage.gameObject.SetActive(false);
                }
                else
                {
                    GameManager.instance.changeToDayTwo = true;
                    faded = true;
                }
                
            }
        }

        if (endFading)
        {
            blackImage.gameObject.SetActive(true);
            if (fadeTimer < 150)
            {
                fadeTimer += Time.deltaTime * 60;
                blackImage.color = new Color(0, 0, 0, fadeTimer / 120);
            }
            else
            {
                endFading = false;
                fadeTimer = 0;
                GameManager.instance.fadeToBlack = false;
                GameManager.instance.gameOver = true;
            }
        }
	
	}

    public void startFade()
    {
        Debug.Log("fading");
        fading = true;
    }

    public void startEndFade()
    {
        Debug.Log("end fading");
        fadeTimer = 0;
        endFading = true;
    }
}
