using UnityEngine;
using System.Collections;

public class cameraScript : MonoBehaviour {

    bool moving;
    float moveTimer;

    public Vector3 goalPosition;
    public float speed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (moving)
        {
            moveTimer += Time.deltaTime * 60;
            transform.position = Vector3.Lerp(transform.position, goalPosition, speed);

            if (moveTimer > 350)
            {
                moving = false;
                Debug.Log("should play");
                FindObjectOfType<movieHandler>().playMovie();
            }
        }
        
	
	}

    public void startMoving()
    {
        moving = true;
    }
}
