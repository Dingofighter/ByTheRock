using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Interactable : MonoBehaviour {

    public int numFlagsRequired;

    public List<int> boolIndex;
    public List<int> boolValueIndex;

    public bool setFlag;

    private bool canInteract;

    public List<int> setBoolIndex;
    public List<int> setBoolValueIndex;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Interact()
    {
        if (setFlag)
        {

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
