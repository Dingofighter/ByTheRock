using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    Vector3 target1;
    Vector3 target2;
    bool switchTarget;
    int counter;

	// Use this for initialization
	void Start () {

        target1 = new Vector3(transform.position.x + 50, transform.position.y, transform.position.z - 10);
        target2 = new Vector3(transform.position.x - 10, transform.position.y, transform.position.z + 50);

    }
	
	// Update is called once per frame
	void Update () {

        counter++;
        if (counter > 100)
        {
            switchTarget = !switchTarget;
            counter = 0;
        }

        if (switchTarget) transform.position = Vector3.MoveTowards(transform.position, target1, 2);
        else transform.position = Vector3.MoveTowards(transform.position, target2, 2);
	
	}
}
