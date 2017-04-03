using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

[Serializable]
public class PlayerChoiceNode : Node {

    public static int width = 200;
    public static int defaultHeight = 230;
    // Space between left side / top of box and contents of node
    public static int padding = 15;
    // Amount of height to add when adding inputs/outputs
    public static int heightIncrease = 55;

    public int currentHeight = defaultHeight;

    public GUIStyle defaultInPointStyle;
    public GUIStyle defaultOutPointStyle;

    public Action<ConnectionPoint> defaultOnClickInPoint;
    public Action<ConnectionPoint> defaultOnClickOutPoint;

    // Variables displayed in node
    public List<string> optionLines = new List<string>();

    public void Init(int id, Vector2 position, int width, int height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode)
    {
        this.id = id;
        title = "Player Choice Node";
        rect = new Rect(position.x, position.y, width, height);
        style = nodeStyle;
        inPoints.Add(new ConnectionPoint(this, ConnectionPoint.ConnectionPointType.In, inPointStyle, OnClickInPoint, 0));
        outPoints.Add(new ConnectionPoint(this, ConnectionPoint.ConnectionPointType.Out, outPointStyle, OnClickOutPoint, 0));
        defaultInPointStyle = inPointStyle;
        defaultOutPointStyle = outPointStyle;
        defaultNodeStyle = nodeStyle;
        selectedNodeStyle = selectedStyle;
        defaultOnClickInPoint = OnClickInPoint;
        defaultOnClickOutPoint = OnClickOutPoint;
        OnRemoveNode = OnClickRemoveNode;

        optionLines.Add("Option " + optionLines.Count);
    }

    public void Load(List<String> optionLines, int inPoints, int outPoints)
    {
        for (int i = 0; i < inPoints - 1; i++)
        {
            AddInPoint();
        }
        for (int i = 0; i < outPoints - 1; i++)
        {
            AddOutPoint();
        }

        this.optionLines = optionLines;
    }

    public override void Draw()
    {
        foreach (ConnectionPoint connectionPoint in inPoints)
        {
            connectionPoint.Draw();
        }
        foreach(ConnectionPoint connectionPoint in outPoints)
        {
            connectionPoint.Draw();
        }
        GUI.Box(rect, title, style);

        GUILayout.BeginArea(new Rect(rect.position.x + padding, rect.position.y + padding, width - padding * 2, currentHeight));
        // Make text inside textArea wrap when reaching edge
        EditorStyles.textArea.wordWrap = true;

        GUIStyle centerTextStyle = new GUIStyle(GUI.skin.textField);
        centerTextStyle.alignment = TextAnchor.MiddleCenter;
        /* Specify contents of node here */
        EditorGUILayout.TextField("ID: " + id.ToString(), centerTextStyle);

        for (int i = 0; i < optionLines.Count; i++)
        {            optionLines[i] = EditorGUILayout.TextArea(optionLines[i], GUILayout.Height(50), GUILayout.ExpandHeight(false));
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add input"))
        {
            AddInPoint();
        }
        if (GUILayout.Button("Add option"))
        {
            AddOption();
        }
        GUILayout.EndHorizontal();

        GUILayout.EndArea();
    }

    public void AddOption()
    {
        optionLines.Add("Option " + optionLines.Count);

        AddOutPoint();
    }

    public void AddInPoint()
    {
        inPoints.Add(new ConnectionPoint(this, ConnectionPoint.ConnectionPointType.In, defaultInPointStyle, defaultOnClickInPoint, inPoints.Count));

        // Increase height of node when inputs would start showing beyond node bounds
        if (Mathf.Max(inPoints.Count, outPoints.Count) > 3)
        {
            currentHeight = defaultHeight + (Mathf.Max(inPoints.Count, outPoints.Count) - 3) * heightIncrease;

            rect = new Rect(rect.position.x, rect.position.y, width, currentHeight);
        }
    }

    public void AddOutPoint()
    {
        outPoints.Add(new ConnectionPoint(this, ConnectionPoint.ConnectionPointType.Out, defaultOutPointStyle, defaultOnClickOutPoint, outPoints.Count));

        // Increase height of node when outputs would start showing beyond node bounds
        if (Mathf.Max(inPoints.Count, outPoints.Count) > 3)
        {
            currentHeight = defaultHeight + (Mathf.Max(inPoints.Count, outPoints.Count) - 3) * heightIncrease;

            rect = new Rect(rect.position.x, rect.position.y, width, currentHeight);
        }
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