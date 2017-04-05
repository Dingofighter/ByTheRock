using UnityEngine;
using UnityEditor;

using System.Collections;

[CustomEditor (typeof(DialogueLine)), CanEditMultipleObjects]

public class DialogueLineEditor : Editor {

    public SerializedProperty
        nodeType_Prop,
        name_Prop,
        line_Prop,
        player_Prop,
        nextLine_Prop;

    void OnEnable()
    {
        nodeType_Prop = serializedObject.FindProperty("nodeType");
        name_Prop = serializedObject.FindProperty("name");
        line_Prop = serializedObject.FindProperty("line");
        player_Prop = serializedObject.FindProperty("player");
        nextLine_Prop = serializedObject.FindProperty("nextLine");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(nodeType_Prop);

        DialogueLine.Type type = (DialogueLine.Type)nodeType_Prop.enumValueIndex;

        switch (type)
        {
            case DialogueLine.Type.CharacterLine:
                EditorGUILayout.PropertyField(name_Prop, new GUIContent("Name"));
                EditorGUILayout.PropertyField(line_Prop, new GUIContent("Line"));
                EditorGUILayout.PropertyField(nextLine_Prop, new GUIContent("Next Line"));

                break;

            case DialogueLine.Type.PlayerLine:
                EditorGUILayout.PropertyField(name_Prop, new GUIContent("Name"));
                EditorGUILayout.PropertyField(line_Prop, new GUIContent("Line"));
                EditorGUILayout.PropertyField(nextLine_Prop, new GUIContent("Next Line"));
                EditorGUILayout.PropertyField(player_Prop, new GUIContent("Player"));

                break;
        }

        serializedObject.ApplyModifiedProperties();
    }

}
