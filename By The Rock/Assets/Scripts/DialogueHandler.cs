﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;

public class DialogueHandler : MonoBehaviour {

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
    public bool firstFrame = false;

    private Transform _player;
    private Transform _interact;
    public Vector3 _pos;
    private bool walkietalkie = false;
    private bool voiceSetup = false;

    Animator aOugrah;
    Animator aGaregh;
    Animator aHania;

    public FMODUnity.EmitterGameEvent _PlayEvent = FMODUnity.EmitterGameEvent.None;
    public FMODUnity.EmitterGameEvent _StopEvent = FMODUnity.EmitterGameEvent.None;
    public FMOD.Studio.EventDescription _EventDescription = null;
    public FMOD.Studio.EventInstance _EventInstance = null, _SnapNeat, _SnapDia = null; 
    public FMOD.ATTRIBUTES_3D _3dAttributes;
    public FMOD.Studio.PLAYBACK_STATE _playbackState;

    GameManager gm;

    public float timer = 0;

    public bool _HasTriggered = false;
    public bool _AllowFadeout = true;
    public bool _isQuitting = false;
    public bool _TriggerOnce = false;

    bool once = false;
    bool unskippable = false;

    // Use this for initialization
    void Start () {
        dialogueNameText.text = "";
        dialogueText.text = "";
        gm = FindObjectOfType<GameManager>().GetComponent<GameManager>();

        if (choiceButtons == null)
        {
            choiceButtons = new List<Button>();
        }

        aOugrah = FindObjectOfType<PlayerController>().GetComponent<Animator>();
        aGaregh = FindObjectOfType<orcMovement>().GetComponent<Animator>();
        aHania = FindObjectOfType<TalkCheck>().GetComponent<Animator>();
		
        System.Guid id;
        FMOD.Studio.Util.ParseID("{0a1248cd-c395-42ad-b955-7c4aabbc7a9e}", out id);
        if (gm._fmodSS.getEventByID(id, out _EventDescription) != FMOD.RESULT.OK)
            Debug.Log("Hitta inte dialogsnap");

        if (_EventDescription.createInstance(out _SnapDia) != FMOD.RESULT.OK)
            Debug.Log("Jag vet inte vad som händer och fötter");
		
        DontDestroyOnLoad(this);
    }
	
