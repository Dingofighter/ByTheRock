#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class NodeBasedEditor : EditorWindow {

    // Container used for saving and loading
    private DialogueContainer dialogue;
    private BoolList boolList;

    private List<Node> nodes = new List<Node>();
    private List<Connection> connections = new List<Connection>();

    private Rect menuBar;

    private GUIStyle nodeStyle;
    private GUIStyle selectedNodeStyle;
    private GUIStyle inPointStyle;
    private GUIStyle outPointStyle;

    private ConnectionPoint selectedInPoint;
    private ConnectionPoint selectedOutPoint;

    private Vector2 offset;
    private Vector2 drag;

    private float menuBarHeight = 20f;
    private int currentHighestID = 0;
    private string lastSavedName = "DialogueName";

    // Show in Unity menu
	[MenuItem("Window/Dialogue Editor")]
    private static void OpenWindow()
    {
        // Create editor window and set title
        NodeBasedEditor window = GetWindow<NodeBasedEditor>();
        window.titleContent = new GUIContent("Dialogue Editor");
    }

    private void OnEnable()
    {
        // Create graphical style for nodes
        nodeStyle = new GUIStyle();
        nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
        nodeStyle.border = new RectOffset(12, 12, 12, 12);

        // Create graphical style for selected node
        selectedNodeStyle = new GUIStyle();
        selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
        selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

        // Create graphical style for input points
        inPointStyle = new GUIStyle();
        inPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
        inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
        inPointStyle.border = new RectOffset(4, 4, 12, 12);

        // Create graphical style for output points
        outPointStyle = new GUIStyle();
        outPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
        outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
        outPointStyle.border = new RectOffset(4, 4, 12, 12);
    }

    private void OnGUI()
    {
        // Draw two differently sized grids
        DrawGrid(20, 0.2f, Color.gray);
        DrawGrid(100, 0.4f, Color.gray);

        // Draw all nodes and connections between nodes
        DrawNodes();
        DrawConnections();

        // Draw connection line between selected in/outpoint and current mouse position
        DrawConnectionLine(Event.current);

        // Draw top menubar on top of everything else
        DrawMenuBar();

        // Process user events for nodes and editor
        ProcessNodeEvents(Event.current);
        ProcessEvents(Event.current);

        // If any change has been made in the editor, repaint to show result of changes
        if (GUI.changed)
        {
            Repaint();
        }
    }

    private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
    {
        int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
        int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

        Handles.BeginGUI();
        Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

        offset += drag * 0.5f;
        Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

        for (int i = 0; i < widthDivs; i++)
        {
            Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0) + newOffset);
        }

        for (int j = 0; j < heightDivs; j++)
        {
            Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0) + newOffset);
        }

        Handles.color = Color.white;
        Handles.EndGUI();
    }

    private void DrawNodes()
    {
        if (nodes != null)
        {
            foreach(Node node in nodes)
            {
                node.Draw();
            }
        }
    }

    private void DrawConnections()
    {
        if (connections != null)
        {
            foreach (Connection connection in connections)
            {
                connection.Draw();
            }
        }
    }

    private void DrawConnectionLine(Event e)
    {
        // Draw line between selected inpoint and mouse if no outpoint is selected
        if (selectedInPoint != null && selectedOutPoint == null)
        {
            Handles.DrawBezier(
                selectedInPoint.rect.center,
                e.mousePosition,
                selectedInPoint.rect.center + Vector2.left * 50f,
                e.mousePosition - Vector2.left * 50f,
                Color.white,
                null,
                2f);

            GUI.changed = true;
        }

        // Draw line between selected outpoint and mouse if no inpoint is selected
        if (selectedOutPoint != null && selectedInPoint == null)
        {
            Handles.DrawBezier(
                selectedOutPoint.rect.center,
                e.mousePosition,
                selectedOutPoint.rect.center + Vector2.right * 50f,
                e.mousePosition - Vector2.right * 50f,
                Color.white,
                null,
                2f);

            GUI.changed = true;
        }
    }

    private void DrawMenuBar()
    {
        menuBar = new Rect(0, 0, position.width, menuBarHeight);

        GUILayout.BeginArea(menuBar, EditorStyles.toolbar);
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Save", EditorStyles.toolbarButton))
        {
            Save();
        }
        if (GUILayout.Button("Load", EditorStyles.toolbarButton))
        {
            Load();
        }
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Edit Flags", EditorStyles.toolbarButton))
        {
            Selection.activeObject = Resources.Load("AllFlags");
        }

        GUILayout.EndHorizontal();
        GUILayout.EndArea();
    }

    private void Save()
    {
        // Show file browser
        string assetPath = EditorUtility.SaveFilePanel("Select asset", "Assets/Resources/Dialogues", lastSavedName, "asset");
        if (assetPath != "")
        {
            // Make selected path relative instead of absolute
            assetPath = assetPath.Replace(Application.dataPath, "Assets");
            string[] splitAssetPath = assetPath.Split('/');
            lastSavedName = splitAssetPath[splitAssetPath.Length - 1];
        }
        else
        {
            return;
        }

        // Try to load asset at specified path
        dialogue = (DialogueContainer)AssetDatabase.LoadAssetAtPath(assetPath, typeof(DialogueContainer));

        if (dialogue != null)
        {
            // If asset exists, delete all sub-assets and clear lists
            Object[] subAssets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
            for (int i = 0; i < subAssets.Length; i++)
            {
                if (subAssets[i] is DialogueContainer)
                {
                    // Don't destroy DialogueContainer
                }
                else
                {
                    DestroyImmediate(subAssets[i], true);
                }
            }

            dialogue.nodes.Clear();
            dialogue.connections.Clear();
        }
        else
        {
            // Create an instance of container class
            dialogue = CreateInstance<DialogueContainer>();
            // Create an asset file from container class instance
            AssetDatabase.CreateAsset(dialogue, assetPath);
        }
        
        // Loop through all nodes and connections in current dialogue and add them as subassets to created asset file
        foreach (Node node in nodes)
        {
            // Create entry in nextNodesID list for each outPoint in node
            for (int i = 0; i < node.outPoints.Count; i++)
            {
                node.nextNodesID.Add(-1);
            }
            node.name = "zzzNode";
            AssetDatabase.AddObjectToAsset(node, dialogue);
            // Reimport immediately to make sure changes are shown
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(node));

            dialogue.nodes.Add(node);
        }
        foreach (Connection connection in connections)
        {
            // Set value of nextNodeID to id of connected node
            connection.outPoint.node.nextNodesID[connection.outPoint.index] = connection.inPoint.node.id;
            connection.name = "zzzConnection";
            AssetDatabase.AddObjectToAsset(connection, dialogue);
            // Reimport immediately to make sure changes are shown
            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(connection));
            dialogue.connections.Add(connection);
        }

        EditorUtility.SetDirty(dialogue);

        // Immediately load newly created save to avoid any further changes made to existing nodes being automatically saved
        LoadSpecific(assetPath);
    }

    private void Load()
    {
        // Show file browser
        string assetPath = EditorUtility.OpenFilePanel("Select asset", "Assets/Resources/Dialogues", "asset");
        if (assetPath != "")
        {
            // Make selected path relative instead of absolute
            assetPath = assetPath.Replace(Application.dataPath, "Assets");
            string[] splitAssetPath = assetPath.Split('/');
            lastSavedName = splitAssetPath[splitAssetPath.Length - 1];
        }
        else
        {
            return;
        }
        // Load selected asset into container class
        dialogue = (DialogueContainer)AssetDatabase.LoadAssetAtPath(assetPath, typeof(DialogueContainer));
        // Clear nodes and connections of current dialogue
        nodes.Clear();
        connections.Clear();
        // Loop through all nodes and connections in loaded container class and add them to current dialogue
        foreach (Node node in dialogue.nodes)
        {
            if (node is StartNode)
            {
                StartNode newNode = CreateInstance<StartNode>();
                newNode.Init(node.id, node.rect.position, StartNode.width, StartNode.defaultHeight, nodeStyle, selectedNodeStyle, outPointStyle, OnClickOutPoint, OnClickRemoveNode);
                nodes.Add(newNode);
            }
            if (node is DialogueLineNode)
            {
                DialogueLineNode newNode = CreateInstance<DialogueLineNode>();
                newNode.Init(node.id, node.rect.position, DialogueLineNode.width, DialogueLineNode.defaultHeight, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode);
                DialogueLineNode tempNode = (DialogueLineNode)node;
                newNode.Load(tempNode.actorName, tempNode.dialogueLine, tempNode.unskippable, node.inPoints.Count, tempNode.Day, tempNode.Clip, tempNode.Char, tempNode._fmod, tempNode.Dia, tempNode.animNR);
                nodes.Add(newNode);
            }
            else if (node is PlayerChoiceNode)
            {
                PlayerChoiceNode newNode = CreateInstance<PlayerChoiceNode>();
                newNode.Init(node.id, node.rect.position, PlayerChoiceNode.width, PlayerChoiceNode.defaultHeight, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode);
                PlayerChoiceNode tempNode = (PlayerChoiceNode)node;
                newNode.Load(tempNode.optionLines, node.inPoints.Count, node.outPoints.Count);
                nodes.Add(newNode);
            }
            else if (node is CheckVariableNode)
            {
                CheckVariableNode newNode = CreateInstance<CheckVariableNode>();
                newNode.Init(node.id, node.rect.position, CheckVariableNode.width, CheckVariableNode.defaultHeight, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode);
                CheckVariableNode tempNode = (CheckVariableNode)node;
                newNode.Load(tempNode.boolIndex, node.inPoints.Count);
                nodes.Add(newNode);
            }
            else if (node is SetVariableNode)
            {
                SetVariableNode newNode = CreateInstance<SetVariableNode>();
                newNode.Init(node.id, node.rect.position, SetVariableNode.width, SetVariableNode.defaultHeight, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode);
                SetVariableNode tempNode = (SetVariableNode)node;
                newNode.Load(tempNode.boolIndex, tempNode.boolValueIndex, node.inPoints.Count );
                nodes.Add(newNode);
            }

            // Make sure currentHighestID is updated to avoid multiple nodes with same id
            if (node.id > currentHighestID)
            {
                currentHighestID = node.id;
            }
        }
        foreach (Connection connection in dialogue.connections)
        {
            Connection newConnection = CreateInstance<Connection>();
            Node inPointNode = null;
            Node outPointNode = null;
            // Go through all nodes in current dialogue to find which ones to create a connection between
            foreach (Node node in nodes)
            {
                if (connection.inPoint.node.id == node.id)
                {
                    inPointNode = node;
                }
                if (connection.outPoint.node.id == node.id)
                {
                    outPointNode = node;
                }
            }
            if (inPointNode != null && outPointNode != null)
            {
                // Create a connection between nodes
                newConnection.Load(inPointNode, outPointNode, connection.inPoint.index, connection.outPoint.index, OnClickRemoveConnection);
                connections.Add(newConnection);
            }
            else
            {
                Debug.Log("Load connection failed. inPointNode: " + inPointNode + " outPointNode: " + outPointNode);
            }
        }
    }

    private void LoadSpecific(string assetPath)
    {
        // Load selected asset into container class
        dialogue = (DialogueContainer)AssetDatabase.LoadAssetAtPath(assetPath, typeof(DialogueContainer));
        // Clear nodes and connections of current dialogue
        nodes.Clear();
        connections.Clear();
        // Loop through all nodes and connections in loaded container class and add them to current dialogue
        foreach (Node node in dialogue.nodes)
        {
            if (node is StartNode)
            {
                StartNode newNode = CreateInstance<StartNode>();
                newNode.Init(node.id, node.rect.position, StartNode.width, StartNode.defaultHeight, nodeStyle, selectedNodeStyle, outPointStyle, OnClickOutPoint, OnClickRemoveNode);
                nodes.Add(newNode);
            }
            if (node is DialogueLineNode)
            {
                DialogueLineNode newNode = CreateInstance<DialogueLineNode>();
                newNode.Init(node.id, node.rect.position, DialogueLineNode.width, DialogueLineNode.defaultHeight, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode);
                DialogueLineNode tempNode = (DialogueLineNode)node;
                newNode.Load(tempNode.actorName, tempNode.dialogueLine, tempNode.unskippable, node.inPoints.Count, tempNode.Day, tempNode.Clip, tempNode.Char, tempNode._fmod, tempNode.Dia, tempNode.animNR);
                nodes.Add(newNode);
            }
            else if (node is PlayerChoiceNode)
            {
                PlayerChoiceNode newNode = CreateInstance<PlayerChoiceNode>();
                newNode.Init(node.id, node.rect.position, PlayerChoiceNode.width, PlayerChoiceNode.defaultHeight, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode);
                PlayerChoiceNode tempNode = (PlayerChoiceNode)node;
                newNode.Load(tempNode.optionLines, node.inPoints.Count, node.outPoints.Count);
                nodes.Add(newNode);
            }
            else if (node is CheckVariableNode)
            {
                CheckVariableNode newNode = CreateInstance<CheckVariableNode>();
                newNode.Init(node.id, node.rect.position, CheckVariableNode.width, CheckVariableNode.defaultHeight, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode);
                CheckVariableNode tempNode = (CheckVariableNode)node;
                newNode.Load(tempNode.boolIndex, node.inPoints.Count);
                nodes.Add(newNode);
            }
            else if (node is SetVariableNode)
            {
                SetVariableNode newNode = CreateInstance<SetVariableNode>();
                newNode.Init(node.id, node.rect.position, SetVariableNode.width, SetVariableNode.defaultHeight, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode);
                SetVariableNode tempNode = (SetVariableNode)node;
                newNode.Load(tempNode.boolIndex, tempNode.boolValueIndex, node.inPoints.Count);
                nodes.Add(newNode);
            }

            // Make sure currentHighestID is updated to avoid multiple nodes with same id
            if (node.id > currentHighestID)
            {
                currentHighestID = node.id;
            }
        }
        foreach (Connection connection in dialogue.connections)
        {
            Connection newConnection = CreateInstance<Connection>();
            Node inPointNode = null;
            Node outPointNode = null;
            // Go through all nodes in current dialogue to find which ones to create a connection between
            foreach (Node node in nodes)
            {
                if (connection.inPoint.node.id == node.id)
                {
                    inPointNode = node;
                }
                if (connection.outPoint.node.id == node.id)
                {
                    outPointNode = node;
                }
            }
            if (inPointNode != null && outPointNode != null)
            {
                // Create a connection between nodes
                newConnection.Load(inPointNode, outPointNode, connection.inPoint.index, connection.outPoint.index, OnClickRemoveConnection);
                connections.Add(newConnection);
            }
            else
            {
                Debug.Log("Load connection failed. inPointNode: " + inPointNode + " outPointNode: " + outPointNode);
            }
        }
    }

    private void ProcessEvents(Event e)
    {
        drag = Vector2.zero;

        switch(e.type)
        {
            case EventType.MouseDown:
                if (e.button == 0)
                {
                    ClearConnectionSelection();
                }
                if (e.button == 1)
                {
                    ProcessContextMenu(e.mousePosition);
                }
                break;

            case EventType.MouseDrag:
                if (e.button == 0)
                {
                    OnDrag(e.delta);
                }
                break;
        }
    }

    private void ProcessNodeEvents(Event e)
    {
        if (nodes != null)
        {
            foreach(Node node in nodes)
            {
                bool guiChanged = node.ProcessEvents(e);

                if (guiChanged)
                {
                    GUI.changed = true;
                }
            }
        }
    }

    private void ProcessContextMenu(Vector2 mousePosition)
    {
        // Create menu to show under mouse when right clicking empty space
        GenericMenu genericMenu = new GenericMenu();
        genericMenu.AddItem(new GUIContent("Add Start Node"), false, () => OnClickAddStartNode(mousePosition));
        genericMenu.AddItem(new GUIContent("Add Dialogue Line Node"), false, () => OnClickAddDialogueLineNode(mousePosition));
        genericMenu.AddItem(new GUIContent("Add Player Choice Node"), false, () => OnClickAddPlayerChoiceNode(mousePosition));
        genericMenu.AddItem(new GUIContent("Add Check Variable Node"), false, () => OnClickAddCheckVariableNode(mousePosition));
        genericMenu.AddItem(new GUIContent("Add Set Variable Node"), false, () => OnClickAddSetVariableNode(mousePosition));
        genericMenu.ShowAsContext();
    }

    private void OnDrag(Vector2 delta)
    {
        drag = delta;

        // Send drag command to all nodes in current dialogue
        if (nodes != null)
        {
            foreach(Node node in nodes)
            {
                node.Drag(delta);
            }
        }

        GUI.changed = true;
    }

    private void OnClickAddStartNode(Vector2 mousePosition)
    {
        if (nodes == null)
        {
            nodes = new List<Node>();
        }

        foreach (Node tempNode in nodes)
        {
            if (tempNode is StartNode)
            {
                EditorUtility.DisplayDialog("Start Node already exists", "A Start Node already exists in this dialogue", "OK");
                return;
            }
        }

        StartNode node = CreateInstance<StartNode>();
        node.Init(0, mousePosition, StartNode.width, StartNode.defaultHeight, nodeStyle, selectedNodeStyle, outPointStyle, OnClickOutPoint, OnClickRemoveNode);

        nodes.Add(node);
    }

    private void OnClickAddDialogueLineNode(Vector2 mousePosition)
    {
        if (nodes == null)
        {
            nodes = new List<Node>();
        }

        currentHighestID++;
        DialogueLineNode node = CreateInstance<DialogueLineNode>();
        node.Init(currentHighestID, mousePosition, DialogueLineNode.width, DialogueLineNode.defaultHeight, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode);

        nodes.Add(node);
    }   

    private void OnClickAddPlayerChoiceNode(Vector2 mousePosition)
    {
        if (nodes == null)
        {
            nodes = new List<Node>();
        }

        currentHighestID++;
        PlayerChoiceNode node = CreateInstance<PlayerChoiceNode>();
        node.Init(currentHighestID, mousePosition, PlayerChoiceNode.width, PlayerChoiceNode.defaultHeight, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode);

        nodes.Add(node);
    }

    private void OnClickAddCheckVariableNode(Vector2 mousePosition)
    {
        if (nodes == null)
        {
            nodes = new List<Node>();
        }

        currentHighestID++;
        CheckVariableNode node = CreateInstance<CheckVariableNode>();
        node.Init(currentHighestID, mousePosition, CheckVariableNode.width, CheckVariableNode.defaultHeight, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode);

        nodes.Add(node);
    }

    private void OnClickAddSetVariableNode(Vector2 mousePosition)
    {
        if (nodes == null)
        {
            nodes = new List<Node>();
        }

        currentHighestID++;
        SetVariableNode node = CreateInstance<SetVariableNode>();
        node.Init(currentHighestID, mousePosition, SetVariableNode.width, SetVariableNode.defaultHeight, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode);

        nodes.Add(node);
    }

    private void OnClickInPoint(ConnectionPoint inPoint)
    {
        selectedInPoint = inPoint;

        if (selectedOutPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }

    public void OnClickOutPoint(ConnectionPoint outPoint)
    {
        selectedOutPoint = outPoint;

        if (selectedInPoint != null)
        {
            if (selectedOutPoint.node != selectedInPoint.node)
            {
                CreateConnection();
                ClearConnectionSelection();
            }
            else
            {
                ClearConnectionSelection();
            }
        }
    }

    private void OnClickRemoveConnection(Connection connection)
    {
        connections.Remove(connection);
    }

    private void OnClickRemoveNode(Node node)
    {
        // Remove connections of node
        if (connections != null)
        {
            List<Connection> connectionsToRemove = new List<Connection>();

            foreach(Connection connection in connections)
            {
                // Loop through every inpoint and outpoint of node to find all connections to remove
                for (int i = 0; i < Mathf.Max(node.inPoints.Count, node.outPoints.Count); i++)
                {
                    if (i + 1 <= node.inPoints.Count && connection.inPoint == node.inPoints[i])
                    {
                        connectionsToRemove.Add(connection);
                    }
                    if (i + 1 <= node.outPoints.Count && connection.outPoint == node.outPoints[i])
                    {
                        connectionsToRemove.Add(connection);
                    }
                }
            }

            foreach(Connection connection in connectionsToRemove)
            {
                connections.Remove(connection);
            }

            connectionsToRemove = null;
        }

        // Remove node
        nodes.Remove(node);
    }

    private void CreateConnection()
    {
        if (connections ==  null)
        {
            connections = new List<Connection>();
        }

        Connection connection = CreateInstance<Connection>();
        connection.Init(selectedInPoint, selectedOutPoint, OnClickRemoveConnection);
        connections.Add(connection);
    }

    private void ClearConnectionSelection()
    {
        selectedInPoint = null;
        selectedOutPoint = null;
    }
}
#endif
