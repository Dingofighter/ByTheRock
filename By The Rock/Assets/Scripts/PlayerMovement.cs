using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {

    Animator animator;
    DialogueHandler dialogueHandler;
    Transform cam;
    public Transform spearPre;
    Transform spear;


    bool isWalking = false;
    bool isCrouching = false;
    bool canMove = true;

    bool holdingSpear;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        dialogueHandler = FindObjectOfType<DialogueHandler>();
        cam = Camera.main.transform;
    }

    public bool getCrouching()
    {
        return isCrouching;
    }
	
	// Update is called once per frame
	void Update () {

        if (GameManager.instance.paused) return;

        if (Input.GetMouseButtonDown(0))
        {
            spear = (Transform)Instantiate(spearPre, new Vector3(transform.position.x, transform.position.y + 1.01f, transform.position.z) + (transform.right * 0.4f), Quaternion.Euler(new Vector3(79.95f, transform.eulerAngles.y, 0)));
            spear.GetComponent<Rigidbody>().detectCollisions = false;
            spear.GetComponent<Rigidbody>().useGravity = false;
            holdingSpear = true;
            spear.GetComponentInChildren<Rigidbody>().detectCollisions = false;
            spear.GetComponentInChildren<Rigidbody>().useGravity = false;
        }

        if (holdingSpear)
        {
            spear.transform.position = new Vector3(transform.position.x, transform.position.y + 1.01f, transform.position.z) + (transform.right * 0.4f);
            spear.transform.rotation = Quaternion.Euler(new Vector3(79.95f, transform.eulerAngles.y, 0));
        }

        if (!Input.GetMouseButton(0) && holdingSpear)
        {
            holdingSpear = false;
            spear.GetComponent<Rigidbody>().useGravity = true;
            spear.GetComponent<Rigidbody>().detectCollisions = true;
            spear.GetComponentInChildren<Rigidbody>().detectCollisions = true;
            spear.GetComponentInChildren<Rigidbody>().useGravity = true;
            spear.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 85 + transform.up * 10, ForceMode.Impulse);
            spear.GetComponent<Spear>().isThrown = true;
        }






        
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


    void onKeyDown()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        //dialogueHandler.StartDialogue(other.GetComponentInParent<Dialogue>());
    }
}