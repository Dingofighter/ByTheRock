using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueHandler : MonoBehaviour {

    public Text dialogueText;
    public Text dialogueNameText;
    public Button choiceButtonPrefab;
    public Vector3 optionsPosition;
    public bool inDialogue = false;
    Dialogue currentDialogue;
    Node currentNode;

    // Use this for initialization
    void Start () {
        dialogueNameText.text = "";
        dialogueText.text = "";
    }
	
	// Update is called once per frame
	void Update () {
	    if (inDialogue && Input.GetButtonDown("Interact"))
        {
            NextNode(0);
        }
	}

    public void StartDialogue(Dialogue dialogue)
    {
        inDialogue = true;
        currentDialogue = dialogue;
        currentNode = currentDialogue.GetNode(0);
        NextNode(0);
    }

    void NextNode(int option)
    {
        currentNode = currentDialogue.GetNode(currentNode.nextNodesID[option]);

        if (currentNode is DialogueLineNode)
        {
            DialogueLineNode tempNode = (DialogueLineNode)currentNode;
            dialogueNameText.text = tempNode.actorName;
            dialogueText.text = tempNode.dialogueLine;
        }
        else if (currentNode is PlayerChoiceNode)
        {
            PlayerChoiceNode tempNode = (PlayerChoiceNode)currentNode;
            dialogueNameText.text = "Ougrah";
            for (int i = 0; i < tempNode.optionLines.Count; i++)
            {
                Button choiceButton = (Button) Instantiate(choiceButtonPrefab, optionsPosition, Quaternion.identity);
                choiceButton.transform.SetParent(dialogueText.transform.parent);
                choiceButton.GetComponentInChildren<Text>().text = tempNode.optionLines[i];
            }
        }
        /*
        currentLine = currentDialogue.GetLine(currentLine).nextLine - 1;
        if (currentLine < 0)
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
        */
    }
}
