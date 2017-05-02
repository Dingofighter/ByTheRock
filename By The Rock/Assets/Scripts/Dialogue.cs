using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Dialogue : MonoBehaviour {

    public DialogueContainer dialogue;

    public Dictionary<int, Node> nodes;

    public int numFlagsRequired;

    public bool walkAndTalk;

    public List<int> boolIndex;
    public List<int> boolValueIndex;

    // Use this for initialization
    void Start () {
        if (boolIndex == null)
        {
            boolIndex = new List<int>();
        }
        if (boolValueIndex == null)
        {
            boolValueIndex = new List<int>();
        }
        
        if (nodes == null)
        {
            nodes = new Dictionary<int, Node>();
        }
        
        foreach(Node node in dialogue.nodes)
        {
            Debug.Log("adding node " + gameObject.name);
            nodes.Add(node.id, node);
            Debug.Log(nodes.Count);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public Node GetNode(int nodeID)
    {
        if (nodes == null)
        {
            Debug.Log("OH NO " + gameObject.name);
        }

        return nodes[nodeID];
    }

    public int GetLength()
    {
        return nodes.Count;
    }
}
