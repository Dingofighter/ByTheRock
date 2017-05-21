using UnityEngine;
using System.Collections;

public class orcMovement : MonoBehaviour {

    float counter;
    int counterIdleMax;
    float maxMoveCounter;

    bool run;
    Vector3 playerPosition;
    bool walking;
    Transform player;
    Animator anim;

    NavMeshAgent agent;
    Vector3 targetPosition;
    Vector3 lastPosition;
    Vector3 spawnPosition;
    float currAngle;
    float currDist;
    bool shouldThrow;

    //public Transform spearPre;
    //Transform spear;
    int spearTimer;
    Transform enemy;

    Renderer rend;
    public Color color;
    public Color colorEdge;
    public Color colorRun;
    public float outOfBoundsDist;
    public float maxDist;
    public float minDist;
    public float walkSpeed;
    public float acceleration;
    public int state;

    float idleCounter;
    float talkTimer;

    readonly int FOLLOW = 0;
    readonly int TARGET = 1;
    readonly int WAIT = 2;

    public Transform runPositionTargetVeryYes;
    

    // Use this for initialization
    void Start()
    {

        counterIdleMax = Mathf.RoundToInt(Random.Range(250, 400));
        spawnPosition = transform.position;

        //rend = GetComponent<Renderer>();
        //rend.material.color = color;

        agent = GetComponent<NavMeshAgent>();
        agent.speed = walkSpeed;
        agent.acceleration = acceleration;

        player = FindObjectOfType<PlayerController>().transform;
        
        if (state == FOLLOW) checkForPlayer();
        if (state == TARGET)
        {
            targetPosition = player.position;
            agent.SetDestination(targetPosition);
        }

        anim = GetComponent<Animator>();

        //shouldThrow = true;
    }

    void checkForPlayer()
    {
        if (Vector3.Distance(player.position, transform.position) > 7)
        {
            maxMoveCounter = 0;
            //Debug.Log("you're LEAVING MEEEE!! REEEEEEEEEEEEEEEEEEEEEEEEEEE");
            //rend.material.color = colorRun;
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
        if (GameManager.instance.paused) return;

        idleCounter += Time.deltaTime*60;
        if (idleCounter >= 270) idleCounter = 0;
        int temp = (int)idleCounter;
        anim.SetInteger("idleCounter", temp);

        anim.SetFloat("Speed", (Mathf.Abs(agent.velocity.z) + Mathf.Abs(agent.velocity.x) + Mathf.Abs(agent.velocity.y))/10);

        if (GameManager.instance.farTalking)
        {
            if (Vector3.Distance(transform.position, player.position) > 5) agent.SetDestination(player.position + player.forward);
            else
            {
                //anim.SetBool("talking", true);

                Vector3 tempAngles = transform.eulerAngles;
                transform.LookAt(player.transform);
                transform.eulerAngles = new Vector3(tempAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);

                talkTimer += Time.deltaTime * 60;
                if (talkTimer >= 100)
                {
                    talkTimer = 0;
                    anim.SetBool("talkHands", !anim.GetBool("talkHands"));
                }
                return;
            }
        }
        else if (Vector3.Distance(player.transform.position, transform.position) < 3 && GameManager.instance.talking)
        {
            //anim.SetBool("talking", true);

            Vector3 tempAngles = transform.eulerAngles;
            transform.LookAt(player.transform);
            transform.eulerAngles = new Vector3(tempAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);

            talkTimer += Time.deltaTime * 60;
            if (talkTimer >= 100)
            {
                talkTimer = 0;
                anim.SetBool("talkHands", !anim.GetBool("talkHands"));
            }
            return;
        }
        else { } //anim.SetBool("talking", false);

        if (Vector3.Distance(player.position, transform.position) < 1)
        {
            agent.SetDestination(transform.position);
            return;
        }

        if (AllFlags.Instance.flags[1].value == true && !GameManager.instance.talking)
        {
            state = TARGET;
            agent.SetDestination(runPositionTargetVeryYes.position);
        }
        
        if (state == FOLLOW)
        {
            if (!run)
            {
                counter += Time.deltaTime*60;
                if (((agent.velocity == Vector3.zero || maxMoveCounter >= 400) && walking) || (!walking && counter > counterIdleMax))
                {
                    walking = !walking;
                    counterIdleMax = Mathf.RoundToInt(Random.Range(250, 400));
                    counter = 0;
                    if (walking)
                    {
                        if (Vector3.Distance(spawnPosition, transform.position) > outOfBoundsDist)
                        {
                            //rend.material.color = colorEdge;
                            transform.Rotate(new Vector3(0, 1, 0) * 180);
                            targetPosition = lastPosition;
                            agent.SetDestination(lastPosition);
                        }
                        else
                        {
                            lastPosition = targetPosition;
                            currAngle = Random.Range(0, 360);
                            currDist = Random.Range(minDist, maxDist);

                            //rend.material.color = color;

                            transform.Rotate(new Vector3(0, 1, 0) * currAngle);

                            agent.SetDestination(new Vector3(transform.position.x + transform.forward.x * currDist, transform.position.y - 100, transform.position.z + transform.forward.z * currDist));
                            targetPosition = new Vector3(transform.position.x + transform.forward.x * currDist, transform.position.y - 100, transform.position.z + transform.forward.z * currDist);
                        }
                    }
                    else
                    {
                        maxMoveCounter = 0;
                        if (Vector3.Distance(spawnPosition, transform.position) > outOfBoundsDist)
                        {
                            //rend.material.color = colorEdge;
                        }
                        else { } //rend.material.color = color;
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
        }
        else if (state == TARGET)
        {
            if (agent.velocity == Vector3.zero || maxMoveCounter > 1000)
            {
                maxMoveCounter = 0;
                state = WAIT;
            }
        } 
        else if (state == WAIT)
        {
            agent.SetDestination(transform.position);
            state = FOLLOW;
        }
        
        if (state == FOLLOW || state == TARGET && (walking || run))
        {
            maxMoveCounter += Time.deltaTime*60;
        }

        if (state == FOLLOW)
        {
            checkForPlayer();
        }
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
