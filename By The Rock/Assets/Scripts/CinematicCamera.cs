using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CinematicCamera : MonoBehaviour {

    public float speed;
    public Transform target;

    bool moving = true;

    public Animator animator;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (moving)
        {
            transform.position = Vector3.Lerp(transform.position, target.transform.position, Time.deltaTime * speed);
            transform.rotation = Quaternion.Lerp(transform.rotation, target.transform.rotation, Time.deltaTime * speed);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
    }

    void OnTriggerEnter(Collider c)
    {
        print("Trigger");
        if (c.tag == "TurnAroundWall")
        {
            animator.SetBool("Pickup", true);
            moving = false;
        }
    }
}
