using UnityEngine;
using System.Collections;

public class particleHandlerRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.eulerAngles = new Vector3(300, 50, 0);
	
	}
}
