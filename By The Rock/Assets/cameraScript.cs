using UnityEngine;
using System.Collections;

public class cameraScript : MonoBehaviour {

    bool moving;
    float moveTimer;

    public Transform goalPosition;
    public float speed;

    public GameObject film; 

	// Use this for initialization
	void Start () {

        film.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {

        if (moving)
        {
            moveTimer += Time.deltaTime * 60;
            transform.position = Vector3.Lerp(transform.position, goalPosition.position, speed * Time.deltaTime * 60);

            if (moveTimer > 260)
            {
                FindObjectOfType<butterflyMovement>().startFlying();
            }

            if (moveTimer > 380)
            {
                
                transform.position = goalPosition.position;
                moving = false;
                Debug.Log("should play");
                film.SetActive(true);
                FindObjectOfType<movieHandler>().playMovie();
            }
        }
        
	
	}

    public void startMoving()
    {
        moving = true;
    }
}
