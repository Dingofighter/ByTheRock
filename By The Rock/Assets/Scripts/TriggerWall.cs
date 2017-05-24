using UnityEngine;
using System.Collections;

public class TriggerWall : MonoBehaviour {

    public GameObject invisibleWall;

	void OnTriggerEnter(Collider c)
    {
        if (c.tag == "Player")
        {
            invisibleWall.gameObject.SetActive(true);
        }
    }
}
