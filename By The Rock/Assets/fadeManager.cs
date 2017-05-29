using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class fadeManager : MonoBehaviour {

    public Image blackImage;
    public Text creditsText;
    public Text tbcText;
    public Text finalFadeC;
    public Image finalFadeLogo;

    float fadeTimer;
    bool fading;
    bool faded;
    bool endFading;
    bool endCreditsStart;
    float fadeTimerTwo;
    float fadeTimerThree;
    float slaskTimer;

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
                GameManager.instance.fadingAtm = false;
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
            fadeTimer += Time.deltaTime * 60;
            if (fadeTimer < 150)
            {
                blackImage.color = new Color(0, 0, 0, fadeTimer / 120);
            }
            else
            {
                if (!endCreditsStart)
                {
                    creditsText.gameObject.SetActive(true);
                    tbcText.gameObject.SetActive(true);
                    GameManager.instance.fadingAtm = false;
                    //endFading = false;
                    endCreditsStart = true;
                    GameManager.instance.fadeToBlack = false;
                    GameManager.instance.gameOver = true;
                }

                fadeTimerTwo += Time.deltaTime * 60;

                if (fadeTimer < 200)
                {
                    tbcText.color = new Color(1, 1, 1, fadeTimerTwo / 60);
                }
                else if (fadeTimer < 450) { fadeTimerTwo = 0; }
                else if (fadeTimer < 510)
                {
                    tbcText.color = new Color(1, 1, 1, 1 - (fadeTimerTwo / 60));

                }
                else
                {
                    Debug.Log(creditsText.transform.position.y);
                    if (creditsText.transform.position.y < 2430)
                    {
                        creditsText.transform.position = new Vector3(creditsText.transform.position.x, creditsText.transform.position.y + 2.5f, creditsText.transform.position.z);
                    }
                    else
                    {
                        fadeTimerThree += Time.deltaTime * 60;
                        if (fadeTimerThree > 120 && fadeTimerThree < 240)
                        {
                            finalFadeC.color = new Color(1, 1, 1, 1 - (fadeTimerThree - 120 ) / 120);
                            finalFadeLogo.color = new Color(1, 1, 1, 1 - (fadeTimerThree - 120) / 120);
                        }
                        else if (fadeTimerThree > 240)
                        {
#if UNITY_EDITOR
                            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
                        }

                    }
                }
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
