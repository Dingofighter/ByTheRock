using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

[Serializable]
public class Connection : ScriptableObject{

    public ConnectionPoint inPoint;
    public ConnectionPoint outPoint;

#if UNITY_EDITOR
    public Action<Connection> OnClickRemoveConnection;

    public void OnEnable()
    {
    }

    // Init is used when creating a connections for the first time
    public void Init(ConnectionPoint inPoint, ConnectionPoint outPoint, Action<Connection> OnClickRemoveConnection)
    {
        this.inPoint = inPoint;
        this.outPoint = outPoint;
        this.OnClickRemoveConnection = OnClickRemoveConnection;
    }

    // Load is used when recreating a connection when loading a saved dialogue
    public void Load(Node inPointNode, Node outPointNode, int inPointIndex, int outPointIndex, Action<Connection> OnClickRemoveConnection)
    {
        inPoint = inPointNode.inPoints[inPointIndex];
        outPoint = outPointNode.outPoints[outPointIndex];
        this.OnClickRemoveConnection = OnClickRemoveConnection;
    }

    public void Draw()
    {
        Handles.DrawBezier(
            inPoint.rect.center,
            outPoint.rect.center,
            inPoint.rect.center + Vector2.left * 50f,
            outPoint.rect.center + Vector2.right * 50f,
            Color.white,
            null,
            2f);

        // Square button used to remove connection
        if (Handles.Button((inPoint.rect.center + outPoint.rect.center) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleCap))
        {
            if (OnClickRemoveConnection != null)
            {
                OnClickRemoveConnection(this);
            }
        }
    }
#endif
}
