﻿using UnityEngine;
using System.Collections;

public class RustleManager : BaseEmitter {

	// Use this for initialization
	protected override void Start () {

        base.Start();
	
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Prassel")
        {
            Play();
            _EventInstance.release();
            Debug.Log("pamsdfasdf");
        } 
       
    }

    // Update is called once per frame
    void Update () {
	
	}
}
