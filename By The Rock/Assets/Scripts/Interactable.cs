using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Interactable : MonoBehaviour {

    public int numFlagsRequired;

    public List<int> boolIndex;
    public List<int> boolValueIndex;

    public bool setFlag;

    private bool canInteract;

    public int setBoolIndex;
    public int setBoolValueIndex;

    // Use this for initialization
    void Start () {
        if (boolIndex == null)
        {
            boolIndex = new List<int>();
        }
        if (boolValueIndex == null)
        {
            boolValueIndex = new List<int>();
        }
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void Interact()
    {
        if (setFlag)
        {
            if (setBoolValueIndex == 0)
            {
                AllFlags.Instance.flags[setBoolIndex].value = true;
            }
            else
            {
                AllFlags.Instance.flags[setBoolIndex].value = false;
            }
        }
    }

    public bool CheckInteractable()
    {
        canInteract = true;

        if (numFlagsRequired < 1)
        {
            return true;
        }

        for (int i = 0; i < numFlagsRequired; i++)
        {
            if (boolValueIndex[i] == 0)
            {
                if (AllFlags.Instance.flags[boolIndex[i]].value == false)
                {
                    canInteract = false;
                    break;
                }
            }
            else
            {
                if (AllFlags.Instance.flags[boolIndex[i]].value == true)
                {
                    canInteract = false;
                    break;
                }
            }
        }
        if (canInteract)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
