using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class DialogueContainer : ScriptableObject {

    public List<Node> nodes;
    public List<Connection> connections;

    public void OnEnable()
    {
        if (nodes == null)
        {
            nodes = new List<Node>();
        }
        if (connections == null)
        {
            connections = new List<Connection>();
        }
    }
}
