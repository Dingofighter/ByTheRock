using UnityEngine;
using System.Collections;

public class PlayerInteraction : MonoBehaviour {

    Transform inter;

    DialogueHandler dialogueHandler;

	// Use this for initialization
	void Start () {
        dialogueHandler = FindObjectOfType<DialogueHandler>();
    }
	
	// Update is called once per frame
	void Update () {
        

    }

    void OnTriggerStay(Collider c)
    {
        if (GameManager.instance.paused) return;

        if (c.gameObject.tag == "Dialogue" && c.transform.parent.GetComponent<Dialogue>().autoTriggered && !GameManager.instance.shoulderView)
        {
            dialogueHandler.StartDialogue(c.GetComponentsInParent<Dialogue>());
            if (dialogueHandler.firstFrame)
            {
                //c.GetComponentInParent<Dialogue>().transform.LookAt(transform);
                //transform.LookAt(c.GetComponentInParent<Dialogue>().transform);
                //transform.rotation = new Quaternion(0, transform.rotation.y, 0, 0);
                //transform.LookAt(new Vector3(transform.right.x, c.transform.position.y, transform.forward.z));
                inter = c.gameObject.transform;

                if (!c.transform.parent.GetComponent<Dialogue>().walkAndTalk)
                {
                    if (c.transform.parent.GetComponent<Dialogue>().rotationTarget != null)
                    {
                        transform.rotation = Quaternion.Euler(0, c.transform.parent.GetComponent<Dialogue>().rotationTarget.eulerAngles.y + 180, 0);
                    }
                    else
                    {
                        transform.rotation = Quaternion.Euler(0, c.transform.eulerAngles.y + 180, 0);
                    }
                }
            }
        }

        if (Input.GetButtonDown("Interact") && !GameManager.instance.crouching)
        {
            if (c.gameObject.tag == "Interact1")
            {
                if (GameManager.instance.itemID1 == -1) GameManager.instance.changeItem(0, 0);
                else GameManager.instance.changeItem(0, -1);
            }
            if (c.gameObject.tag == "Interact2")
            {
                c.gameObject.transform.position += new Vector3(0, 1, 0);
            }
            if (c.gameObject.tag == "Dialogue" && !GameManager.instance.shoulderView)
            {
                //c.GetComponentInParent<Dialogue>().transform.LookAt(transform);
                //transform.LookAt(c.GetComponentInParent<Dialogue>().transform);
                //transform.rotation = new Quaternion(0, transform.rotation.y, 0, 0);
                //transform.LookAt(new Vector3(transform.right.x, c.transform.position.y, transform.forward.z));
                inter = c.gameObject.transform;

                if (!c.transform.parent.GetComponent<Dialogue>().walkAndTalk)
                {
                    transform.rotation = Quaternion.Euler(0, c.transform.eulerAngles.y + 180, 0);
                }
                FindObjectOfType<DialogueHandler>().StartDialogue(c.GetComponentsInParent<Dialogue>());
            }
            if (c.gameObject.tag == "spear")
            {
                //Destroy(c.GetComponentInParent<Spear>().gameObject);
                PlayerMovement.gotSpear = true;
            }
        }
    }

    public Transform getCollisionTransform()
    {
        return inter;
    }
}
