using UnityEngine;
using System.Collections;

[System.Serializable]
public class Game
{

    public static Game current;
    public int invSlot1 = -1;
    public int invSlot2 = -1;
    public int invSlot3 = -1;
    public int invSlot4 = -1;

    void Awake()
    {
        current = new Game();
    }

    public void updateValues()
    {
        current = this;
    }

    public void loadValues()
    {
        invSlot1 = current.invSlot1;
        invSlot2 = current.invSlot2;
        invSlot3 = current.invSlot3;
        invSlot4 = current.invSlot4;
    }

    public Game()
    {
        /*
        invSlot1 = -1;
        invSlot2 = -1;
        invSlot3 = -1;
        invSlot4 = -1;*/
    }

}