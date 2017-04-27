using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderCollisionValues : MonoBehaviour {

    public Transform player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Shader.SetGlobalVector("_PlayerPos", new Vector4(player.position.x, player.position.y, player.position.z, 0));
	}
}
