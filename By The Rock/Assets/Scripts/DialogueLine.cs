using UnityEngine;
using System.Collections;
using System;

public class DialogueLine : MonoBehaviour{

    public enum Type { CharacterLine, PlayerLine };

    public Type nodeType;
    public string name;
    public string line;
    public string player;
    public int nextLine;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
