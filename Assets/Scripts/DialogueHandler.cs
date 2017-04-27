﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class DialogueHandler : BaseEmitter {

    public Text dialogueText;
    public Text dialogueNameText;
    public Button choiceButtonPrefab;
    public float displayTimePerChar = 0.3f;
    public Vector3 optionsPosition;
    public Vector3 optionsOffset;
    public bool inDialogue = false;
    private bool isChoice = false;
    private bool choiceSelected = false;
    private List<Button> choiceButtons;
    Dialogue currentDialogue;
    Node currentNode;
    private bool dialogueChosen = false;
    private float autoClearTime;
    public Transform interact;

    // Use this for initialization
    protected override void Start () {

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

        if (inDialogue && currentDialogue.walkAndTalk)
        {
            if (Time.time >= autoClearTime)
            {
                NextNode(0);
            }
        }

        if (inDialogue)
        {

        }

    
    }

    public void StartDialogue(Dialogue[] dialogues)
    {
        foreach (Dialogue dialogue in dialogues)
        {
            dialogueChosen = true;
            for (int i = 0; i < dialogue.numFlagsRequired; i++)
            {
                if (dialogue.boolValueIndex[i] == 0)
                {
                    if (AllFlags.Instance.flags[dialogue.boolIndex[i]].value == false)
                    {
                        dialogueChosen = false;
                        break;
                    }
                }
                else
                {
                    if (AllFlags.Instance.flags[dialogue.boolIndex[i]].value == true)
                    {
                        dialogueChosen = false;
                        break;
                    }
                }
            }
            if (dialogueChosen)
            {
                currentDialogue = dialogue;
                break;
            }
        }

        if (!dialogueChosen) return;

        if (GameManager.instance.talking) return;

        if (!currentDialogue.walkAndTalk)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            GameManager.instance.talking = true;
        }
        inDialogue = true;
        currentNode = currentDialogue.GetNode(0);
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
            if (_EventInstance != null)
                _EventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            return;
        }

        //Set current node
        currentNode = currentDialogue.GetNode(currentNode.nextNodesID[option]);

        //Check if currentNode is dialogueLine or playerLine
        if (currentNode is DialogueLineNode)
        {
            isChoice = false;
            DialogueLineNode tempNode = (DialogueLineNode)currentNode;
            setFmodParams(tempNode.DayBank, tempNode.Char, tempNode.Day, tempNode.Clip);
            base.Start();
            _EventInstance.start();
            dialogueNameText.text = tempNode.actorName;
            dialogueText.text = tempNode.dialogueLine;


            if (currentDialogue.walkAndTalk)
            {
                autoClearTime = Time.time + (tempNode.dialogueLine.Length * displayTimePerChar);
            }
        }
        else if (currentNode is PlayerChoiceNode)
        {
            isChoice = true;
            PlayerChoiceNode tempNode = (PlayerChoiceNode)currentNode;

            for (int i = 0; i < tempNode.optionLines.Count; i++)
            {
                // Spawn buttons with offset
                Button choiceButton = (Button)Instantiate(choiceButtonPrefab, optionsPosition - (optionsOffset * i), Quaternion.identity);
                choiceButton.transform.SetParent(dialogueText.transform.parent);
                choiceButton.GetComponentInChildren<Text>().text = tempNode.optionLines[i];
                choiceButtons.Add(choiceButton);
            }
        }
        else if (currentNode is CheckVariableNode)
        {
            CheckVariableNode tempNode = (CheckVariableNode)currentNode;
            if (AllFlags.Instance.flags[tempNode.boolIndex].value)
            {
                NextNode(0);
            }
            else
            {
                NextNode(1);
            }
        }
        else if (currentNode is SetVariableNode)
        {
            SetVariableNode tempNode = (SetVariableNode)currentNode;
            if (tempNode.boolValueIndex == 0)
            {
                AllFlags.Instance.flags[tempNode.boolIndex].value = true;
            }
            else
            {
                AllFlags.Instance.flags[tempNode.boolIndex].value = false;
            }

            NextNode(0);
        }
    }

    void setFmodParams(string bank, int charac, int dia, int vc)
    {
        if (_EventInstance != null)
            _EventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        System.Guid thing;

        FMOD.Studio.Util.ParseID(bank, out thing);
        var gm = FindObjectOfType<GameManager>().GetComponent<GameManager>();

        if (gm._fmodSS.getEventByID(thing, out _EventDescription) != FMOD.RESULT.OK)
            Debug.Log("Event not found");

        if (_EventDescription.createInstance(out _EventInstance) != FMOD.RESULT.OK)
            Debug.Log("Instance not created because fuck you slask");


        

        float c = charac + 0.2f;
        float d = dia + 0.2f;
        float v = vc + 0.2f;

        Params = new FMODUnity.ParamRef[3];

        Debug.Log("sLASK1");
        //base.Start();
        Debug.Log("sLASK2");

        

        //_3dAttributes.position.x = -20;// interact.transform.position.x;
        //_3dAttributes.position.y = -3;// interact.transform.position.y;
        //_3dAttributes.position.z = 25; //interact.transform.position.z;
        //_EventInstance.set3DAttributes(_3dAttributes);

        _EventInstance.setParameterValue("CHAR", c);
        _EventInstance.setParameterValue("Dialogues", d);
        _EventInstance.setParameterValue("VoiceClip", v);

        /*Params[0].Name = "CHAR";
        Params[1].Name = "Dialogues";
        Params[2].Name = "VocieClip";

        Params[0].Value = c;
        Params[1].Value = d;
        Params[2].Value = v;**/

        //Transform p = FindObjectOfType<PlayerController>().GetComponent<Transform>();

        _3dAttributes.position.x = -20;// interact.transform.position.x;
        _3dAttributes.position.y = -3;// interact.transform.position.y;
        _3dAttributes.position.z = 25; //interact.transform.position.z;
        _EventInstance.set3DAttributes(_3dAttributes);
    }
}
