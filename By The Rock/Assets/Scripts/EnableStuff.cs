using UnityEngine;
using System.Collections;

public class EnableStuff : MonoBehaviour {

    public GameObject stuffToEnable;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player")
        {
            stuffToEnable.SetActive(true);
        }
    }
}
