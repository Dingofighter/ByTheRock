using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

[Serializable]
public class CheckVariableNode : Node {

    public static int width = 250;
    public static int defaultHeight = 220;
    // Space between left side / top of box and contents of node
    public static int padding = 15;
    // Amount of height to add when adding inputs
    public static int heightIncrease = 55;
    // Offset to add to outpoints
    public static int outPointOffset = 115;

    public int currentHeight = defaultHeight;

    public string[] variableTypes = { "Attribute", "Bool" };
    public string[] attributes = { "Aggressive", "Kind" };
    public string[] intComparators = { ">", "<", "==", "!=", ">=", "<=" };

    public string[] bools = { "TestBool1", "TestBool2" };

    // Variables displayed in node
    public int variableTypeIndex = 1;
    public bool valueComparison = false;
    public int attributeIndex = 0;
    public int intComparatorIndex = 0;
    public string intValueTwo = "";
    public int attributeIndexTwo = 0;

    public int boolIndex = 0;
    public int boolComparatorIndex = 0;

#if UNITY_EDITOR
    public GUIStyle defaultInPointStyle;

    public Action<ConnectionPoint> defaultOnClickInPoint;

    public void Init(int id, Vector2 position, int width, int height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode)
    {
        this.id = id;
        title = "Check Variable Node";
        currentHeight = height;
        rect = new Rect(position.x, position.y, width, height);
        style = nodeStyle;
        inPoints.Add(new ConnectionPoint(this, ConnectionPoint.ConnectionPointType.In, inPointStyle, OnClickInPoint, 0, 0));
        outPoints.Add(new ConnectionPoint(this, ConnectionPoint.ConnectionPointType.Out, outPointStyle, OnClickOutPoint, 0, outPointOffset));
        outPoints.Add(new ConnectionPoint(this, ConnectionPoint.ConnectionPointType.Out, outPointStyle, OnClickOutPoint, 1, outPointOffset));
        defaultInPointStyle = inPointStyle;
        defaultNodeStyle = nodeStyle;
        selectedNodeStyle = selectedStyle;
        defaultOnClickInPoint = OnClickInPoint;
        OnRemoveNode = OnClickRemoveNode;

        // Load all flags from AllFlags asset into bools list
        AllFlags allFlags = (AllFlags) AssetDatabase.LoadAssetAtPath("Assets/Resources/AllFlags.asset", typeof(AllFlags));
        bools = new string[allFlags.flags.Count];
        for(int i = 0; i < bools.Length; i++)
        {
            bools[i] = allFlags.flags[i].description;
        }
    }

    public void Load(int boolIndex, int inPoints, string bank, int day, int clip, int chara)
    {
        this.boolIndex = boolIndex;

        for (int i = 0; i < inPoints - 1; i++)
        {
            AddInPoint();
        }

        this.DayBank = bank;
        this.Day = day;
        this.Clip = clip;
        this.Char = chara;
        this.Fmod.CharSelect = chara;
        this.Fmod.DiaSelect = day;
        this.Fmod.VoxSelect = clip;
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

        GUIStyle labelRight = new GUIStyle(GUI.skin.label);
        labelRight.alignment = TextAnchor.MiddleRight;

        GUIStyle centerTextStyle = new GUIStyle(GUI.skin.textField);
        centerTextStyle.alignment = TextAnchor.MiddleCenter;
        /* Specify contents of node here */
        EditorGUILayout.TextField("ID: " + id.ToString(), centerTextStyle);

        EditorGUILayout.LabelField("Variable Type");
        variableTypeIndex = EditorGUILayout.Popup(variableTypeIndex, variableTypes);

        if (variableTypeIndex == 0)
        {
            valueComparison = EditorGUILayout.Toggle("Int/Attribute comparison", valueComparison);
            GUILayout.BeginHorizontal();

            attributeIndex = EditorGUILayout.Popup(attributeIndex, attributes);

            intComparatorIndex = EditorGUILayout.Popup(intComparatorIndex, intComparators);

            if (valueComparison)
            {
                intValueTwo = EditorGUILayout.TextField(intValueTwo);
            }
            else
            {
                attributeIndexTwo = EditorGUILayout.Popup(attributeIndexTwo, attributes);
            }

            GUILayout.EndHorizontal();
        }
        else if (variableTypeIndex == 1)
        {
            EditorGUILayout.LabelField("Bool to check:");

            boolIndex = EditorGUILayout.Popup(boolIndex, bools);
        }

        if (GUILayout.Button("Add input"))
        {
            AddInPoint();
        }

        GUILayout.Space(6);

        EditorGUILayout.LabelField("IF TRUE", labelRight);

        GUILayout.Space(35);

        EditorGUILayout.LabelField("IF FALSE", labelRight);

        GUILayout.EndArea();

        Fmod.FMODAddon(rect, "", style, currentHeight);
        DayBank = Fmod.setEvent();
        Char = Fmod.setChar();
        Day = Fmod.setDialogue();
        Clip = Fmod.setClip();
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
#endif
}
/*
#else
    public override void OnClickRemoveNode(){}
    public override void ProcessContextMenu(){}
    public override bool ProcessEvents(Event e){ return true; }
    public override void Drag(Vector2 delta){}
    public override void Draw(){}
*/