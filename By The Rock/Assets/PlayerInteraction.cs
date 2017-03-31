using UnityEngine;
using System.Collections;

public class PlayerInteraction : MonoBehaviour {

    private bool pressingInteract;

	// Use this for initialization
	void Start () {
        
	
	}
	
	// Update is called once per frame
	void Update () {
        

    }

    void OnTriggerStay(Collider c)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (c.gameObject.tag == "Interact1")
            {
                Destroy(c.gameObject);
            }
            if (c.gameObject.tag == "Interact2")
            {
                c.gameObject.transform.position += new Vector3(0, 1, 0);
            }
        }
    }
}
