using UnityEngine;
using System.Collections;

public class squirrelMovement : MonoBehaviour {

    float timer;
    public int treeLength;
    public int timeDown;
    public int timeUp;
    public float spinSpeed;
    public float runSpeed;

    bool turned;

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

        timer += Time.deltaTime * 60;

        if (timer < treeLength)
        {
            transform.position += transform.forward * runSpeed * Time.deltaTime*60;
            transform.Rotate(new Vector3(0, 0, spinSpeed) * Time.deltaTime * 60);
            transform.position -= transform.right * runSpeed * Time.deltaTime * 60;
            
        }
        else if (timer >= treeLength + (timeDown - 1) && !turned)
        {
            turned = true;
            transform.eulerAngles = new Vector3(transform.eulerAngles.x + 180, transform.eulerAngles.y + 180, transform.eulerAngles.z);
            //transform.Rotate(new Vector3(180, 0, 0));
        }
        else if (timer <= treeLength + timeDown) { }
        else if (timer < treeLength*2 + timeDown)
        {
            transform.position += transform.forward * runSpeed * Time.deltaTime * 60;
            transform.Rotate(new Vector3(0, 0, spinSpeed) * Time.deltaTime * 60);
            transform.position -= transform.right * runSpeed * Time.deltaTime * 60;
        }
        else if (timer < treeLength * 2 + timeDown + timeUp)
        {
            transform.position = Vector3.Lerp(transform.position, startPosition, 0.1f * Time.deltaTime * 60);
            transform.rotation = Quaternion.Lerp(transform.rotation, startRotation, 0.1f * Time.deltaTime * 60);
        }
        else if (timer >= treeLength*2 + timeDown + timeUp)
        {
            turned = false;
            timer = 0;
            
            //transform.Rotate(new Vector3(180, 0, 0));
        }
	
	}
}
