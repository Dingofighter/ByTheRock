using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerMovement1 : MonoBehaviour
{

    Animator animator;
    Transform cam;
    public Transform spearPre;
    Transform spear;

    bool isWalking = false;
    bool isCrouching = false;

    bool unload;
    string unloadName;

    bool holdingSpear;
    bool chargingSpear;
    public static bool gotSpear;
    int shootDelay;

    private readonly int NONE = 0;
    private readonly int FOREST = 1;
    private readonly int VILLAGE = 2;

    int shootCounter;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        cam = Camera.main.transform;
        Object.DontDestroyOnLoad(this);
        GameManager1.instance.currScene = FOREST;
        GameManager1.instance.currSecondScene = NONE;
        gotSpear = true;
        shootDelay = 20;
    }

    public bool getCrouching()
    {
        return isCrouching;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager1.instance.inShop)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameManager1.instance.toggleShop();
            }
            if (Input.GetKeyDown(KeyCode.Alpha1) && GameManager1.instance.currentCash > 100 && !GameManager1.instance.bought1)
            {
                GameManager1.instance.bought1 = true;
                GameManager1.instance.currentCash -= 100;
                shootDelay = 15;
            }
        }


        if (GameManager1.instance.paused) return;

        if (!GameManager1.instance.talking && !GameManager1.instance.inShop)
        {
            GameManager1.instance.shoulderView = Input.GetMouseButton(1);

            /*
            if (GameManager.instance.crouching && !holdingSpear && gotSpear)
            {
                spear = (Transform)Instantiate(spearPre, new Vector3(transform.position.x, transform.position.y + 1.01f, transform.position.z) + (transform.right * 0.4f), Quaternion.Euler(new Vector3(79.95f, transform.eulerAngles.y, 0)));
                spear.GetComponent<Rigidbody>().detectCollisions = false;
                spear.GetComponent<Rigidbody>().useGravity = false;
                holdingSpear = true;
                spear.GetComponentInChildren<Rigidbody>().detectCollisions = false;
                spear.GetComponentInChildren<Rigidbody>().useGravity = false;
            }*/

            shootCounter++;

            if (Input.GetMouseButton(0) && GameManager1.instance.shoulderView && shootCounter > shootDelay)
            {
                shootCounter = 0;
                spear = (Transform)Instantiate(spearPre, new Vector3(transform.position.x, transform.position.y + 1.01f, transform.position.z) + (transform.right * 0.4f), Quaternion.Euler(new Vector3(79.95f, transform.eulerAngles.y, 0)));
                spear.GetComponent<Rigidbody>().detectCollisions = false;
                spear.GetComponent<Rigidbody>().useGravity = false;
                spear.GetComponentInChildren<Rigidbody>().detectCollisions = false;
                spear.GetComponentInChildren<Rigidbody>().useGravity = false;
                spear.transform.position = new Vector3(transform.position.x, transform.position.y + 1.01f, transform.position.z) + (transform.right * 0.4f);
                spear.transform.rotation = Quaternion.Euler(new Vector3(79.95f, transform.eulerAngles.y + Time.deltaTime * 2, 0));
                //gotSpear = false;
                chargingSpear = false;
                spear.GetComponent<Rigidbody>().useGravity = true;
                spear.GetComponent<Rigidbody>().detectCollisions = true;
                spear.GetComponentInChildren<Rigidbody>().detectCollisions = true;
                spear.GetComponentInChildren<Rigidbody>().useGravity = true;
                spear.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 85 + transform.up * 10, ForceMode.Impulse);
                spear.GetComponent<Spear1>().isThrown = true;
            }

            /*
            if (holdingSpear)
            {
                
                if (Input.GetMouseButtonDown(0))
                {
                    chargingSpear = true;
                }
            }

            if (Input.GetMouseButtonUp(0) && chargingSpear)
            {
                
            }*/

            if (!GameManager1.instance.shoulderView && holdingSpear)
            {
                holdingSpear = false;
                chargingSpear = false;
                Destroy(spear.gameObject);
            }

            float vertical = Input.GetAxis("Vertical");
            float horizontal = Input.GetAxis("Horizontal");

            // Calculate movement based on camera direction
            Vector3 camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 move = vertical * camForward + horizontal * cam.right;

            // Normalize movement to avoid faster diagonal movement
            if (move.magnitude > 1f)
            {
                move.Normalize();
            }

            // Translates movement vector to local space
            move = transform.InverseTransformDirection(move);
            // Calculate turn amount based on movement input
            float turnAmount = Mathf.Atan2(move.x, move.z);
            float forwardAmount = move.z;

            /*
            if (Input.GetButtonDown("Walk"))
            {
                isWalking = !isWalking;
            }*/

            if (Input.GetButtonDown("Crouch"))
            {
                isCrouching = !isCrouching;
                GameManager1.instance.crouching = !GameManager1.instance.crouching;
            }

            // Lower forward speed when walking
            if (isWalking)
            {
                animator.SetFloat("Forward", forwardAmount * 0.5f);
            }
            else
            {
                animator.SetFloat("Forward", forwardAmount);
            }

            animator.SetFloat("Turn", turnAmount);
            animator.SetBool("Crouch", isCrouching);
        }
        else
        {
            animator.SetFloat("Forward", 0);
            animator.SetFloat("Turn", 0);
        }

        
        

        if (unload)
        {
            unload = false;
            SceneManager.UnloadScene(unloadName);
        }
    }

    void OnTriggerEnter(Collider c)
    {

        if (c.gameObject.tag == "loadInWall" && GameManager1.instance.currSecondScene == NONE)
        {
            GameManager1.instance.currSecondScene = VILLAGE;
            SceneManager.LoadSceneAsync("AdditiveTest", LoadSceneMode.Additive);
        }
        if (c.gameObject.tag == "loadInWallRevert" && GameManager1.instance.currSecondScene == VILLAGE)
        {
            GameManager1.instance.currSecondScene = NONE;
            unload = true;
            unloadName = "AdditiveTest";
        }
        if (c.gameObject.tag == "loadOutWall" && GameManager1.instance.currScene == FOREST)
        {
            Debug.Log("unloading");
            GameManager1.instance.currScene = VILLAGE;
            GameManager1.instance.currSecondScene = NONE;
            unload = true;
            unloadName = "AITest";
            //SceneManager.UnloadScene("AITest");
            Debug.Log("unloaded");
        }
        if (c.gameObject.tag == "loadOutWallRevert" && GameManager1.instance.currScene == VILLAGE)
        {
            GameManager1.instance.currSecondScene = FOREST;
            SceneManager.LoadSceneAsync("AITest", LoadSceneMode.Additive);
        }
    }
}