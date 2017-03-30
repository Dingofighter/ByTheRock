using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueHandler : MonoBehaviour {

    public Text dialogueText;
    bool inDialogue = false;
    Dialogue currentDialogue;
    int currentLine = 0;

    // Use this for initialization
    void Start () {
        dialogueText.text = "";
    }
	
	// Update is called once per frame
	void Update () {
	    if (inDialogue && Input.GetButtonDown("Interact"))
        {
            NextLine();
        }
	}

    public void StartDialogue(Dialogue dialogue)
    {
        inDialogue = true;
        currentDialogue = dialogue;
        currentLine = 0;
        dialogueText.text = dialogue.GetLine(currentLine);
    }

    void NextLine()
    {
        currentLine++;
        if (currentLine + 1 > currentDialogue.GetLength())
        {
            inDialogue = false;
            dialogueText.text = "";
        }
        else
        {
            dialogueText.text = currentDialogue.GetLine(currentLine);
        }
    }
}
