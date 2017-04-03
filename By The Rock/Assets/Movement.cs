using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{

    int counter;
    int counterIdleMax;
    bool invincibility;
    int invincibilityTimer;

    bool run;
    Vector3 playerPosition;
    bool walking;

    Vector3 targetPosition;
    Vector3 lastPosition;
    Vector3 spawnPosition;
    float currAngle;
    float currDist;

    Renderer rend;
    public Color color;
    public Color colorEdge;
    public Color colorHit;
    public Color colorRun;
    public float outOfBoundsDist;
    public float maxDist;
    public float minDist;
    public float walkSpeed;
    float currWalkSpeed;

    public int health;

    // Use this for initialization
    void Start()
    {

        counterIdleMax = Mathf.RoundToInt(Random.Range(100, 250));
        spawnPosition = transform.position;

        rend = GetComponent<Renderer>();
        rend.material.color = color;

        currWalkSpeed = walkSpeed;

    }

    public void takeDamage(int i)
    {
        if (!invincibility)
        {
            if (!walking) counter = counterIdleMax - 1;
            health -= i;
            rend.material.color = colorHit;
            invincibility = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (health <= 0) Destroy(gameObject);
        
        if (!run)
        {
            counter++;
            if ((transform.position == targetPosition && walking) || (!walking && counter > counterIdleMax))
            {
                walking = !walking;
                if (invincibility)
                {
                    counterIdleMax = 1;
                }
                else
                {
                    counterIdleMax = Mathf.RoundToInt(Random.Range(100, 250));
                }
                counter = 0;
                if (walking)
                {
                    if (Vector3.Distance(spawnPosition, transform.position) > outOfBoundsDist)
                    {
                        rend.material.color = colorEdge;
                        transform.Rotate(new Vector3(0, 1, 0) * 180);
                        targetPosition = lastPosition;
                    }
                    else
                    {
                        lastPosition = targetPosition;
                        currAngle = Random.Range(0, 360);
                        currDist = Random.Range(minDist, maxDist);

                        rend.material.color = color;

                        transform.Rotate(new Vector3(0, 1, 0) * currAngle);

                        targetPosition = new Vector3(transform.position.x + transform.forward.x * currDist, transform.position.y, transform.position.z + transform.forward.z * currDist);
                        //targetPosition = new Vector3(transform.position.x + Mathf.Sin(currAngle) * currDist, transform.position.y, transform.position.z + Mathf.Cos(currAngle) * currDist);
                    }
                }
                else
                {
                    if (Vector3.Distance(spawnPosition, transform.position) > outOfBoundsDist)
                    {
                        rend.material.color = colorEdge;
                    }
                    else rend.material.color = color;
                }

            }

            if (walking)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, currWalkSpeed);
            }

        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 0.2f);
            if (transform.position == targetPosition)
            {
                run = false;
                spawnPosition = transform.position;
            }
        }

        if (invincibility)
        {
            currWalkSpeed = 0.1f;
            rend.material.color = colorHit;
            invincibilityTimer++;
            if (invincibilityTimer > 50)
            {
                currWalkSpeed = walkSpeed;
                rend.material.color = color;
                invincibility = false;
                invincibilityTimer = 0;
            }
        }

        //transform.Rotate(new Vector3(0, 1, 0) * Time.deltaTime * 3000);
        //transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.forward.x * 100, transform.position.y, transform.forward.z * 100), 0.01f);

    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "closeToPlayer" && !run && !c.GetComponentInParent<PlayerMovement>().getCrouching())
        {
            rend.material.color = colorRun;
            run = true;
            playerPosition = c.gameObject.transform.position;

            //Vector3 v = transform.position - playerPosition;
            //v.Normalize();
            //float angle = Mathf.Atan2(v.z, v.x) * Mathf.Rad2Deg; 

            //Vector3.RotateTowards(transform.position, playerPosition, 2, 2);
            //currAngle = Vector3.Angle(playerPosition, transform.position);

            //Debug.Log(angle.ToString());

            transform.LookAt(playerPosition);
            transform.Rotate(new Vector3(0, 180, 0));

            //transform.Rotate(new Vector3(0, 1, 0) * (currAngle));

            //transform.rotation = Quaternion.Euler(0, -currAngle, 0) * transform.rotation;

            //transform.eulerAngles = new Vector3(currAngle, 0, 0);

            targetPosition = new Vector3(transform.forward.x * 30, transform.position.y, transform.forward.z * 30);// transform.position.x + Mathf.Sin(currAngle) * 30, transform.position.y, transform.position.z + Mathf.Cos(currAngle) * 30);
            //targetPosition = new Vector3(transform.position.x - playerPosition.x, transform.position.y, transform.position.z - playerPosition.z);

        }
    }


}
