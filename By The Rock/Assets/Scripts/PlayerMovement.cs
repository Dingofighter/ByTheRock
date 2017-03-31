using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    Animator animator;
    DialogueHandler dialogueHandler;
    Transform cam;

    bool isWalking = false;
    bool isCrouching = false;
    bool canMove = true;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        dialogueHandler = FindObjectOfType<DialogueHandler>();
        cam = Camera.main.transform;
    }
	
	// Update is called once per frame
	void Update () {
        canMove = !dialogueHandler.inDialogue;

        if (canMove)
        {
            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");

            // Calculate movement based on camera direction
            Vector3 camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 move = vertical * camForward + horizontal * cam.right;

            // Normalize movement to avoid faster diagonal movement
            if (move.magnitude > 1f)
            {
                move.Normalize();
            }

            // Translates movement vector to local space
            move = transform.InverseTransformDirection(move);
            // Calculate turn amount based on movement input
            float turnAmount = Mathf.Atan2(move.x, move.z);
            float forwardAmount = move.z;

            if (Input.GetButtonDown("Walk"))
            {
                isWalking = !isWalking;
            }

            if (Input.GetButtonDown("Crouch"))
            {
                isCrouching = !isCrouching;
            }

            // Lower forward speed when walking
            if (isWalking)
            {
                animator.SetFloat("Forward", forwardAmount * 0.5f);
            }
            else
            {
                animator.SetFloat("Forward", forwardAmount);
            }

            animator.SetFloat("Turn", turnAmount);
            animator.SetBool("Crouch", isCrouching);
        }
        else
        {
            animator.SetFloat("Forward", 0);
            animator.SetFloat("Turn", 0);
        }
    }
    /*
    void OnTriggerEnter(Collider other)
    {
<<<<<<< HEAD
        FindObjectOfType<DialogueHandler>().StartDialogue(other.GetComponentInParent<Dialogue>());
    }*/
=======
        dialogueHandler.StartDialogue(other.GetComponentInParent<Dialogue>());
    }
>>>>>>> refs/remotes/origin/master
}