using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(Interactable))]
[CanEditMultipleObjects]
public class InteractableEditor : Editor
{
    string[] bools;
    List<int> boolIndex;
    List<int> boolValueIndex;
    public string[] boolValues = { "True", "False" };
    int setBoolIndex;
    int setBoolValueIndex;

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
        Interactable targetInteractable = (target as Interactable);

        boolIndex = targetInteractable.boolIndex;
        boolValueIndex = targetInteractable.boolValueIndex;

        EditorGUILayout.LabelField("Flags needed to interact with this item:");

        for (int i = 0; i < targetInteractable.numFlagsRequired; i++)
        {
            GUILayout.BeginHorizontal();

            boolIndex[i] = EditorGUILayout.Popup(boolIndex[i], bools);
            boolValueIndex[i] = EditorGUILayout.Popup(boolValueIndex[i], boolValues);

            GUILayout.EndHorizontal();
        }

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Add flag"))
        {
            targetInteractable.numFlagsRequired++;

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
        if (GUILayout.Button("Remove flag") && (targetInteractable.numFlagsRequired > 0))
        {
            targetInteractable.numFlagsRequired--;
            boolIndex.Remove(boolIndex.Count - 1);
            boolValueIndex.Remove(boolValueIndex.Count - 1);
        }
        GUILayout.EndHorizontal();

        targetInteractable.boolIndex = boolIndex;
        targetInteractable.boolValueIndex = boolValueIndex;

        targetInteractable.setFlag = GUILayout.Toggle(targetInteractable.setFlag, "Set flag when interacted with");

        if (targetInteractable.setFlag)
        {
            setBoolIndex = targetInteractable.setBoolIndex;
            setBoolValueIndex = targetInteractable.setBoolValueIndex;

            GUILayout.BeginHorizontal();

            setBoolIndex = EditorGUILayout.Popup(setBoolIndex, bools);
            setBoolValueIndex = EditorGUILayout.Popup(setBoolValueIndex, boolValues);

            GUILayout.EndHorizontal();

            targetInteractable.setBoolIndex = setBoolIndex;
            targetInteractable.setBoolValueIndex = setBoolValueIndex;
        }

        if (GUI.changed)
        {
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}
