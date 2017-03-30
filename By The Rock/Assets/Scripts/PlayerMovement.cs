using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    Animator animator;
    DialogueHandler dialogueHandler;

    bool isWalking = false;
    bool isCrouching = false;
    bool canMove = true;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        dialogueHandler = FindObjectOfType<DialogueHandler>();
    }
	
	// Update is called once per frame
	void Update () {
        canMove = !dialogueHandler.inDialogue;

        if (canMove)
        {
            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");

            if (Input.GetButtonDown("Walk"))
            {
                isWalking = !isWalking;
            }

            if (Input.GetButtonDown("Crouch"))
            {
                isCrouching = !isCrouching;
            }

            if (isWalking)
            {
                animator.SetFloat("Forward", vertical * 0.5f);
            }
            else
            {
                animator.SetFloat("Forward", vertical);
            }

            animator.SetFloat("Turn", horizontal * 0.5f);
            animator.SetBool("Crouch", isCrouching);
        }
        else
        {
            animator.SetFloat("Forward", 0);
            animator.SetFloat("Turn", 0);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        dialogueHandler.StartDialogue(other.GetComponentInParent<Dialogue>());
    }
}