using UnityEngine;
using System.Collections;

public class movieHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void playMovie()
    {
        Debug.Log("playing intro");
        ((MovieTexture)GetComponent<Renderer>().material.mainTexture).Play();
    }
}
