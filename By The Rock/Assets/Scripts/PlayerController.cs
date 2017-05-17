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
    private bool crouching;

    GameObject itemToDestroy;
    int itemToAdd;
    bool removeMushroom;

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
            anim.SetBool("talking", true);
            if (interacting)
            {
                interactTimer++;

                if (interactTimer > 30)
                {
                    interacting = false;
                    interactTimer = 0;
                    GameManager.instance.givingItem = false;
                    anim.SetBool("interacting", false);
                }
                else
                return;
            }
            return;
        }
        
        anim.SetBool("talking", false);

        if (interacting)
        {
            interactTimer++;
            if (interactTimer == 40)
            {
                if (removeMushroom)
                {
                    removeFromInv(SVAMP1);
                    removeFromInv(SVAMP2);
                    removeFromInv(SVAMP3);
                    removeFromInv(SVAMP4);
                    removeMushroom = false;
                }

                GameManager.instance.changeItem(itemToAdd, false, false);
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

        if (crouching || anim.GetCurrentAnimatorStateInfo(0).IsName("Crouch Up") || anim.GetCurrentAnimatorStateInfo(0).IsName("Crouch Idle"))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                //turnRight = !turnRight;
                crouching = !crouching;
                anim.SetBool("crouching", crouching);

            }
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
            //turnRight = !turnRight;
            crouching = !crouching;

        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            turnLeft = !turnLeft;
        }

        anim.SetBool("crouching", crouching);
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
                pickUp(MOSSA, c.transform.gameObject);
            }
            if (c.gameObject.tag == "Vatten")
            {
                pickUp(VATTEN, c.transform.gameObject);
            }
            if (c.gameObject.tag == "Bark")
            {
                pickUp(BARK, c.transform.gameObject);
            }
            if (c.gameObject.tag == "Ort")
            {
                pickUp(ORT, c.transform.gameObject);
            }
            if (c.gameObject.tag == "Svamp")
            {
                if (GameManager.instance.itemID1 >= INGET && GameManager.instance.itemID1 <= SVAMP4)
                {
                    int temp = GameManager.instance.itemID1;
                    removeMushroom = true;
                    pickUp(temp + 1, c.transform.gameObject);
                }
                else if (GameManager.instance.itemID2 >= INGET && GameManager.instance.itemID2 <= SVAMP4)
                {
                    int temp = GameManager.instance.itemID2;
                    removeMushroom = true;
                    pickUp(temp + 1, c.transform.gameObject);
                }
                else if (GameManager.instance.itemID3 >= INGET && GameManager.instance.itemID3 <= SVAMP4)
                {
                    int temp = GameManager.instance.itemID3;
                    removeMushroom = true;
                    pickUp(temp + 1, c.transform.gameObject);
                }
                else if (GameManager.instance.itemID4 >= INGET && GameManager.instance.itemID4 <= SVAMP4)
                {
                    int temp = GameManager.instance.itemID4;
                    removeMushroom = true;
                    pickUp(temp + 1, c.transform.gameObject);
                }
                else return;
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

                if (c.transform.parent.gameObject.tag == "Hania")
                {
                    Debug.Log("talk han");
                    if (currentlyHolding(MOSSA)) removeFromInv(MOSSA);
                    if (currentlyHolding(VATTEN)) removeFromInv(VATTEN);
                    if (currentlyHolding(BARK)) removeFromInv(BARK);
                    if (currentlyHolding(ORT)) removeFromInv(ORT);
                }
            }
        }
    }

    void pickUp(int itemID, GameObject item)
    {
        itemToAdd = itemID;
        itemToDestroy = item;
        interacting = true;
        anim.SetBool("interacting", true);
    }

    void removeFromInv(int itemID)
    {
        GameManager.instance.changeItem(itemID, false, true);
        if (!removeMushroom)
        {
            GameManager.instance.givingItem = true;
            interacting = true;
            anim.SetBool("interacting", true);
        }
    }

    bool currentlyHolding(int itemID)
    {
        return (GameManager.instance.itemID1 == itemID ||
            GameManager.instance.itemID2 == itemID ||
            GameManager.instance.itemID3 == itemID ||
            GameManager.instance.itemID4 == itemID);
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
