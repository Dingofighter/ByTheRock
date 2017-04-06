using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class DialogueHandler : MonoBehaviour {

    public Text dialogueText;
    public Text dialogueNameText;
    public Button choiceButtonPrefab;
    public Vector3 optionsPosition;
    public Vector3 optionsOffset;
    public bool inDialogue = false;
    private bool isChoice = false;
    private bool choiceSelected = false;
    private List<Button> choiceButtons;
    Dialogue currentDialogue;
    Node currentNode;

    // Use this for initialization
    void Start () {
        dialogueNameText.text = "";
        dialogueText.text = "";

        if (choiceButtons == null)
        {
            choiceButtons = new List<Button>();
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (inDialogue && Input.GetButtonDown("Interact") && !GameManager.instance.paused)
        {
            // If player choice, check if button clicked
            if (isChoice)
            {
                for(int i = 0; i < choiceButtons.Count; i++)
                {
                    //If a button is clicked, go to nextLine
                    if (EventSystem.current.currentSelectedGameObject == choiceButtons[i].gameObject)
                    {
                        NextNode(i);
                        choiceSelected = true;
                    }
                }

                //If a choice is selected, destroy all buttons and clear buttonlist
                if (choiceSelected)
                {
                    foreach (Button choiceButton in choiceButtons)
                    {
                        Destroy(choiceButton.gameObject);
                    }

                    choiceButtons.Clear();
                    choiceSelected = false;
                }
            }
            else
            {
                NextNode(0);
            }
        }
	}

    public void StartDialogue(Dialogue dialogue)
    {
        if (GameManager.instance.talking) return;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        GameManager.instance.talking = true;
        inDialogue = true;
        currentDialogue = dialogue;
        currentNode = currentDialogue.GetNode(0);
        NextNode(0);
    }

    void NextNode(int option)
    {
        //Remove dialogue if last
        if (!currentDialogue.nodes.ContainsKey(currentNode.nextNodesID[option]))
        {
            dialogueNameText.text = "";
            dialogueText.text = "";
            inDialogue = false;
            GameManager.instance.talking = false;
            Cursor.lockState = CursorLockMode.Locked;
            return;
        }

        //Set current node
        currentNode = currentDialogue.GetNode(currentNode.nextNodesID[option]);

        //Check if currentNode is dialogueLine or playerLine
        if (currentNode is DialogueLineNode)
        {
            isChoice = false;
            DialogueLineNode tempNode = (DialogueLineNode)currentNode;
            dialogueNameText.text = tempNode.actorName;
            dialogueText.text = tempNode.dialogueLine;
        }
        else if (currentNode is PlayerChoiceNode)
        {
            isChoice = true;
            PlayerChoiceNode tempNode = (PlayerChoiceNode)currentNode;
            dialogueNameText.text = "Ougrah";

            for (int i = 0; i < tempNode.optionLines.Count; i++)
            {
                // Spawn buttons with offset
                Button choiceButton = (Button) Instantiate(choiceButtonPrefab, optionsPosition - (optionsOffset * i), Quaternion.identity);
                choiceButton.transform.SetParent(dialogueText.transform.parent);
                choiceButton.GetComponentInChildren<Text>().text = tempNode.optionLines[i];
                choiceButtons.Add(choiceButton);
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
