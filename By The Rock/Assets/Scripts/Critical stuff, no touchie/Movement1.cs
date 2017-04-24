using UnityEngine;
using System.Collections;

public class Movement1 : MonoBehaviour
{
    int counter;
    
    Vector3 playerPosition;

    NavMeshAgent agent;
    Vector3 targetPosition;
    Vector3 lastPosition;
    Vector3 spawnPosition;
    float currAngle;
    float currDist;
    bool still;
    Transform player;
    bool playerAlive = true;

    Renderer rend;
    public Color color;
    public Color colorEdge;
    public Color colorHit;
    public Color colorRun;
    public float walkSpeed;
    public float acceleration;

    public int health;

    // Use this for initialization
    void Start()
    {
        
        spawnPosition = transform.position;

        rend = GetComponent<Renderer>();
        rend.material.color = color;

        agent = GetComponent<NavMeshAgent>();
        agent.speed = walkSpeed;
        agent.acceleration = acceleration;

        if (!GameManager1.instance.playerDead) player = FindObjectOfType<PlayerMovement1>().transform;
    }

    public void takeDamage(int i)
    {
        health -= i;
        GameManager1.instance.currentCash += 5;
        rend.material.color = colorHit;
    }

    public void standStill()
    {
        still = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager1.instance.paused || GameManager1.instance.inShop) return;

        //if (still) return;

        if (!GameManager1.instance.playerDead)
        {

            playerPosition = player.position;

            transform.LookAt(playerPosition);

            agent.SetDestination(playerPosition);
            targetPosition = playerPosition;
        }
        
        if (health <= 0)
        {
            GameManager1.instance.currentCash += 50;
            Destroy(gameObject);
            return;
        }
        
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Player")
        {
            GameManager1.instance.playerDead = true;
            Destroy(c.gameObject);
        }
    }
    
}
