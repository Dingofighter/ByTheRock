using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(Dialogue))]
[CanEditMultipleObjects]
public class DialogueEditor : Editor
{
    string[] bools;
    List<int> boolIndex;
    List<int> boolValueIndex;
    public string[] boolValues = { "True", "False" };

    void OnEnable()
    {
        if (boolIndex == null)
        {
            boolIndex = new List<int>();
        }
        if (boolValueIndex == null)
        {
            boolValueIndex = new List<int>();
        }

        AllFlags allFlags = (AllFlags)AssetDatabase.LoadAssetAtPath("Assets/Resources/AllFlags.asset", typeof(AllFlags));
        bools = new string[allFlags.flags.Count];
        for (int i = 0; i < bools.Length; i++)
        {
            bools[i] = allFlags.flags[i].description;
        }
    }

    public override void OnInspectorGUI()
    {
        Dialogue targetDialogue = (Dialogue)target;

        targetDialogue.dialogue = (DialogueContainer)EditorGUILayout.ObjectField(targetDialogue.dialogue, typeof(DialogueContainer), false);

        boolIndex = targetDialogue.boolIndex;
        boolValueIndex = targetDialogue.boolValueIndex;

        targetDialogue.walkAndTalk = EditorGUILayout.Toggle("Walk and Talk", targetDialogue.walkAndTalk);

        EditorGUILayout.LabelField("Flags needed to start this dialogue:");

        for (int i = 0; i < targetDialogue.numFlagsRequired; i++)
        {
            GUILayout.BeginHorizontal();

            boolIndex[i] = EditorGUILayout.Popup(boolIndex[i], bools);
            boolValueIndex[i] = EditorGUILayout.Popup(boolValueIndex[i], boolValues);

            GUILayout.EndHorizontal();
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add flag"))
        {
            targetDialogue.numFlagsRequired++;

            if (boolIndex == null)
            {
                boolIndex = new List<int>();
            }
            if (boolValueIndex == null)
            {
                boolValueIndex = new List<int>();
            }

            boolIndex.Add(0);
            boolValueIndex.Add(0);
        }
        if (GUILayout.Button("Remove flag") && (targetDialogue.numFlagsRequired > 0))
        {
            targetDialogue.numFlagsRequired--;
            boolIndex.Remove(boolIndex.Count - 1);
            boolValueIndex.Remove(boolValueIndex.Count - 1);
        }
        GUILayout.EndHorizontal();

        targetDialogue.boolIndex = boolIndex;
        targetDialogue.boolValueIndex = boolValueIndex;
    }
}
