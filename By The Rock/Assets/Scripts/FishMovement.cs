using UnityEngine;
using System.Collections;

public class FishMovement : MonoBehaviour {

    public int rotateSpeed;
    public float moveSpeed;
    public float waterHeight;

    public ParticleSystem splash;

    int timer;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {

        if (GameManager.instance.paused) return;

        //transform.position += transform.right * 0.1f;

        //transform.position = new Vector3(transform.position.x, transform.position.y + 0.2f * Mathf.Sin(Time.time * 3), transform.position.z);

        timer++;
        if (timer == 50)
        {
            //splash.Play();
            timer = 0;
        }
        
        
        //transform.position -= transform.forward * 0.2f * Mathf.Sin(Time.time * 3);
        transform.Rotate(-Vector3.forward * Time.deltaTime*rotateSpeed);


        //Debug.Log(transform.rotation.eulerAngles);
        //Debug.Log(splash.transform.rotation.eulerAngles);
        //Debug.Log(transform.position.y);

        // if (transform.rotation.eulerAngles.x > 315 && transform.rotation.eulerAngles.x < 325)
        if (transform.position.y > (waterHeight - 0.1) && transform.position.y < (waterHeight + 0.1))
        {

            splash.Play();

        }

        //transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.y+100, transform.rotation.z, transform.rotation.w);
        transform.position += transform.right* moveSpeed * Time.deltaTime * 60;
       // transform.rotation

    }
}
