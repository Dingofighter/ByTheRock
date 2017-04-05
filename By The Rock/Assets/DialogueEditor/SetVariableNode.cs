using UnityEngine;
using UnityEditor;
using System;

[Serializable]
public class SetVariableNode : Node {

    public static int width = 250;
    public static int defaultHeight = 220;
    // Space between left side / top of box and contents of node
    public static int padding = 15;
    // Amount of height to add when adding inputs
    public static int heightIncrease = 55;

    public int currentHeight = defaultHeight;

    public GUIStyle defaultInPointStyle;

    public Action<ConnectionPoint> defaultOnClickInPoint;

    public string[] variableTypes = { "Attribute", "Bool" };
    public string[] attributes = { "Aggressive", "Kind" };
    public string[] intModifiers = { "+", "-", "=" };

    public string[] bools = { "TestBool1", "TestBool2" };
    public string[] boolValues = { "True", "False" };

    // Variables displayed in node
    public int variableTypeIndex = 0;
    public bool valueComparison = false;
    public int attributeIndex = 0;
    public int intModifierIndex = 0;
    public string intValueTwo = "";
    public int attributeIndexTwo = 0;

    public int boolIndex = 0;
    public int boolValueIndex = 0;

    public void Init(int id, Vector2 position, int width, int height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode)
    {
        this.id = id;
        title = "Set Variable Node";
        currentHeight = height;
        rect = new Rect(position.x, position.y, width, height);
        style = nodeStyle;
        inPoints.Add(new ConnectionPoint(this, ConnectionPoint.ConnectionPointType.In, inPointStyle, OnClickInPoint, 0, 0));
        outPoints.Add(new ConnectionPoint(this, ConnectionPoint.ConnectionPointType.Out, outPointStyle, OnClickOutPoint, 0, 0));
        defaultInPointStyle = inPointStyle;
        defaultNodeStyle = nodeStyle;
        selectedNodeStyle = selectedStyle;
        defaultOnClickInPoint = OnClickInPoint;
        OnRemoveNode = OnClickRemoveNode;
    }

    public void Load(int inPoints)
    {
        for (int i = 0; i < inPoints - 1; i++)
        {
            AddInPoint();
        }
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

        EditorGUILayout.LabelField("Variable Type");
        variableTypeIndex = EditorGUILayout.Popup(variableTypeIndex, variableTypes);

        if (variableTypeIndex == 0)
        {
            GUILayout.BeginHorizontal();

            attributeIndex = EditorGUILayout.Popup(attributeIndex, attributes);

            intModifierIndex = EditorGUILayout.Popup(intModifierIndex, intModifiers);

            intValueTwo = EditorGUILayout.TextField(intValueTwo);

            GUILayout.EndHorizontal();
        }
        else if (variableTypeIndex == 1)
        {
            GUILayout.BeginHorizontal();

            boolIndex = EditorGUILayout.Popup(boolIndex, bools);
            boolValueIndex = EditorGUILayout.Popup(boolValueIndex, boolValues);

            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Add input"))
        {
            AddInPoint();
        }

        GUILayout.EndArea();
    }

    public void AddInPoint()
    {
        inPoints.Add(new ConnectionPoint(this, ConnectionPoint.ConnectionPointType.In, defaultInPointStyle, defaultOnClickInPoint, inPoints.Count, 0));

        // Increase height of node when inputs would start showing beyond node bounds
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