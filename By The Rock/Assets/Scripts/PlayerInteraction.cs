using UnityEngine;
using System.Collections;

public class PlayerInteraction : MonoBehaviour {
    

	// Use this for initialization
	void Start () {
        
	
	}
	
	// Update is called once per frame
	void Update () {
        

    }

    void OnTriggerStay(Collider c)
    {
        if (GameManager.instance.paused) return;

        if (Input.GetButtonDown("Interact"))
        {
            if (c.gameObject.tag == "Interact1")
            {
                Destroy(c.gameObject);
            }
            if (c.gameObject.tag == "Interact2")
            {
                c.gameObject.transform.position += new Vector3(0, 1, 0);
            }
            if (c.gameObject.tag == "Dialogue" && !GameManager.instance.shoulderView)
            {
                FindObjectOfType<DialogueHandler>().StartDialogue(c.GetComponentsInParent<Dialogue>());
            }
        }
    }
}
