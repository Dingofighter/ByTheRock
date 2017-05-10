using UnityEngine;
using System.Collections;

public class PlayerInteraction : MonoBehaviour {

    Transform inter;

    DialogueHandler dialogueHandler;

    readonly int INGET = -1;
    readonly int SVAMP1 = 0;
    readonly int SVAMP2 = 1;
    readonly int SVAMP3 = 2;
    readonly int SVAMP4 = 3;
    readonly int SVAMP5 = 4;
    readonly int MOSSA = 5;
    readonly int VATTEN = 6;
    readonly int BARK = 7;
    readonly int ORT = 8;


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
            
            if (c.gameObject.tag == "Mossa")
            {
                GameManager.instance.changeItem(0, MOSSA, false);
                //if (GameManager.instance.itemID1 == -1) GameManager.instance.changeItem(0, 0);
                //else GameManager.instance.changeItem(0, -1);
                Destroy(c.transform.gameObject);
            }
            if (c.gameObject.tag == "Vatten")
            {
                GameManager.instance.changeItem(1, VATTEN, false);
                Destroy(c.transform.gameObject);
            }
            if (c.gameObject.tag == "Bark")
            {
                GameManager.instance.changeItem(2, BARK, false);
                Destroy(c.transform.gameObject);
            }
            if (c.gameObject.tag == "Ort")
            {
                GameManager.instance.changeItem(3, ORT, false);
                Destroy(c.transform.gameObject);
            }

            if (c.gameObject.tag == "Svamp")
            {
                if (GameManager.instance.itemID2 >= INGET && GameManager.instance.itemID2 <= SVAMP4)
                {
                    GameManager.instance.changeItem(1, GameManager.instance.itemID2 + 1, false);
                }
                Destroy(c.transform.gameObject);
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
