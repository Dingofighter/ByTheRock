using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
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

    private readonly int NONE = 0;
    private readonly int FOREST = 1;
    private readonly int VILLAGE = 2;

    // Use this for initialization
    void Start()
    {
        animator = GetComponent<Animator>();
        cam = Camera.main.transform;
        Object.DontDestroyOnLoad(this);
        GameManager.instance.currScene = FOREST;
        GameManager.instance.currSecondScene = NONE;
        gotSpear = true;
    }

    public bool getCrouching()
    {
        return isCrouching;
    }

    // Update is called once per frame
    void Update()
    {

        if (GameManager.instance.paused) return;

        if (!GameManager.instance.talking)
        {
            if (GameManager.instance.crouching && !holdingSpear && gotSpear)
            {
                spear = (Transform)Instantiate(spearPre, new Vector3(transform.position.x, transform.position.y + 1.01f, transform.position.z) + (transform.right * 0.4f), Quaternion.Euler(new Vector3(79.95f, transform.eulerAngles.y, 0)));
                spear.GetComponent<Rigidbody>().detectCollisions = false;
                spear.GetComponent<Rigidbody>().useGravity = false;
                holdingSpear = true;
                spear.GetComponentInChildren<Rigidbody>().detectCollisions = false;
                spear.GetComponentInChildren<Rigidbody>().useGravity = false;
            }

            /*
            if (Input.GetMouseButtonDown(0) && GameManager.instance.shoulderView)
            {
                spear = (Transform)Instantiate(spearPre, new Vector3(transform.position.x, transform.position.y + 1.01f, transform.position.z) + (transform.right * 0.4f), Quaternion.Euler(new Vector3(79.95f, transform.eulerAngles.y, 0)));
                spear.GetComponent<Rigidbody>().detectCollisions = false;
                spear.GetComponent<Rigidbody>().useGravity = false;
                holdingSpear = true;
                spear.GetComponentInChildren<Rigidbody>().detectCollisions = false;
                spear.GetComponentInChildren<Rigidbody>().useGravity = false;
            }
            */

            if (holdingSpear)
            {
                spear.transform.position = new Vector3(transform.position.x, transform.position.y + 1.01f, transform.position.z) + (transform.right * 0.4f);
                spear.transform.rotation = Quaternion.Euler(new Vector3(79.95f, transform.eulerAngles.y + Time.deltaTime*2, 0));
                if (Input.GetMouseButtonDown(0))
                {
                    chargingSpear = true;
                }
            }

            if (Input.GetMouseButtonUp(0) && chargingSpear)
            {
                holdingSpear = false;
                //gotSpear = false;
                chargingSpear = false;
                spear.GetComponent<Rigidbody>().useGravity = true;
                spear.GetComponent<Rigidbody>().detectCollisions = true;
                spear.GetComponentInChildren<Rigidbody>().detectCollisions = true;
                spear.GetComponentInChildren<Rigidbody>().useGravity = true;
                spear.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 85 + transform.up * 10, ForceMode.Impulse);
                spear.GetComponent<Spear>().isThrown = true;
            }

            if (!GameManager.instance.shoulderView && holdingSpear)
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

            if (Input.GetButtonDown("Walk"))
            {
                isWalking = !isWalking;
            }

            if (Input.GetButtonDown("Crouch"))
            {
                isCrouching = !isCrouching;
                GameManager.instance.crouching = !GameManager.instance.crouching;
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
        if (c.gameObject.tag == "loadInWall" && GameManager.instance.currSecondScene == NONE)
        {
            GameManager.instance.currSecondScene = VILLAGE;
            SceneManager.LoadSceneAsync("AdditiveTest", LoadSceneMode.Additive);
        }
        if (c.gameObject.tag == "loadInWallRevert" && GameManager.instance.currSecondScene == VILLAGE)
        {
            GameManager.instance.currSecondScene = NONE;
            unload = true;
            unloadName = "AdditiveTest";
        }
        if (c.gameObject.tag == "loadOutWall" && GameManager.instance.currScene == FOREST)
        {
            Debug.Log("unloading");
            GameManager.instance.currScene = VILLAGE;
            GameManager.instance.currSecondScene = NONE;
            unload = true;
            unloadName = "AITest";
            //SceneManager.UnloadScene("AITest");
            Debug.Log("unloaded");
        }
        if (c.gameObject.tag == "loadOutWallRevert" && GameManager.instance.currScene == VILLAGE)
        {
            GameManager.instance.currSecondScene = FOREST;
            SceneManager.LoadSceneAsync("AITest", LoadSceneMode.Additive);
        }
    }
}