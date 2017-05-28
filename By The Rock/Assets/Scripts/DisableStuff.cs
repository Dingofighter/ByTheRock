using UnityEngine;
using System.Collections;

public class DisableStuff : MonoBehaviour {

    public GameObject stuffToDisable;

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
            stuffToDisable.SetActive(false);
        }
    }
}
