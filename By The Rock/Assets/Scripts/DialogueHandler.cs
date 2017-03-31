using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueHandler : MonoBehaviour {

    public Text dialogueText;
    public Text dialogueNameText;
    public bool inDialogue = false;
    Dialogue currentDialogue;
    int currentLine = 0;

    // Use this for initialization
    void Start () {
        dialogueNameText.text = "";
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
        dialogueNameText.text = currentDialogue.GetLine(currentLine).name;
        dialogueText.text = currentDialogue.GetLine(currentLine).line;
    }

    void NextLine()
    {
        currentLine++;
        if (currentLine + 1 > currentDialogue.GetLength())
        {
            inDialogue = false;
            dialogueNameText.text = "";
            dialogueText.text = "";
        }
        else
        {
            dialogueNameText.text = currentDialogue.GetLine(currentLine).name;
            dialogueText.text = currentDialogue.GetLine(currentLine).line;
        }
    }
}
