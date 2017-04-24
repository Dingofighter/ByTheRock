using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Flag {
    public string description;
    public bool value;

    public Flag(string name)
    {
        description = name;
    }
}
