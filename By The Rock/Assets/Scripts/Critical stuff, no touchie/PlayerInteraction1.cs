using UnityEngine;
using System.Collections;

public class PlayerInteraction1 : MonoBehaviour {
    

	// Use this for initialization
	void Start () {
        
	
	}
	
	// Update is called once per frame
	void Update () {
        

    }

    void OnTriggerStay(Collider c)
    {
        if (GameManager1.instance.paused) return;
        
        if (Input.GetButtonDown("Interact") && !GameManager1.instance.crouching)
        {
            if (c.gameObject.tag == "Interact1")
            {
                Destroy(c.gameObject);
            }
            if (c.gameObject.tag == "Interact2")
            {
                c.gameObject.transform.position += new Vector3(0, 1, 0);
            }
            if (c.gameObject.tag == "Dialogue" && !GameManager1.instance.shoulderView)
            {
                //c.GetComponentInParent<Dialogue>().transform.LookAt(transform);
                //transform.LookAt(c.GetComponentInParent<Dialogue>().transform);
                //transform.rotation = new Quaternion(0, transform.rotation.y, 0, 0);
                //transform.LookAt(new Vector3(transform.right.x, c.transform.position.y, transform.forward.z));
                transform.rotation = Quaternion.Euler(0, c.transform.eulerAngles.y + 180, 0);
                FindObjectOfType<DialogueHandler>().StartDialogue(c.GetComponentInParent<Dialogue>());
            }
            if (c.gameObject.tag == "spear")
            {
                Destroy(c.GetComponentInParent<Spear1>().gameObject);
                PlayerMovement1.gotSpear = true;
            }
            if (c.gameObject.tag == "shop")
            {
                GameManager1.instance.toggleShop();
               
            }
        }
    }
}
