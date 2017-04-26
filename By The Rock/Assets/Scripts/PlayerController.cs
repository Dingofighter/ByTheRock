using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public float speed;

    private CharacterController charController;
    private Transform cam;
    private Animator anim;

    private Vector3 camForward;


    /* Scene Loading Stuff */
    bool unload;
    string unloadName;

    private readonly int NONE = 0;
    private readonly int FOREST = 1;
    private readonly int VILLAGE = 2;

    // Use this for initialization
    void Start () {
        charController = GetComponent<CharacterController>();
        cam = Camera.main.transform;
        anim = GetComponent<Animator>();
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

        anim.SetFloat("Speed", Mathf.Abs(vertical) + Mathf.Abs(horizontal));
        charController.Move(movement * speed * Time.deltaTime);

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
}
