﻿using UnityEngine;
using System.Collections;

public class TalkCheck : MonoBehaviour {

    Transform player;
    Animator anim;

    int talkTimer;
    bool talking;
    bool talkHand;
    bool takingItem;

    int maxHandTimer;

	// Use this for initialization
	void Start () {
	
        player = FindObjectOfType<PlayerController>().transform;
        anim = GetComponent<Animator>();
        maxHandTimer = Random.Range(100, 250);

    }
	
	// Update is called once per frame
	void Update () {

        if (GameManager.instance.talking && Vector3.Distance(transform.position, player.position) < 10)
        {
            talking = true;
        }
        else
        {
            talking = false;
        }

        if (talking)
        {
            talkTimer++;
            if (talkTimer > maxHandTimer)
            {
                talkHand = !talkHand;
                talkTimer = 0;
                maxHandTimer = Random.Range(100, 250);
            }
        }

        takingItem = GameManager.instance.givingItem;

        anim.SetBool("acceptingItem", takingItem);
        //anim.SetBool("talking", talking);
        anim.SetBool("talkWithHand", talkHand);
        
    }
}
