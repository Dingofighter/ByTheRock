using UnityEngine;
using System.Collections;

public class FireEmitter : BaseEmitter {

	// Use this for initialization
	protected override void Start () {

        base.Start();
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void StartFadeOut()
    {
        Stop();
    }
}
