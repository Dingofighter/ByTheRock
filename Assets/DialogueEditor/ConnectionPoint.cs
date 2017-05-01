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

    public int index;

    public int startOffset;

#if UNITY_EDITOR
    public GUIStyle style;
    public Action<ConnectionPoint> OnClickConnectionPoint;

    public ConnectionPoint(Node node, ConnectionPointType type, GUIStyle style, Action<ConnectionPoint> OnClickConnectionPoint, int index, int startOffset)
    {
        this.node = node;
        this.type = type;
        this.style = style;
        this.OnClickConnectionPoint = OnClickConnectionPoint;
        this.index = index;
        this.startOffset = startOffset;
        rect = new Rect(0, startOffset, 10f, 20f);
    }

    public void Draw()
    {
        rect.y = (node.rect.y + rect.height) + startOffset + index * offset;

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
#endif
}