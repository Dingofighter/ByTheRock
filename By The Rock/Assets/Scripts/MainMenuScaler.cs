using UnityEngine;
using System.Collections;

public class MainMenuScaler : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
        print(Screen.width * 1.65f);
        print(GetComponent<RectTransform>().rect.width);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
