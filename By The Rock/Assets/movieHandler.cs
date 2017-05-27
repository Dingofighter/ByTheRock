using UnityEngine;
using System.Collections;

public class movieHandler : BaseEmitter {

	// Use this for initialization
	protected override void Start () {
    }
	
	// Update is called once per frame
	void Update () {
        
	
	}

    public void playMovie()
    {
        
        Debug.Log("playing intro");
        ((MovieTexture)GetComponent<Renderer>().material.mainTexture).Play();
        Play();
    }
}
