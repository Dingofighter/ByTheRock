using UnityEngine;
using System.Collections;

public class butterflyMovement : MonoBehaviour {

    bool moving;
    int stillTimer;
    public int maxStillTime = 200;
    public int minStillTime = 30;
    public bool waitForPlayer = false;
    int stillTime;
    int currAngle;
    public float speed;

    Transform player;

	// Use this for initialization
	void Start () {

        moving = true;
        currAngle = Random.Range(0, 360);
        transform.Rotate(new Vector3(0, 0, 1) * currAngle);
        transform.eulerAngles = new Vector3(50, transform.eulerAngles.y, transform.eulerAngles.z);
        stillTime = Random.Range(minStillTime, maxStillTime);

        player = FindObjectOfType<PlayerController>().transform;

    }
	
	// Update is called once per frame
	void Update () {

        if (GameManager.instance.paused) return;

        if (moving)
        {
            transform.position += transform.up * speed;
            transform.Rotate(new Vector3(1, 0, 0) * 0.5f);
        }
        else
        {
            if (!waitForPlayer) stillTimer++;
            
            if (Vector3.Distance(player.position, transform.position) < 4)
            {
                stillTimer = stillTime;
            }
            
            if (stillTimer >= stillTime)
            {
                moving = true;
                stillTimer = 0;
                transform.eulerAngles = new Vector3(90, transform.eulerAngles.y, transform.eulerAngles.z);
                currAngle = Random.Range(0, 360);
                transform.Rotate(new Vector3(0, 0, 1) * currAngle);
                // transform.rotation = new Quaternion(/*transform.rotation.x*/1, transform.rotation.y, transform.rotation.z, transform.rotation.w);
                transform.eulerAngles = new Vector3(50, transform.eulerAngles.y, transform.eulerAngles.z);
                stillTime = Random.Range(minStillTime, maxStillTime);
            }
        }

	}

    public bool getMoving()
    {
        return moving;
    }

    public void collide(Collision c)
    {
        if (c.gameObject.tag == "tree" || c.gameObject.tag == "Player")
        {
            Physics.IgnoreCollision(c.collider, GetComponent<Collider>());
        }
        else
        {
            moving = false;
        }
    }

    void OnCollisionEnter(Collision c)
    {
       
        if (c.gameObject.tag == "tree" || c.gameObject.tag == "Player")
        {
            Physics.IgnoreCollision(c.collider, GetComponent<Collider>());
        }
        else
        {
            moving = false;
        }
    }
}
