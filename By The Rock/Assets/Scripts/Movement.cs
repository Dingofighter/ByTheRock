using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour
{
    int counter;
    int counterIdleMax;
    bool invincibility;
    int invincibilityTimer;
    int maxMoveCounter;

    bool run;
    Vector3 playerPosition;
    bool walking;

    NavMeshAgent agent;
    Vector3 targetPosition;
    Vector3 lastPosition;
    Vector3 spawnPosition;
    float currAngle;
    float currDist;
    bool still;

    Renderer rend;
    public Color color;
    public Color colorEdge;
    public Color colorHit;
    public Color colorRun;
    public float outOfBoundsDist;
    public float maxDist;
    public float minDist;
    public float walkSpeed;
    public float acceleration;

    public int health;

    // Use this for initialization
    void Start()
    {

        counterIdleMax = Mathf.RoundToInt(Random.Range(100, 250));
        spawnPosition = transform.position;

        rend = GetComponent<Renderer>();
        rend.material.color = color;

        agent = GetComponent<NavMeshAgent>();
        agent.speed = walkSpeed;
        agent.acceleration = acceleration;
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

    public void standStill()
    {
        still = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.paused) return;

        if (health <= 0)
        {
            Destroy(gameObject);
            return;
        }

        if (still) return;
        
        if (!run)
        {
            counter++;
            if (((agent.velocity == Vector3.zero || maxMoveCounter >= 200) && walking) || (!walking && counter > counterIdleMax))
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
                        agent.SetDestination(lastPosition);
                    }
                    else
                    {
                        lastPosition = targetPosition;
                        currAngle = Random.Range(0, 360);
                        currDist = Random.Range(minDist, maxDist);

                        rend.material.color = color;

                        transform.Rotate(new Vector3(0, 1, 0) * currAngle);

                        agent.SetDestination(new Vector3(transform.position.x + transform.forward.x * currDist, transform.position.y, transform.position.z + transform.forward.z * currDist));
                        targetPosition = new Vector3(transform.position.x + transform.forward.x * currDist, transform.position.y, transform.position.z + transform.forward.z * currDist);
                    }
                }
                else
                {
                    maxMoveCounter = 0;
                    if (Vector3.Distance(spawnPosition, transform.position) > outOfBoundsDist)
                    {
                        rend.material.color = colorEdge;
                    }
                    else rend.material.color = color;
                }

            }

        }
        else
        {
            if (agent.velocity == Vector3.zero || maxMoveCounter >= 200)
            {
                maxMoveCounter = 0;
                run = false;
                agent.acceleration = acceleration;
                agent.speed = walkSpeed;
                spawnPosition = transform.position;

                targetPosition = transform.position;
                agent.SetDestination(transform.position);
            }
        }

        if (walking || run)
        {
            maxMoveCounter++;
        }

        if (invincibility)
        {
            agent.acceleration = acceleration * 1.5f;
            agent.speed = walkSpeed * 1.5f;
            rend.material.color = colorHit;
            invincibilityTimer++;
            if (invincibilityTimer > 50)
            {
                rend.material.color = color;
                agent.acceleration = acceleration;
                agent.speed = walkSpeed;
                rend.material.color = color;
                invincibility = false;
                invincibilityTimer = 0;
            }
        }
    }

    void OnTriggerStay(Collider c)
    {
        if (c.gameObject.tag == "Player" && !run && !c.GetComponent<PlayerMovement>().getCrouching() && !still)
        {
            maxMoveCounter = 0;
            rend.material.color = colorRun;
            run = true;
            playerPosition = c.gameObject.transform.position;

            transform.LookAt(playerPosition);
            transform.Rotate(new Vector3(0, 180, 0));

            agent.acceleration = acceleration * 2;
            agent.speed = walkSpeed * 2;

            agent.SetDestination(new Vector3(transform.forward.x * 30, transform.position.y, transform.forward.z * 30));
            targetPosition = new Vector3(transform.forward.x * 30, transform.position.y, transform.forward.z * 30);
        }
    }
}
