using UnityEngine;
using System.Collections;

public class ButterflyPart : MonoBehaviour
{

    public bool rightSide;
    public int flapSpeed;

    bool flyDirection;
    int flapTimer;



    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //if (GameManager.instance.paused) return;
        if (!transform.parent.GetComponent<butterflyMovement>().getMoving()) return;
        flapTimer++;
        if (flyDirection)
        {
            if (rightSide) transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + flapSpeed);
            else transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - flapSpeed);
            if (flapTimer >= 10)
            {
                flyDirection = !flyDirection;
                flapTimer = 0;
            }
        }
        else
        {
            if (rightSide) transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z - flapSpeed);
            else transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + flapSpeed);
            if (flapTimer >= 10)
            {
                flyDirection = !flyDirection;
                flapTimer = 0;
            }
        }
       
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "tree" || c.gameObject.tag == "Player")
        {
            Debug.Log("Collision detected!!!!");
            Physics.IgnoreCollision(c.collider, GetComponent<Collider>());
        }
        else
        {
            transform.parent.GetComponent<butterflyMovement>().collide(c);
        }
        
    }
}