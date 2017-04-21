using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class shopDisplay : MonoBehaviour {

    public Text shopLabel;

    // Use this for initialization
    void Start ()
    {
	
	}

    // Update is called once per frame
    void Update()
    {
        string buy1 = "1: buy shot speed upgrade ($100)";
        string buy2 = "2: buy damage upgrade ($500)";
        string buy3 = "3: buy super speed ($3000)";
        string buy4 = "4: buy slave.. I mean ally ($2000)";
        if (GameManager1.instance.bought1) buy1 = ":(";
        if (GameManager1.instance.bought2) buy2 = ":(";
        if (GameManager1.instance.bought3) buy3 = ":(";
        if (GameManager1.instance.bought4) buy4 = ":(";

        Debug.Log(GameManager1.instance.inShop);
        if (GameManager1.instance.inShop) transform.parent.gameObject.SetActive(true);

        //transform.parent.gameObject.SetActive(GameManager1.instance.inShop);
        
        

        shopLabel.text = "E: leave shop\n" + buy1 + "\n" + buy2 + "\n" + buy3 + "\n" + buy4;
    }
}
