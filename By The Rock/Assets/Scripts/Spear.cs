using UnityEngine;
using System.Collections;

public class Spear : MonoBehaviour {

    public bool isThrown = false;
    private bool hitSomething = false;
    private bool hitSomethingForward = false;

    float distToGround;

    private Rigidbody rigidbodyY;

	// Use this for initialization
	void Start () {

        rigidbodyY = GetComponent<Rigidbody>();

        distToGround = GetComponent<Collider>().bounds.extents.y;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        Debug.DrawRay(transform.position, transform.forward * (distToGround + 0.1f), Color.magenta);
        if (Physics.Raycast(transform.position, transform.forward, distToGround + 0.1f) && isThrown && !hitSomething && !hitSomethingForward)
        {
            hitSomething = true;
        }

        Debug.DrawRay(transform.position, transform.up * 1.5f, Color.green);
        if (Physics.Raycast(transform.position, transform.up, 1.5f) && isThrown && !hitSomethingForward && !hitSomething)
        {
            hitSomethingForward = true;
        }

        if (rigidbodyY.velocity != Vector3.zero && !hitSomething && !hitSomethingForward)
        {
            transform.LookAt(transform.position + rigidbodyY.velocity);
            transform.Rotate(90, 0, 0);
            //transform.rotation = Quaternion.Euler(new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z));
        }

    }

    void OnCollisionEnter(Collision c)
    {
       if (c.gameObject.tag == "tree")
        {
            Physics.IgnoreCollision(c.collider, GetComponent<Collider>());
        }
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "enemy" && isThrown)
        {
            c.GetComponentInParent<Movement>().takeDamage(1);
        }
        if (c.gameObject.tag == "FriendOrc" && isThrown)
        {
            c.GetComponentInParent<orcMovement>().hitByPlayer();
        }
    }
}
