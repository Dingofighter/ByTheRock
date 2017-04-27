using UnityEngine;
using System.Collections;

public class butterflyMovement : MonoBehaviour {

    bool moving;
    int stillTimer;
    int currAngle;

	// Use this for initialization
	void Start () {

        moving = true;
        currAngle = Random.Range(0, 360);
        transform.Rotate(new Vector3(0, 1, 0) * currAngle);
        
    }
	
	// Update is called once per frame
	void Update () {

        if (moving)
        {
            transform.position += transform.up * 0.1f;

        }
        else
        {
            stillTimer++;
            if (stillTimer >= Random.Range(30, 90))
            {
                moving = true;
                stillTimer = 0;
                currAngle = Random.Range(0, 360);
                transform.Rotate(new Vector3(0, 1, 0) * currAngle);
            }
        }

	}

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "tree")
        {
            Physics.IgnoreCollision(c.collider, GetComponent<Collider>());
        }
        moving = false;
    }
}
