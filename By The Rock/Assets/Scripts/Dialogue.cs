using UnityEngine;
using System.Collections;

public class Dialogue : MonoBehaviour {

    public string[] lines;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public string GetLine(int lineNumber)
    {
        return lines[lineNumber];
    }

    public int GetLength()
    {
        return lines.Length;
    }
}
