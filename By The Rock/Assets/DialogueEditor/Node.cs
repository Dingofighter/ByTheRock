using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public abstract class Node : ScriptableObject {

    public Rect rect;
    public string title = "NODE";
    public int id;
    public bool isDragged;
    public bool isSelected;

    public List<ConnectionPoint> inPoints = new List<ConnectionPoint>();
    public List<ConnectionPoint> outPoints = new List<ConnectionPoint>();

    public GUIStyle style;
    public GUIStyle defaultNodeStyle;
    public GUIStyle selectedNodeStyle;

    public Action<Node> OnRemoveNode;

    public List<int> nextNodesID;

    public void OnEnable()
    {
        if (nextNodesID == null)
        {
            nextNodesID = new List<int>();
        }
    }

    // All methods are abstract to avoid issues with serialization
    public abstract void Draw();

    public abstract void Drag(Vector2 delta);

    public abstract bool ProcessEvents(Event e);

    public abstract void ProcessContextMenu();

    public abstract void OnClickRemoveNode();
}