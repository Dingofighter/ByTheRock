using UnityEngine;
using System.Collections;

public class OrmbunkeSetup : MonoBehaviour {

    DynamicBone db;

	// Use this for initialization
	void Start () {
        db = GetComponent<DynamicBone>();
        PlayerController ougrah = FindObjectOfType<PlayerController>();
        db.m_Colliders.Add(ougrah.GetComponentInChildren<DynamicBoneCollider>());
        db.m_ReferenceObject = ougrah.transform;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
