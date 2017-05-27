using UnityEngine;
using System.Collections;

public class WallTimer : MonoBehaviour {

    float timer = 0;
    public bool start = false;
    public GameObject trigger;
	
	// Update is called once per frame
	void Update ()
    {
	    if (start)
        {
            timer += 1 * Time.deltaTime;
            trigger.SetActive(false);
        }
        else
        {
            timer = 0;
            
        }

        if (timer >= 5)
        {
            trigger.SetActive(true);
            start = false;
        }

	}
}
