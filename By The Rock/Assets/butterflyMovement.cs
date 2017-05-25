﻿
using UnityEngine;

using System.Collections;



public class butterflyMovement : MonoBehaviour
{



    bool moving;

    float stillTimer;

    public int maxStillTime = 200;

    public int minStillTime = 30;

    public bool waitForPlayer = false;

    int stillTime;

    int currAngle;

    public float speed;



    public bool slaveMode;

    public float slaveDirectionDegrees;

    public float angleUp;

    bool begin;



    Transform player;



    // Use this for initialization

    void Start()
    {



        //moving = true;

        if (slaveMode)

        {

            

        }

        else

        {

            currAngle = Random.Range(0, 360);

            transform.Rotate(new Vector3(0, 0, 1) * currAngle);

            transform.eulerAngles = new Vector3(50, transform.eulerAngles.y, transform.eulerAngles.z);

        }



        stillTime = Random.Range(minStillTime, maxStillTime);



        //player = FindObjectOfType<PlayerController>().transform;



    }



    // Update is called once per frame

    void Update()
    {



        //if (GameManager.instance.paused) return;



        if (slaveMode)

        {

            if (moving)

            {

                Debug.Log(":(");
                transform.position += transform.up * speed * Time.deltaTime * 60;

                transform.Rotate(new Vector3(1, 0, 0) * 0.5f * Time.deltaTime * 60);

            }

            else

            {

                if (begin)

                {

                    moving = true;

                    stillTimer = 0;

                    transform.eulerAngles = new Vector3(90, transform.eulerAngles.y, transform.eulerAngles.z);

                    transform.Rotate(new Vector3(0, 0, 1) * slaveDirectionDegrees);

                    transform.eulerAngles = new Vector3(angleUp, transform.eulerAngles.y, transform.eulerAngles.z);

                }

            }



        }

        else

        {

            if (moving)

            {

                transform.position += transform.up * speed * Time.deltaTime * 60;

                transform.Rotate(new Vector3(1, 0, 0) * 0.5f * Time.deltaTime * 60);

            }

            else

            {

                if (!waitForPlayer) stillTimer += Time.deltaTime * 60;



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



    }



    public void startFlying()

    {
        transform.Rotate(new Vector3(0, 0, 1) * slaveDirectionDegrees);

        transform.eulerAngles = new Vector3(angleUp, transform.eulerAngles.y, transform.eulerAngles.z);
        begin = true;

    }



    public bool getMoving()

    {

        return moving;

    }



    public void collide(Collision c)
    {

        if (!slaveMode)
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



    void OnCollisionEnter(Collision c)

    {


        if (!slaveMode)
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

}





