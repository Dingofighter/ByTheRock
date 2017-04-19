using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class AllFlags : ScriptableObject {

    public List<Flag> flags;

    private static AllFlags instance;

	public static AllFlags Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType<AllFlags>();
            }
            if (!instance)
            {
                instance = Resources.Load<AllFlags>("AllFlags");
            }
            if (!instance)
            {
                Debug.Log(instance);
                Debug.LogError("AllFlags has not been created, go to Assets > Create > AllFlags to create");
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }
	
	public void Reset()
    {
        if (flags == null)
        {
            return;
        }

        for(int i = 0; i < flags.Count; i++)
        {
            flags[i].value = false;
        }
    }

    [MenuItem("Assets/Create/AllFlags")]
    private static void CreateAllConditionsAsset()
    {
        if (Instance)
            return;

        AllFlags instance = CreateInstance<AllFlags>();
        AssetDatabase.CreateAsset(instance, "Assets/Resources/AllFlags.asset");

        Instance = instance;

        instance.flags = new List<Flag>();
    }
}
