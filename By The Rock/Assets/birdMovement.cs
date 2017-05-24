using UnityEngine;
using System.Collections;

public class birdMovement : MonoBehaviour {

    int timer;

    Vector3 startPosition;

	// Use this for initialization
	void Start () {

        startPosition = transform.position;

	}
	
	// Update is called once per frame
	void Update () {

        if (GameManager.instance.paused) return;


        /*
        timer++;
        transform.position -= transform.forward * 0.07f;
        if (timer >= 6000)
        {
            transform.position += transform.forward * 0.07f * 6000;
            timer = 0;
        }
        */

        transform.position -= transform.forward * 0.1f;
        transform.Rotate(new Vector3(0, 2, 0));

	}
}
