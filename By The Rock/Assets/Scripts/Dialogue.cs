using UnityEngine;
using System.Collections;

public class Dialogue : MonoBehaviour {

    public int startLine;
    public DialogueLine[] lines;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public DialogueLine GetLine(int lineNumber)
    {
        return lines[lineNumber];
    }

    public int GetLength()
    {
        return lines.Length;
    }
}
