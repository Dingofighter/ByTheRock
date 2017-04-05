using UnityEngine;
using System.Collections;

public class Melee : MonoBehaviour {
    

	// Use this for initialization
	void Start () {
	
        

	}
	
	// Update is called once per frame
	void Update () {

        


    }

    void OnTriggerStay(Collider c)
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (c.gameObject.tag == "enemy")
            {
                c.GetComponent<Movement>().takeDamage(1);
            }
        }

    }
}
