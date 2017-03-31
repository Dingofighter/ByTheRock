using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    Vector3 target1;
    Vector3 target2;
    Vector3 target3;
    Vector3 target4;
    int currTarget;
    int counter;
    bool fast;
    int fastCounter;
    bool invincibility;

    public int health;

    // Use this for initialization
    void Start () {

        target1 = new Vector3(transform.position.x + 5, transform.position.y, transform.position.z);
        target2 = new Vector3(transform.position.x, transform.position.y, transform.position.z + 5);
        target3 = new Vector3(transform.position.x - 5, transform.position.y, transform.position.z);
        target4 = new Vector3(transform.position.x, transform.position.y, transform.position.z - 5);

    }

    public void takeDamage(int i)
    {
        if (!invincibility)
        {
            health -= i;
            fast = true;
            invincibility = true;
        }
    }
        
	// Update is called once per frame
	void Update () {

        if (health <= 0) Destroy(gameObject); 

        counter++;
        if (counter > 130)
        {
            currTarget++;
            counter = 0;
        }
        if (currTarget == 4) currTarget = 0;

        float speed = 0.07f;
        if (fast)
        {
            speed = 0.2f;
            fastCounter++;
            if (fastCounter > 30) invincibility = false;
            if (fastCounter == 100)
            {
                fast = false;
                fastCounter = 0;
            }
        }

        if (currTarget == 0) transform.position = Vector3.MoveTowards(transform.position, target1, speed);
        if (currTarget == 1) transform.position = Vector3.MoveTowards(transform.position, target2, speed);
        if (currTarget == 2) transform.position = Vector3.MoveTowards(transform.position, target3, speed);
        if (currTarget == 3) transform.position = Vector3.MoveTowards(transform.position, target4, speed);
	
	}
}
