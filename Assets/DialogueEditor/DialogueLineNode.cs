using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

[Serializable]
public class DialogueLineNode : Node {

    public static int width = 200;
    public static int defaultHeight = 180;
    // Space between left side / top of box and contents of node
    public static int padding = 15;
    // Amount of height to add when adding inputs
    public static int heightIncrease = 55;

    public int currentHeight = defaultHeight;

    // Variables displayed in node
    public string actorName = "Name";
    public string dialogueLine = "Line";

    

#if UNITY_EDITOR
    public GUIStyle defaultInPointStyle;

    public Action<ConnectionPoint> defaultOnClickInPoint;

    public void Init(int id, Vector2 position, int width, int height, GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, Action<ConnectionPoint> OnClickInPoint, Action<ConnectionPoint> OnClickOutPoint, Action<Node> OnClickRemoveNode)
    {
        this.id = id;
        title = "Dialogue Line Node";
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

    public void Load(string actorName, string dialogueLine, int inPoints, string bank, int day, int clip, int chara, int yes)
    {
        this.actorName = actorName;
        this.dialogueLine = dialogueLine;
        for (int i = 0; i < inPoints - 1; i++)
        {
            AddInPoint();
        }
        this.DayBank = bank;
        this.Day = day;
        this.Clip = clip;
        this.Char = chara;
        this._fmod = yes;
        int daynr = 1;

        if (bank == "{adc96b33-ea8e-4527-a960-14fa573a4b8e}" || bank == "")
            daynr = 1;




        this.Fmod.DaySelect = daynr;
        this.Fmod.CharSelect = chara;
        this.Fmod.DiaSelect = day;
        this.Fmod.VoxSelect = clip;
        if (_fmod > 0)
            this.Fmod.fmod = true;
        else
            this.Fmod.fmod = false;
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
        EditorStyles.textField.wordWrap = true;

        GUIStyle centerTextStyle = new GUIStyle(GUI.skin.textField);
        centerTextStyle.alignment = TextAnchor.MiddleCenter;
        /* Specify contents of node here */
        EditorGUILayout.TextField("ID: " + id.ToString(), centerTextStyle);

        EditorGUILayout.LabelField("ACTOR NAME");
        actorName = EditorGUILayout.TextField(actorName);

        EditorGUILayout.LabelField("DIALOGUE LINE");
        dialogueLine = EditorGUILayout.TextArea(dialogueLine, GUILayout.Height(50), GUILayout.ExpandHeight(false));

        if (GUILayout.Button("Add input"))
        {
            AddInPoint();
        }

        

        GUILayout.EndArea();

        

        Fmod.FMODAddon(rect, "", style, currentHeight);
        DayBank = Fmod.setEvent();
        Char = Fmod.setChar();
        Day = Fmod.setDialogue();
        Clip = Fmod.setClip();
        _fmod = Fmod.getBool();
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