using UnityEngine;
using System;

[Serializable]
public class ConnectionPoint {

    public enum ConnectionPointType { In, Out }

    // Amount of space to add between inpoints/outpoints
    public static int offset = 52;

    public Rect rect;

    public ConnectionPointType type;

    public Node node;

    public GUIStyle style;

    public Action<ConnectionPoint> OnClickConnectionPoint;

    public int index;
    
    public ConnectionPoint(Node node, ConnectionPointType type, GUIStyle style, Action<ConnectionPoint> OnClickConnectionPoint, int index)
    {
        this.node = node;
        this.type = type;
        this.style = style;
        this.OnClickConnectionPoint = OnClickConnectionPoint;
        this.index = index;
        rect = new Rect(0, 0, 10f, 20f);
    }

    public void Draw()
    {
        rect.y = (node.rect.y + rect.height) + index * offset;

        switch(type)
        {
            case ConnectionPointType.In:
                rect.x = node.rect.x - rect.width + 8f;
                break;

            case ConnectionPointType.Out:
                rect.x = node.rect.x + node.rect.width - 8f;
                break;
        }

        if (GUI.Button(rect, "", style))
        {
            if (OnClickConnectionPoint != null)
            {
                OnClickConnectionPoint(this);
            }
        }
    }
}
