using UnityEngine;
using System.Collections;

public class MenuHandler : MonoBehaviour {

    public GameObject MainPanel;
    public GameObject SettingsPanel;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void OpenSettings()
    {
        MainPanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        MainPanel.SetActive(true);
        SettingsPanel.SetActive(false);
    }
}
