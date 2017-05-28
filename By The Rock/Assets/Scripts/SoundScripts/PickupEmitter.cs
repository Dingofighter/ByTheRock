using UnityEngine;
using System.Collections;

public class PickupEmitter : BaseEmitter {

    int played = 0;
	// Use this for initialization
	protected override void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PickUp()
    {

        Play();
        _EventInstance.release();
        played++;
        Debug.Log("Sound played: " + played);
    }
}
