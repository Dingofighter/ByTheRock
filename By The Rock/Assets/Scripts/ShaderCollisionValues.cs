using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShaderCollisionValues : MonoBehaviour {

    private Transform player;

	// Use this for initialization
	void Start () {
        player = FindObjectOfType<PlayerController>().transform;
	}
	
	// Update is called once per frame
	void Update () {
        Shader.SetGlobalVector("_PlayerPos", new Vector4(player.position.x, player.position.y + 1, player.position.z, 0));
	}
}
