using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class StartNode : Node {

    public static int width = 200;
    public static int defaultHeight = 70;
    // Space between left side / top of box and contents of node
    public static int padding = 15;
    // Amount of height to add when adding inputs
    public static int heightIncrease = 55;

    public int currentHeight = defaultHeight;

    public GUIStyle defaultInPointStyle;

    public Action<ConnectionPoint> defaultOnClickInPoint;

    // Variables displayed in node


    public void Init(int id, Vector2 position, int width, int height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode)
    {
        this.id = id;
        title = "Start Node";
        currentHeight = height;
        rect = new Rect(position.x, position.y, width, height);
        style = nodeStyle;
        outPoints.Add(new ConnectionPoint(this, ConnectionPoint.ConnectionPointType.Out, outPointStyle, OnClickOutPoint, 0, 0));
        defaultNodeStyle = nodeStyle;
        selectedNodeStyle = selectedStyle;
        OnRemoveNode = OnClickRemoveNode;
    }

    public void Load()
    {

    }

    public override void Draw()
    {
        foreach (ConnectionPoint connectionPoint in inPoints)
        {
            connectionPoint.Draw();
        }
        foreach (ConnectionPoint connectionPoint in outPoints)
        {
            connectionPoint.Draw();
        }
        GUI.Box(rect, title, style);

        GUILayout.BeginArea(new Rect(rect.position.x + padding, rect.position.y + padding, width - padding * 2, currentHeight - padding * 2));
        // Make text inside textArea wrap when reaching edge
        EditorStyles.textArea.wordWrap = true;

        GUIStyle centerTextStyle = new GUIStyle(GUI.skin.textField);
        centerTextStyle.alignment = TextAnchor.MiddleCenter;
        /* Specify contents of node here */
        EditorGUILayout.TextField("ID: " + id.ToString(), centerTextStyle);

        GUILayout.EndArea();
    }

    /* BASE CLASS FUNCTIONS */
    public override void Drag(Vector2 delta)
    {
        rect.position += delta;
    }

    public override bool ProcessEvents(Event e)
    {
        switch (e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    if (rect.Contains(e.mousePosition))
                    {
                        isDragged = true;
                        GUI.changed = true;
                        isSelected = true;
                        style = selectedNodeStyle;
                    }
                    else
                    {
                        GUI.changed = true;
                        isSelected = false;
                        style = defaultNodeStyle;
                    }
                }

                if (e.button == 1 && isSelected && rect.Contains(e.mousePosition))
                {
                    ProcessContextMenu();
                    e.Use();
                }
                break;

            case EventType.MouseUp:
                isDragged = false;
                break;

            case EventType.MouseDrag:
                if (e.button == 0 && isDragged)
                {
                    Drag(e.delta);
                    e.Use();
                    return true;
                }
                break;
        }

        return false;
    }

    public override void ProcessContextMenu()
    {
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Remove Node"), false, () => OnClickRemoveNode());
        genericMenu.ShowAsContext();
    }

    public override void OnClickRemoveNode()
    {
        if (OnRemoveNode != null)
        {
            OnRemoveNode(this);
        }
    }
}