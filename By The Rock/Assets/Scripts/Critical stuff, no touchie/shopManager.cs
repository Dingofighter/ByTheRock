using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class shopManager : MonoBehaviour {

    public GameObject panel;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        panel.SetActive(GameManager1.instance.inShop);

	}
}
