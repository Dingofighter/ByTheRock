using UnityEngine;
using System.Collections;

public class WallTimer : MonoBehaviour {

    float timer = 0;
    public bool start = false;
    public GameObject trigger;
    public GameObject Bear;
	
	// Update is called once per frame
	void Update ()
    {
        if (!AllFlags.Instance.flags[3].value)
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
        else
            Destroy(Bear);
            

    }
}
