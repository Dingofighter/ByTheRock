using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {
    
    int counter;
    int counterIdleMax;
    bool invincibility;

    bool agro;
    bool walking;
    
    Vector3 targetPosition;
    Vector3 lastPosition;
    Vector3 spawnPosition;
    float currAngle;
    float currDist;

    Renderer rend;
    public Color c = Color.green;
    public Color c2 = Color.red;
    public float outOfBoundsDist;
    public float maxDist;
    public float minDist;
    public float walkSpeed;

    public int health;

    // Use this for initialization
    void Start () {
        
        counterIdleMax = Mathf.RoundToInt(Random.Range(100, 250));
        spawnPosition = transform.position;

        rend = GetComponent<Renderer>();
        rend.material.color = c;

    }

    public void takeDamage(int i)
    {
        if (!invincibility)
        {
            health -= i;
            invincibility = true;
        }
    }
        
	// Update is called once per frame
	void Update () {

        if (health <= 0) Destroy(gameObject); 

        counter++;
        if ((transform.position == targetPosition && walking) || (!walking && counter > counterIdleMax))
        {
            walking = !walking;
            counterIdleMax = Mathf.RoundToInt(Random.Range(100, 250));
            counter = 0;
            if (walking)
            {
                if (Vector3.Distance(spawnPosition, transform.position) > outOfBoundsDist)
                {
                    rend.material.color = c2;
                    targetPosition = lastPosition;
                }
                else
                {
                    lastPosition = targetPosition;
                    currAngle = Random.Range(0, 2 * Mathf.PI);
                    currDist = Random.Range(3, 5);

                    rend.material.color = c;
                    
                    targetPosition = new Vector3(transform.position.x + Mathf.Sin(currAngle) * currDist, transform.position.y, transform.position.z + Mathf.Cos(currAngle) * currDist);
                }
            }
            else
            {
                if (Vector3.Distance(spawnPosition, transform.position) > outOfBoundsDist)
                {
                    rend.material.color = c2;
                }
                else rend.material.color = c;
            }
            
        }

        if (walking)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, walkSpeed);
        }


    }
}
