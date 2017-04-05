using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;

[Serializable]

public class BoolList : ScriptableObject {


    public List<bool> boolList;


    public void OnEnable()
    {
        if (boolList == null)
        {
            boolList = new List<bool>();
        }
       
    }
}
