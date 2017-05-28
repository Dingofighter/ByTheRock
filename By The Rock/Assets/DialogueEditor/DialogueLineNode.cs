using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

[Serializable]
public class DialogueLineNode : Node {

    public static int width = 200;
    public static int defaultHeight = 220;
    // Space between left side / top of box and contents of node
    public static int padding = 15;
    // Amount of height to add when adding inputs
    public static int heightIncrease = 55;

    public int currentHeight = defaultHeight;

    // Variables displayed in node
    public string actorName = "Name";
    public string dialogueLine = "Line";
    public bool unskippable = false;

    public int animNR;
    public string[] animations = { "None", "Talk", "Hands", "Give" };

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

    public void Load(string actorName, string dialogueLine, bool unskippable, int inPoints, int day, int clip, int chara, int yes, int dia, int animNR)
    {
        this.actorName = actorName;
        this.dialogueLine = dialogueLine;
        this.unskippable = unskippable;
        for (int i = 0; i < inPoints - 1; i++)
        {
            AddInPoint();
        }

        this.Day = day;
        this.Clip = clip;
        this.Char = chara;
        this._fmod = yes;
        this.Dia = dia;

        if (Day < 1)
            Day = 1;

        this.Fmod.DaySelect = Day;
        this.Fmod.CharSelect = chara;
        this.Fmod.DiaSelect = dia;
        this.Fmod.VoxSelect = clip;
        if (_fmod > 0)
            this.Fmod.fmod = true;
        else
            this.Fmod.fmod = false;

        this.animNR = animNR;

        

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
        
        unskippable = EditorGUILayout.Toggle("Unskippable", unskippable);

        EditorGUILayout.LabelField("ACTOR NAME");
        actorName = EditorGUILayout.TextField(actorName);

        EditorGUILayout.LabelField("DIALOGUE LINE");
        dialogueLine = EditorGUILayout.TextArea(dialogueLine, GUILayout.Height(50), GUILayout.ExpandHeight(false));

        animNR = EditorGUILayout.Popup(animNR, animations);

        if (GUILayout.Button("Add input"))
        {
            AddInPoint();
        }

        GUILayout.EndArea();

        Fmod.FMODAddon(rect, "", style, currentHeight);
        Day = Fmod.setDay();
        Char = Fmod.setChar();
        Dial = Fmod.setDialogue();
        Clip = Fmod.setClip();
        _fmod = Fmod.getBool();
        Dia = Fmod.setDialogue();
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
