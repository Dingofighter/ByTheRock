using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public float speed;

    private CharacterController charController;
    private Transform cam;
    private Animator anim;

    private int idleCounter;
    private bool turnRight;
    private bool turnLeft;

    private Vector3 camForward;


    /* Scene Loading Stuff */
    bool unload;
    string unloadName;

    private readonly int NONE = 0;
    private readonly int FOREST = 1;
    private readonly int VILLAGE = 2;

    Transform inter;

    DialogueHandler dialogueHandler;

    readonly int INGET = -1;
    readonly int SVAMP1 = 0;
    readonly int SVAMP2 = 1;
    readonly int SVAMP3 = 2;
    readonly int SVAMP4 = 3;
    readonly int SVAMP5 = 4;
    readonly int MOSSA = 5;
    readonly int VATTEN = 6;
    readonly int BARK = 7;
    readonly int ORT = 8;

    private bool interacting;
    private int interactTimer;

    GameObject itemToDestroy;
    int itemToAdd;
    int slotToAddTo;

    // Use this for initialization
    void Start()
    {
        charController = GetComponent<CharacterController>();
        cam = Camera.main.transform;
        anim = GetComponent<Animator>();
        dialogueHandler = FindObjectOfType<DialogueHandler>();
    }

    void Update()
    {
        if (GameManager.instance.paused)
        {
            return;
        }

        if (GameManager.instance.talking)
        {
            anim.SetFloat("Speed", 0);
            return;
        }

        if (interacting)
        {
            interactTimer++;
            if (interactTimer == 40)
            {
                GameManager.instance.changeItem(slotToAddTo, itemToAdd, false);
                Destroy(itemToDestroy);
            }

            if (interactTimer > 130)
            {
                interacting = false;
                interactTimer = 0;
                anim.SetBool("interacting", false);
            }
            else
            return;
        }

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        if (Input.GetButton("Walk"))
        {
            vertical = 1f;
        }

        //Calculate camera relative direction
        camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = vertical * camForward + horizontal * cam.right;



        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(movement.normalized);
        }

        if (!charController.isGrounded) movement += Physics.gravity;

        anim.SetFloat("Speed", Mathf.Abs(vertical) + Mathf.Abs(horizontal));
        if (Mathf.Abs(vertical) + Mathf.Abs(horizontal) > 0.1 && GameManager.instance.showingInventory) GameManager.instance.CloseInventory();
        charController.Move(movement * speed * Time.deltaTime);

        idleCounter++;
        anim.SetInteger("idleCounter", idleCounter);
        if (idleCounter >= 305) idleCounter = -150;
        if (!(Mathf.Abs(vertical) + Mathf.Abs(horizontal) == 0) || GameManager.instance.talking) idleCounter = 0;

        if (Input.GetKeyDown(KeyCode.E))
        {
            turnRight = !turnRight;
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            turnLeft = !turnLeft;
        }

        anim.SetBool("turnRight", turnRight);
        anim.SetBool("turnLeft", turnLeft);

        if (unload)
        {
            unload = false;
            SceneManager.UnloadScene(unloadName);
        }
    }

    void OnTriggerEnter(Collider c)
    {
        /* Scene Loading */
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

    void OnTriggerStay(Collider c)
    {
        if (GameManager.instance.paused) return;

        if (c.gameObject.tag == "Dialogue" && c.transform.parent.GetComponent<Dialogue>().autoTriggered && !GameManager.instance.shoulderView)
        {
            dialogueHandler.StartDialogue(c.GetComponentsInParent<Dialogue>());
            if (dialogueHandler.firstFrame)
            {
                //c.GetComponentInParent<Dialogue>().transform.LookAt(transform);
                //transform.LookAt(c.GetComponentInParent<Dialogue>().transform);
                //transform.rotation = new Quaternion(0, transform.rotation.y, 0, 0);
                //transform.LookAt(new Vector3(transform.right.x, c.transform.position.y, transform.forward.z));
                inter = c.gameObject.transform;

                if (!c.transform.parent.GetComponent<Dialogue>().walkAndTalk)
                {
                    if (c.transform.parent.GetComponent<Dialogue>().rotationTarget != null)
                    {
                        transform.rotation = Quaternion.Euler(0, c.transform.parent.GetComponent<Dialogue>().rotationTarget.eulerAngles.y + 180, 0);
                    }
                    else
                    {
                        transform.rotation = Quaternion.Euler(0, c.transform.eulerAngles.y + 180, 0);
                    }
                }
            }
        }

        if (Input.GetButtonDown("Interact") && !GameManager.instance.crouching)
        {

            if (c.gameObject.tag == "Mossa")
            {
                pickUp(0, MOSSA, c.transform.gameObject);
            }
            if (c.gameObject.tag == "Vatten")
            {
                pickUp(1, VATTEN, c.transform.gameObject);
            }
            if (c.gameObject.tag == "Bark")
            {
                pickUp(2, BARK, c.transform.gameObject);
            }
            if (c.gameObject.tag == "Ort")
            {
                pickUp(3, ORT, c.transform.gameObject);
            }
            if (c.gameObject.tag == "Svamp")
            {
                if (GameManager.instance.itemID2 >= INGET && GameManager.instance.itemID2 <= SVAMP4)
                {
                    itemToAdd = GameManager.instance.itemID2 + 1;
                    slotToAddTo = 1;
                    //GameManager.instance.changeItem(1, GameManager.instance.itemID2 + 1, false);
                }
                itemToDestroy = c.transform.gameObject;
                //Destroy(c.transform.gameObject);
                interacting = true;
                anim.SetBool("interacting", true);
            }


            if (c.gameObject.tag == "Dialogue" && !GameManager.instance.shoulderView)
            {
                //c.GetComponentInParent<Dialogue>().transform.LookAt(transform);
                //transform.LookAt(c.GetComponentInParent<Dialogue>().transform);
                //transform.rotation = new Quaternion(0, transform.rotation.y, 0, 0);
                //transform.LookAt(new Vector3(transform.right.x, c.transform.position.y, transform.forward.z));
                inter = c.gameObject.transform;

                if (!c.transform.parent.GetComponent<Dialogue>().walkAndTalk)
                {
                    transform.rotation = Quaternion.Euler(0, c.transform.eulerAngles.y + 180, 0);
                }
                FindObjectOfType<DialogueHandler>().StartDialogue(c.GetComponentsInParent<Dialogue>());
            }
        }
    }

    void pickUp(int slot, int itemID, GameObject item)
    {
        slotToAddTo = slot;
        itemToAdd = itemID;
        itemToDestroy = item;
        interacting = true;
        anim.SetBool("interacting", true);
    }

    void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "Butterfly")
        {
            Debug.Log("You ran on a butterfly YOU MONSTER");
            Physics.IgnoreCollision(c.collider, GetComponent<Collider>());
        }
    }

    public Transform getCollisionTransform()
    {
        return inter;
    }
}
