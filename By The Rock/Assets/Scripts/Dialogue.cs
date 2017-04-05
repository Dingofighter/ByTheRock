using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dialogue : MonoBehaviour {

    public DialogueContainer dialogue;

    public Dictionary<int, Node> nodes;

	// Use this for initialization
	void Start () {
        if (nodes == null)
        {
            nodes = new Dictionary<int, Node>();
        }

        foreach(Node node in dialogue.nodes)
        {
            nodes.Add(node.id, node);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Node GetNode(int nodeID)
    {
        return nodes[nodeID];
    }

    public int GetLength()
    {
        return nodes.Count;
    }
}
