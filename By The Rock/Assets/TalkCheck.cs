using UnityEngine;
using System.Collections;

public class TalkCheck : MonoBehaviour {

    Transform player;
    Animator anim;

    int talkTimer;
    bool talking;
    bool talkHand;
    bool takingItem;

    bool changedTexture;

    int maxHandTimer;

    Renderer rend;

    public Material bandageMaterial;

	// Use this for initialization
	void Start () {
	
        player = FindObjectOfType<PlayerController>().transform;
        anim = GetComponent<Animator>();
        maxHandTimer = Random.Range(100, 250);
        rend = GetComponent<Renderer>();

        Renderer[] children;
        children = GetComponentsInChildren<Renderer>();
        

    } 
	
	// Update is called once per frame
	void Update () {

        /*
        if (boolGivenMoss && !changedTexture)
        {
            foreach (Renderer r in children)
            {
                r.material = bandageMaterial;
            }
            changedTexture = true
        }*/


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