	// Update is called once per frame
	void Update () {
        if (inDialogue)
        {
            //print("paused: " + GameManager.instance.paused);
            //print("unskippable: " + unskippable);
            //print("firstframe: " + firstFrame);
        }
        if (inDialogue && !firstFrame && Input.GetButtonDown("Interact") && !GameManager.instance.paused && !unskippable)
        {
            print("CLICK NEXT NODE PLEASE");
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

        if (inDialogue)
        {
            if (currentDialogue.walkAndTalk && voiceSetup || unskippable)
                _EventInstance.getPlaybackState(out _playbackState);
        }

        if (inDialogue && currentDialogue.walkAndTalk || unskippable)
        {
            if (voiceSetup)
            { 
                if (_playbackState == FMOD.Studio.PLAYBACK_STATE.STOPPED)
                {
                    NextNode(0);
                }
            }
            else if (Time.time >= autoClearTime)
            {
                NextNode(0);
            }
        }

        if (inDialogue)
        {
            //do stuff
        }

        if (walkietalkie && voiceSetup)
            updatePosition();

        firstFrame = false;

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
        firstFrame = true;
        currentNode = currentDialogue.GetNode(0);
        NextNode(0);

        _SnapDia.start();
    }

    void NextNode(int option)
    {
        print("NextNode");
        //Remove dialogue if last
        if (!currentDialogue.nodes.ContainsKey(currentNode.nextNodesID[option]))
        {
            aOugrah.SetBool("talking", false);
            aGaregh.SetBool("talking", false);
            aHania.SetBool("talking", false);
             
            dialogueNameText.text = "";
            dialogueText.text = "";
            inDialogue = false;
            GameManager.instance.talking = false;
            Cursor.lockState = CursorLockMode.Locked;
            if (_EventInstance != null)
                _EventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

            switch (currentDialogue.name)
            {
                default:
                    break;
                case "BearRoarDialogue":
                    Destroy(currentDialogue.gameObject);
                    Snapshit(2);
                    gm.GetComponent<MusicEmitter>().Change(1, false);
                    break;
                case "Very invisible wall after bear":
                    currentDialogue.GetComponent<WallTimer>().start = true;                   
                    break;
            }

            if (AllFlags.Instance.flags[3].value && !once && _SnapNeat != null)
            {
                _SnapNeat.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                _SnapNeat.release();
                FindObjectOfType<Area2WallTrigger>().GetComponent<Area2WallTrigger>().enableWall();
                once = true;
                gm.GetComponent<MusicEmitter>().play();
            }

            //Debug.Log(currentDialogue.name);

            _SnapDia.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            return;
        }

        //Set current node
        currentNode = currentDialogue.GetNode(currentNode.nextNodesID[option]);

        //Check if currentNode is dialogueLine or playerLine
        if (currentNode is DialogueLineNode)
        {


            isChoice = false;
            DialogueLineNode tempNode = (DialogueLineNode)currentNode;
            setFmodParams(tempNode.Day, tempNode.Char, tempNode.Dia, tempNode.Clip);
            if (voiceSetup)
            {
                updatePosition(tempNode.Char);
                _EventInstance.start();
            }
            dialogueNameText.text = tempNode.actorName;
            dialogueText.text = tempNode.dialogueLine;

            if (!currentDialogue.walkAndTalk)
            {
                if (tempNode.actorName == "Ougrah")
                {
                    switch (tempNode.animNR)
                    {
                        case 0:
                            aOugrah.SetBool("mouth", false);
                            aOugrah.SetBool("talking", false);
                            aOugrah.SetBool("give", false);
                            break;
                        case 1:
                            aOugrah.SetBool("mouth", true);
                            aOugrah.SetBool("talking", false);
                            aOugrah.SetBool("give", false);
                            break;
                        case 2:
                            aOugrah.SetBool("talking", true);
                            aOugrah.SetBool("mouth", false);
                            aOugrah.SetBool("give", false);
                            break;
                        case 3:
                            aOugrah.SetBool("give", true);
                            aOugrah.SetBool("mouth", false);
                            aOugrah.SetBool("talking", false);
                            break;
                    }
                    aOugrah.SetBool("talking", true);

                    aGaregh.SetBool("mouth", false);
                    aGaregh.SetBool("talking", false);
                    aGaregh.SetBool("give", false);

                    aHania.SetBool("mouth", false);
                    aHania.SetBool("talking", false);
                    aHania.SetBool("give", false);
                }
                else if (tempNode.actorName == "Garegh")
                {
                    switch (tempNode.animNR)
                    {
                        case 0:
                            aGaregh.SetBool("mouth", false);
                            aGaregh.SetBool("talking", false);
                            aGaregh.SetBool("give", false);
                            break;
                        case 1:
                            aGaregh.SetBool("mouth", true);
                            aGaregh.SetBool("talking", false);
                            aGaregh.SetBool("give", false);
                            break;
                        case 2:
                            aGaregh.SetBool("talking", true);
                            aGaregh.SetBool("mouth", false);
                            aGaregh.SetBool("give", false);
                            break;
                        case 3:
                            aGaregh.SetBool("give", true);
                            aGaregh.SetBool("mouth", false);
                            aGaregh.SetBool("talking", false);
                            break;
                    }

                    aOugrah.SetBool("mouth", false);
                    aOugrah.SetBool("talking", false);
                    aOugrah.SetBool("give", false);

                    if (Vector3.Distance(aOugrah.transform.position, aGaregh.transform.position) <= 5)
                    {
                        aGaregh.SetBool("talking", true);
                    }
                }
                else if (tempNode.actorName == "Hania" || tempNode.actorName == "???")
                {
                    switch (tempNode.animNR)
                    {
                        case 0:
                            aHania.SetBool("mouth", false);
                            aHania.SetBool("talking", false);
                            aHania.SetBool("give", false);
                            break;
                        case 1:
                            aHania.SetBool("mouth", true);
                            aHania.SetBool("talking", false);
                            aHania.SetBool("give", false);
                            break;
                        case 2:
                            aHania.SetBool("talking", true);
                            aHania.SetBool("mouth", false);
                            aHania.SetBool("give", false);
                            break;
                        case 3:
                            aHania.SetBool("give", true);
                            aHania.SetBool("mouth", false);
                            aHania.SetBool("talking", false);
                            break;
                    }

                    aOugrah.SetBool("mouth", false);
                    aOugrah.SetBool("talking", false);
                    aOugrah.SetBool("give", false);

                    aHania.SetBool("talking", true);
                }
            }
            

            if (currentDialogue.walkAndTalk)
            {
                walkietalkie = true;
                autoClearTime = Time.time + (tempNode.dialogueLine.Length * displayTimePerChar);
            }
            else
                walkietalkie = false;

            if (tempNode.unskippable)
            {
                unskippable = true;
            }
            else
            {
                unskippable = false;
            }
        }
        else if (currentNode is PlayerChoiceNode)
        {
            unskippable = false;
            isChoice = true;
            PlayerChoiceNode tempNode = (PlayerChoiceNode)currentNode;

            for (int i = 0; i < tempNode.optionLines.Count; i++)
            {
                // Spawn buttons with offset
                Button choiceButton = (Button)Instantiate(choiceButtonPrefab, new Vector3((Screen.width / 2), (optionsPosition.y + optionsOffset.y * tempNode.optionLines.Count)) - (optionsOffset * i), Quaternion.identity);
                choiceButton.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 12 * tempNode.optionLines[i].Length);
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

    void setFmodParams(int day, int charac, int dia, int vc)
    {
        if (_EventInstance != null)
        {
            _EventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            _EventInstance.release();
        }

        print(day + " " + dia);

        string guid = null;

        switch (charac)
        {
            default:
                break;
            case 1: //Ougrah
                guid = setOugrahDialogue(day, dia);
                break;
            case 2: //Garegh
                guid = setGareghDialogue(day, dia);
                break;
            case 3: //Hania
                guid = setHaniaDialogue(day, dia);
                break;
            case 4: //Bear
                guid = "{b97a009f-2201-4a20-bc77-789c4d1ec4a4}";
                Snapshit(1);
                break;
        }

        System.Guid thing;

        if (guid == null)
        {
            voiceSetup = false;
            return;
        }

        FMOD.Studio.Util.ParseID(guid, out thing);

        if (gm._fmodSS.getEventByID(thing, out _EventDescription) != FMOD.RESULT.OK)
        {
            Debug.Log("Event not found");
            voiceSetup = false;
            return;
        }


        if (_EventDescription.createInstance(out _EventInstance) != FMOD.RESULT.OK)
        {
            Debug.Log("Instance not created because fuck you slask... gee that was harsh :|... slask wrote fuck yougrah slask");
            voiceSetup = false;
            return;
        }

        _EventInstance.setParameterValue("VoiceClip", vc);

        _player = FindObjectOfType<PlayerController>().GetComponent<Transform>();
        voiceSetup = true;
    }

    void updatePosition(int charac = 1)
    {
        if (charac > 1.3f && charac != 4)
        {
            if (currentDialogue.autoTriggered)
            {
                if (currentDialogue.rotationTarget)
                {
                    _pos = currentDialogue.rotationTarget.position;
                }
                else
                {
                    _pos = _player.transform.position;
                }
            }
            else
            {
                _interact = _player.GetComponent<PlayerController>().getCollisionTransform();
                _pos = _interact.transform.position;
            }
        }

        else
        {
            _pos = _player.transform.position;
        }


        _3dAttributes.position.x = _pos.x;// interact.transform.position.x;
        _3dAttributes.position.y = _pos.y;// interact.transform.position.y;
        _3dAttributes.position.z = _pos.z; //interact.transform.position.z;
        _EventInstance.set3DAttributes(_3dAttributes);
        //Debug.Log(_pos);
    }


    void Snapshit(int thingtodo = 0)
    {
        if (_SnapNeat != null)
        {
            _SnapNeat.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _SnapNeat.release();
        }

        System.Guid thing;
        var gm = FindObjectOfType<GameManager>().GetComponent<GameManager>();

        switch (thingtodo)
        {
            default:
                Debug.Log("Didn't find ID");
                FMOD.Studio.Util.ParseID("{9f4a9dab-dc20-4a5b-9a2c-5c20fc9b1113}", out thing);
                break;
            case 1:
                FMOD.Studio.Util.ParseID("{3cfe6b6e-b65d-4cc5-9474-c4f5db20fb30}", out thing);
                break;
            case 2:
                FMOD.Studio.Util.ParseID("{afb5aa7c-fcc6-4b80-8728-fd72e9da12ee}", out thing);
                break;
        }


        if (gm._fmodSS.getEventByID(thing, out _EventDescription) != FMOD.RESULT.OK)
        {
            Debug.Log("Oh SNAP... shot wasn't found");
        }
        if (_EventDescription.createInstance(out _SnapNeat) != FMOD.RESULT.OK)
        {
            Debug.Log("Snap your fingers if the instance wasn't created *snaps fingers*");
        }

        _SnapNeat.start();
    }

    string setHaniaDialogue(int day, int dia)
    {
        if (day == 1)
        {
            switch (dia)
            {
                default:
                    break;
                case 4:
                    return "{6f42954b-9c3d-46b2-9dd1-5cd8eaff906d}";
                case 5:
                    return "{f1c31f39-d655-43f6-b853-6f1068520640}";
            }
        }
        else if (day == 2)
        {

        }
        else if (day == 3)
        {

        }
        else if (day == 4)
        {

        }

        return null; //nothing found
    }

    string setGareghDialogue(int day, int dia)
    {
        if (day == 1)
        {
            switch (dia)
            {
                default:
                    break;
                case -1:
                    return "{607a89f9-3755-4219-9eeb-84d798408365}";
                case 1:
                    return "{560683cf-a949-412d-9d34-6db05efd5b50}";
                case 3:
                    return "{669ebd94-c646-4eeb-ab24-9c193ad433ac}";
            }
        }
        else if (day == 2)
        {

        }
        else if (day == 3)
        {

        }
        else if (day == 4)
        {

        }

        return null; //nothing found
    }

    string setOugrahDialogue(int day, int dia)
    {
        if (day == 1)
        {
            switch (dia)
            {
                default:
                    break;
                case -5:
                    return "{4f135a78-a83d-41e2-b4c3-c7d4d60bd352}";
                case -4:
                    return "{b80cdc71-085b-469b-967f-a64753401896}";
                case -1:
                    return "{b8a286eb-b103-4796-a7c8-86d9f78d1f77}";
                case 1:
                    return "{67fef64a-8f51-4791-9692-2a686a2d133f}";
                case 3:
                    return "{ec7623b6-2ae6-431e-ad71-bb07e17356cc}";
                case 4:
                    return "{3a353e26-3950-4484-8b49-250455b3f5be}";
                case 5:
                    return "{23e54cef-927c-4bd7-98cb-f096ec7feca3}";
            }
        }
        else if (day == 2)
        {

        }
        else if (day == 3)
        {

        }
        else if (day == 4)
        {

        }

        return null; //nothing found
    }
}
