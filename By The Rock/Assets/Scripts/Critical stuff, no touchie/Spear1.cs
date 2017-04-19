using UnityEngine;
using System.Collections;

public class Spear1 : MonoBehaviour {

    public bool isThrown = false;
    private bool hitSomething = false;
    private bool hitSomethingForward = false;
    bool stuck;
    bool canDamage = true;
    int deathCounter;

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

        if (hitSomething || hitSomethingForward)
        {
            if (!stuck)
            {
                gameObject.layer = 2;
                stuck = true;
                transform.position += transform.up * 0.4f;
            }
            rigidbodyY.useGravity = false;
            rigidbodyY.constraints = RigidbodyConstraints.FreezeAll;
            //rigidbodyY.detectCollisions = false;
            rigidbodyY.velocity = Vector3.zero;
        }

        if (stuck)
        {
            deathCounter++;
            if (deathCounter > 100)
            {
                Destroy(gameObject);
            }
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
        if (c.gameObject.tag == "enemy" && isThrown && canDamage)
        {
            canDamage = false;
            c.GetComponentInParent<Movement1>().takeDamage(1);
            transform.SetParent(c.transform);
        }
        if (c.gameObject.tag == "FriendOrc" && isThrown)
        {
            c.GetComponentInParent<orcMovement1>().hitByPlayer();
        }
    }
}
