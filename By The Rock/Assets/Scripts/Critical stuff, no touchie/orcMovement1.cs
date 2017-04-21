using UnityEngine;
using System.Collections;

public class orcMovement1 : MonoBehaviour {

    int counter;
    int counterIdleMax;
    int maxMoveCounter;

    bool run;
    Vector3 playerPosition;
    bool walking;
    Transform player;

    NavMeshAgent agent;
    Vector3 targetPosition;
    Vector3 lastPosition;
    Vector3 spawnPosition;
    float currAngle;
    float currDist;
    bool shouldThrow;

    public Transform spearPre;
    Transform spear;
    int spearTimer;
    Transform enemy;
    bool playerAlive;

    Renderer rend;
    public Color color;
    public Color colorEdge;
    public Color colorRun;
    public float outOfBoundsDist;
    public float maxDist;
    public float minDist;
    public float walkSpeed;
    public float acceleration;

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

        player = FindObjectOfType<PlayerMovement1>().transform;
        
        checkForPlayer();

        shouldThrow = true;
    }

    void checkForPlayer()
    {
        if (Vector3.Distance(player.position, transform.position) > 7)
        {
            maxMoveCounter = 0;
            //Debug.Log("you're LEAVING MEEEE!! REEEEEEEEEEEEEEEEEEEEEEEEEEE");
            rend.material.color = colorRun;
            run = true;

            playerPosition = player.position;

            transform.LookAt(playerPosition);

            agent.acceleration = acceleration * 1.5f;
            agent.speed = walkSpeed * 1.5f;

            agent.SetDestination(playerPosition);
            targetPosition = playerPosition;
        }
    }

    public void hitByPlayer()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager1.instance.paused) return;
        
        if (shouldThrow)
        {
            spearTimer++;

            if (spearTimer == 1)
            {
                agent.SetDestination(transform.position);
                enemy = FindObjectOfType<Movement1>().transform;
                transform.LookAt(enemy.position);
                enemy.GetComponent<Movement1>().standStill();

                spear = (Transform)Instantiate(spearPre, new Vector3(transform.position.x, transform.position.y + 1.01f, transform.position.z) + (transform.right * 0.4f), Quaternion.Euler(new Vector3(79.95f, transform.eulerAngles.y, 0)));
                spear.GetComponent<Rigidbody>().detectCollisions = false;
                spear.GetComponent<Rigidbody>().useGravity = false;
                spear.GetComponentInChildren<Rigidbody>().detectCollisions = false;
                spear.GetComponentInChildren<Rigidbody>().useGravity = false;
            }
            else if (spearTimer == 60)
            {
                spear.GetComponent<Rigidbody>().useGravity = true;
                spear.GetComponent<Rigidbody>().detectCollisions = true;
                spear.GetComponentInChildren<Rigidbody>().detectCollisions = true;
                spear.GetComponentInChildren<Rigidbody>().useGravity = true;
                spear.GetComponent<Rigidbody>().AddForce(transform.forward * Vector3.Distance(transform.position, enemy.position) * 10 + transform.up * (enemy.position.y - transform.position.y) * 10 - transform.right * 5, ForceMode.Impulse);
                spear.GetComponent<Spear1>().isThrown = true;
                spearTimer = 0;
                //shouldThrow = false;
            }
            else if (spearTimer > 120)
            {
                spearTimer = 0;
            }
            return;
        }

        if (!run)
        {
            counter++;
            if (((agent.velocity == Vector3.zero || maxMoveCounter >= 200) && walking) || (!walking && counter > counterIdleMax))
            {
                walking = !walking;
                counterIdleMax = Mathf.RoundToInt(Random.Range(100, 250));
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
            if (agent.velocity == Vector3.zero || maxMoveCounter >= 200 || Vector3.Distance(player.position, transform.position) < 4)
            {
                maxMoveCounter = 0;
                if (Vector3.Distance(player.position, transform.position) > 4)
                {
                    //Debug.Log("STILL AWAY WAAH");
                    transform.LookAt(playerPosition);
                    agent.SetDestination(player.position);
                    targetPosition = playerPosition;
                }
                else
                {
                    //Debug.Log("I'm here");
                    run = false;
                    agent.acceleration = acceleration;
                    agent.speed = walkSpeed;
                    spawnPosition = transform.position;

                    targetPosition = transform.position;
                    agent.SetDestination(transform.position);
                }
            }
        }

        if (walking || run)
        {
            maxMoveCounter++;
        }

        if (playerAlive) checkForPlayer();
    }

  /*  void OnTriggerExit(Collider c)
    {
        if (c.gameObject.tag == "Player" && !run)
        {
            maxMoveCounter = 0;
            Debug.Log("you're LEAVING MEEEE!! REEEEEEEEEEEEEEEEEEEEEEEEEEE");
            rend.material.color = colorRun;
            run = true;
            
            playerPosition = c.gameObject.transform.position;

            transform.LookAt(playerPosition);

            agent.acceleration = acceleration * 1.5f;
            agent.speed = walkSpeed * 1.5f;

            agent.SetDestination(playerPosition);
            targetPosition = playerPosition;
        }
    }*/
}
