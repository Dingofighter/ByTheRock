using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{

    public float speed;

    private CharacterController charController;
    private Transform cam;
    private Animator anim;

    private float idleCounter;
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
    private float interactTimer;
    public bool crouching;
    private bool turnAround;

    GameObject itemToDestroy;
    int itemToAdd;
    bool removeMushroom;
    bool once;

    public GameObject buttonImg;
    public PickupEmitter pickupemitter;


    // Use this for initialization
    void Start()
    {
        charController = GetComponent<CharacterController>();
        cam = Camera.main.transform;
        anim = GetComponent<Animator>();
        pickupemitter = GetComponent<PickupEmitter>();

        buttonImg.SetActive(false);

        dialogueHandler = FindObjectOfType<DialogueHandler>();

        DontDestroyOnLoad(this);
    }

    void LateUpdate()
    {
        buttonImg.transform.LookAt(cam);
    }

    void Update()
    {
        if (GameManager.instance.paused || GameManager.instance.fadingAtm)
        {
            return;
        }

        if (GameManager.instance.itemID1 == SVAMP5 ||
            GameManager.instance.itemID2 == SVAMP5 ||
            GameManager.instance.itemID3 == SVAMP5 ||
            GameManager.instance.itemID4 == SVAMP5)
        {
            AllFlags.Instance.flags[25].value = true;
        }

        if (GameManager.instance.talking)
        {
            anim.SetFloat("Speed", 0);
            //anim.SetBool("talking", true);
            if (interacting)
            {
                interactTimer += Time.deltaTime*60;

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
        else if (crouching && !GameManager.instance.talking)
        {
            crouching = false;
            anim.SetBool("crouching", false);
        }
        
        //anim.SetBool("talking", false);
        GameManager.instance.farTalking = false;

        if (interacting)
        {
            interactTimer += Time.deltaTime * 60;
            if (!once)
            {
                //itemToDestroy.GetComponent<BoxCollider>().enabled = false;
                //itemToDestroy.GetComponentInChildren<BoxCollider>().enabled = false;
            }
            if (interactTimer >= 40 && !once)
            {
                once = true;
                if (removeMushroom)
                {
                    removeFromInv(SVAMP1);
                    removeFromInv(SVAMP2);
                    removeFromInv(SVAMP3);
                    removeFromInv(SVAMP4);
                    removeMushroom = false;
                }

                GameManager.instance.changeItem(itemToAdd, false, false);
                itemToDestroy.GetComponent<Interactable>().Interact();
                Destroy(itemToDestroy);

            }

            if (!anim.GetCurrentAnimatorStateInfo(0).IsTag("interacting") && interactTimer > 40)
            //if (interactTimer > 130)
            {
                Debug.Log("klar");
                interacting = false;
                interactTimer = 0;
                anim.SetBool("interacting", false);
            }
            else
            return;
        }
        /*
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
        */
        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");

        if (Input.GetButton("Walk"))
        {
            vertical = 1f;
        }

        //Calculate camera relative direction
        camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 movement = vertical * camForward + horizontal * cam.right;

        movement = Vector3.ClampMagnitude(movement, 1);

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(movement.normalized);
        }

        if (!charController.isGrounded) movement += Physics.gravity;

        anim.SetFloat("Speed", Mathf.Abs(vertical) + Mathf.Abs(horizontal));
        if (Mathf.Abs(vertical) + Mathf.Abs(horizontal) > 0.1 && GameManager.instance.showingInventory) GameManager.instance.CloseInventory();
        charController.Move(movement * speed * Time.deltaTime);

        idleCounter += Time.deltaTime*60;
        int temp = (int)idleCounter;
        anim.SetInteger("idleCounter", temp);
        if (idleCounter >= 305)
        {
            idleCounter = -150;
            anim.SetBool("otherIdle", !anim.GetBool("otherIdle"));
        }
        if (!(Mathf.Abs(vertical) + Mathf.Abs(horizontal) == 0) || GameManager.instance.talking) idleCounter = 0;
        /*
        if (Input.GetKeyDown(KeyCode.E))
        {
            //turnRight = !turnRight;
            crouching = !crouching;

        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            turnLeft = !turnLeft;
            Debug.Log("pressed to fade");
            GameManager.instance.fadeToBlack = true;
        }
        */
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

        /*INV WALL*/
        if (c.gameObject.tag == "TurnAroundWall")
        {
            print("TURN AROUND");
            turnAround = true;
        }
    }

    void OnTriggerExit(Collider c)
    {
        buttonImg.SetActive(false);

        if (c.gameObject.tag == "Glow")
        {
            c.transform.parent.GetComponent<Renderer>().material.shader = Shader.Find("Standard");
        }
    }

    void OnTriggerStay(Collider c)
    {
        if (GameManager.instance.paused) return;

        string s = c.gameObject.tag;

        if (s == "Dialogue" && !GameManager.instance.talking && !c.transform.parent.GetComponent<Dialogue>().autoTriggered && !c.transform.parent.GetComponent<Dialogue>().walkAndTalk || s == "Mossa" || s == "Vatten" || s == "Bark" || s == "Ort" || s == "Svamp")
        {

            //buttonImg.transform.position = c.transform.position - c.transform.right * 0.7f + new Vector3(0, 1.3f, 0);

            if (s != "Dialogue" && s != "Svamp")
            { 
                if (AllFlags.Instance.flags[3].value)
                {
                    switch (s)
                    {
                        default:
                            break;
                        case "Bark":
                        case "Mossa":
                        case "Ort":
                            buttonImg.transform.position = c.transform.position + new Vector3(0, 0.8f, 0);
                            buttonImg.transform.LookAt(cam);
                            //buttonImg.SetActive(true);
                            break;
                        case "Vatten":
                            buttonImg.transform.position = cam.position + new Vector3(2, 0, 0f);
                            buttonImg.transform.LookAt(cam);
                            //buttonImg.SetActive(true);
                            break;
                    }
                }
            }
            else
            {
                if (!AllFlags.Instance.flags[3].value)
                {
                    switch (c.gameObject.name)
                    {
                        default:
                            break;
                        case "Moss":
                        case "Water":
                        case "Barkis":
                        case "Herb":
                            return;
                    }
                }

                if (!AllFlags.Instance.flags[24].value && s == "Svamp" || AllFlags.Instance.flags[25].value && s == "Svamp")
                    return;
                else if (AllFlags.Instance.flags[24].value && s == "Svamp")
                {
                    buttonImg.transform.position = c.transform.position + new Vector3(0, 0.8f, 0);
                    buttonImg.transform.LookAt(cam);
                    //buttonImg.SetActive(true);
                }


                if (c.gameObject.name == "DialougeTriggerHania")
                {
                    if (AllFlags.Instance.flags[24].value && !AllFlags.Instance.flags[20].value)
                    {
                    }
                    else
                    {
                        buttonImg.transform.position = c.transform.position + new Vector3(0, 0.8f, 0);
                        buttonImg.transform.LookAt(cam);
                        //buttonImg.SetActive(true);
                    }
                        

                    if (AllFlags.Instance.flags[22].value)
                    {
                    }
                    else
                    {
                        buttonImg.transform.position = c.transform.position + new Vector3(0, 0.8f, 0);
                        buttonImg.transform.LookAt(cam);
                        //buttonImg.SetActive(true);
                    }





                }             
                else
                {
                    if (c.gameObject.name == "DialougeTriggerGaregh")
                    {

                        if (AllFlags.Instance.flags[25].value && !AllFlags.Instance.flags[22].value)
                        {
                        }
                        else
                        {
                            buttonImg.transform.position = c.transform.position + new Vector3(0, 1.7f, 0);
                            buttonImg.transform.LookAt(cam);
                            //buttonImg.SetActive(true);
                        }

                    }


                }


            }


   

        }
        else
        {
            buttonImg.SetActive(false);
        }

        if (s == "Glow")
        {
            c.transform.parent.GetComponent<Renderer>().material.shader = Shader.Find("RimLightning Lerp");
        }

        if (c.gameObject.tag == "Dialogue" && c.transform.parent.GetComponent<Dialogue>().autoTriggered && !GameManager.instance.shoulderView)
        {
          
            dialogueHandler.StartDialogue(c.GetComponentsInParent<Dialogue>());

            buttonImg.SetActive(false);
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
                        Debug.Log("rotating");
                        Vector3 tempAngles = transform.eulerAngles;
                        transform.LookAt(c.transform.parent.GetComponent<Dialogue>().rotationTarget);
                        transform.eulerAngles = new Vector3(tempAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);
                        GameManager.instance.farTalking = true;
                        //transform.rotation = Quaternion.Euler(0, c.transform.parent.GetComponent<Dialogue>().rotationTarget.eulerAngles.y + 180, 0);
                    }
                    else
                    {
                        transform.rotation = Quaternion.Euler(0, c.transform.eulerAngles.y + 180, 0);
                    }
                }
                buttonImg.SetActive(false);
            }
        }

        if (Input.GetButtonDown("Interact") && !GameManager.instance.crouching)
        {
            if (!AllFlags.Instance.flags[3].value)
            {
                switch (c.gameObject.name)
                {
                    default:
                        break;
                    case "Moss":
                    case "Water":
                    case "Barkis":
                    case "Herb":
                    case "Svamp":
                        return;
                }
            }


                if (c.gameObject.tag == "Mossa")
                {
                    if (c.GetComponent<Interactable>().CheckInteractable())
                    {
                        pickUp(MOSSA, c.transform.gameObject);
                        //c.GetComponent<Interactable>().Interact();
                    }
                }
                if (c.gameObject.tag == "Vatten")
                {
                    if (c.GetComponent<Interactable>().CheckInteractable())
                    {
                        pickUp(VATTEN, c.transform.gameObject);
                        //c.GetComponent<Interactable>().Interact();
                    }
                }
                if (c.gameObject.tag == "Bark")
                {
                    if (c.GetComponent<Interactable>().CheckInteractable())
                    {
                        pickUp(BARK, c.transform.gameObject);
                        //c.GetComponent<Interactable>().Interact();
                    }
                }
                if (c.gameObject.tag == "Ort")
                {
                    if (c.GetComponent<Interactable>().CheckInteractable())
                    {
                        pickUp(ORT, c.transform.gameObject);
                        //c.GetComponent<Interactable>().Interact();
                    }
                }
                if (c.gameObject.tag == "Svamp")
                {
                    if (c.GetComponent<Interactable>().CheckInteractable())
                    {
                        if (GameManager.instance.itemID1 >= INGET && GameManager.instance.itemID1 <= SVAMP4)
                        {
                            int temp = GameManager.instance.itemID1;
                            removeMushroom = true;
                            pickUp(temp + 1, c.transform.gameObject);
                            //c.GetComponent<Interactable>().Interact();
                        
                        }
                        else if (GameManager.instance.itemID2 >= INGET && GameManager.instance.itemID2 <= SVAMP4)
                        {
                            int temp = GameManager.instance.itemID2;
                            removeMushroom = true;
                            pickUp(temp + 1, c.transform.gameObject);
                            //c.GetComponent<Interactable>().Interact();
                        }
                        else if (GameManager.instance.itemID3 >= INGET && GameManager.instance.itemID3 <= SVAMP4)
                        {
                            int temp = GameManager.instance.itemID3;
                            removeMushroom = true;
                            pickUp(temp + 1, c.transform.gameObject);
                            //c.GetComponent<Interactable>().Interact();
                        }
                        else if (GameManager.instance.itemID4 >= INGET && GameManager.instance.itemID4 <= SVAMP4)
                        {
                            int temp = GameManager.instance.itemID4;
                            removeMushroom = true;
                            pickUp(temp + 1, c.transform.gameObject);
                            //c.GetComponent<Interactable>().Interact();
                        }
                        else return;
                    }
                }

            if (c.gameObject.tag == "Dialogue" && !GameManager.instance.shoulderView && !GameManager.instance.talking)
            {
                //Debug.Log("tlaking      ;");
                //c.GetComponentInParent<Dialogue>().transform.LookAt(transform);
                //transform.LookAt(c.GetComponentInParent<Dialogue>().transform);
                //transform.rotation = new Quaternion(0, transform.rotation.y, 0, 0);
                //transform.LookAt(new Vector3(transform.right.x, c.transform.position.y, transform.forward.z));
                inter = c.gameObject.transform;

                if (!c.transform.parent.GetComponent<Dialogue>().walkAndTalk)
                {
                    Vector3 tempAngles = transform.eulerAngles;
                    transform.LookAt(c.transform);
                    transform.eulerAngles = new Vector3(tempAngles.x, transform.eulerAngles.y, transform.eulerAngles.z);

                    /*tempAngles = c.transform.eulerAngles;
                    c.transform.LookAt(transform);
                    c.transform.eulerAngles = new Vector3(tempAngles.x, c.transform.eulerAngles.y, c.transform.eulerAngles.z);*/

                    //Debug.Log("turned");
                    //transform.rotation = Quaternion.Euler(0, c.transform.eulerAngles.y + 180, 0);
                }
 
                FindObjectOfType<DialogueHandler>().StartDialogue(c.GetComponentsInParent<Dialogue>());

                if (c.transform.parent.gameObject.tag == "Hania")
                {
                    //Debug.Log("talk han");
                    if (currentlyHolding(MOSSA)) removeFromInv(MOSSA);
                    if (currentlyHolding(VATTEN)) removeFromInv(VATTEN);
                    if (currentlyHolding(BARK)) removeFromInv(BARK);
                    if (currentlyHolding(ORT)) removeFromInv(ORT);
                    if (currentlyHolding(SVAMP5)) removeFromInv(SVAMP5);
                }
            }
        }
    }

    void pickUp(int itemID, GameObject item)
    {
        itemToAdd = itemID;
        itemToDestroy = item;
        
        interacting = true;
        once = false;
        anim.SetBool("interacting", true);
    }

    void removeFromInv(int itemID)
    {
        GameManager.instance.changeItem(itemID, false, true);
        if (!removeMushroom)
        {
            GameManager.instance.givingItem = true;
            interacting = true;
            once = false;
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
