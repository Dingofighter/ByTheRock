using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    Animator anim;

    bool isWalking = false;
    bool isCrouching = false;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        if (Input.GetButtonDown("Fire2"))
        {
            isWalking = !isWalking;
        }

        if (Input.GetButtonDown("Crouch"))
        {
            isCrouching = !isCrouching;
        }

        if (isWalking)
        {
            anim.SetFloat("Forward", vertical * 0.5f);
        }
        else
        {
            anim.SetFloat("Forward", vertical);
        }

        anim.SetFloat("Turn", horizontal * 0.5f);
        anim.SetBool("Crouch", isCrouching);
    }
}
