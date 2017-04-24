using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(FPSCounter1))]
public class FPSDisplay1 : MonoBehaviour
{
    public Text fpsLabel;

    FPSCounter1 fpsCounter;

    void Awake()
    {
        fpsCounter = GetComponent<FPSCounter1>();
    }

    void Update()
    {
        fpsLabel.text = "Wave " + GameManager1.instance.currentWave + ". " + GameManager1.instance.currentCash + " Dollarydoos.";
    }
}