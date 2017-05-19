using UnityEngine;
using System.Collections;

public class birdMovement : MonoBehaviour {

    int timer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (GameManager.instance.paused) return;

        timer++;
        transform.position -= transform.forward * 0.07f;
        if (timer >= 6000)
        {
            transform.position += transform.forward * 0.07f * 6000;
            timer = 0;
        }
	}
}
