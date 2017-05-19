using UnityEngine;
using System.Collections;

public class squirrelMovement : MonoBehaviour {

    int timer;
    public int treeLength;
    public int timeDown;
    public int timeUp;
    public float spinSpeed;
    public float runSpeed;

    Vector3 startPosition;
    Quaternion startRotation;

	// Use this for initialization
	void Start () {

        startPosition = transform.position;
        startRotation = transform.rotation;

	}
	
	// Update is called once per frame
	void Update () {

        if (GameManager.instance.paused) return;

        timer++;

        if (timer < treeLength)
        {
            transform.position += transform.forward * runSpeed;
            transform.Rotate(new Vector3(0, 0, spinSpeed));
            transform.position -= transform.right * runSpeed;
            
        }
        else if (timer == treeLength + (timeDown - 1))
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x + 180, transform.eulerAngles.y + 180, transform.eulerAngles.z);
            //transform.Rotate(new Vector3(180, 0, 0));
        }
        else if (timer <= treeLength + timeDown) { }
        else if (timer < treeLength*2 + timeDown)
        {
            transform.position += transform.forward * runSpeed;
            transform.Rotate(new Vector3(0, 0, spinSpeed));
            transform.position -= transform.right * runSpeed;
        }
        else if (timer < treeLength * 2 + timeDown + timeUp)
        {
            transform.position = Vector3.Lerp(transform.position, startPosition, 0.1f);
            transform.rotation = Quaternion.Lerp(transform.rotation, startRotation, 0.1f);
        }
        else if (timer == treeLength*2 + timeDown + timeUp)
        {
            timer = 0;
            
            //transform.Rotate(new Vector3(180, 0, 0));
        }
	
	}
}
