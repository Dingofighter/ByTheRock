using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SaveLoad
{

    public static List<Game> savedGames = new List<Game>();

    //it's static so we can call it from anywhere
    public static void Save()
    {
        SaveLoad.savedGames.Add(Game.current);
        Debug.Log("saved " + Game.current.invSlot1);
        BinaryFormatter bf = new BinaryFormatter();
        //Application.persistentDataPath is a string, so if you wanted you can put that into debug.log if you want to know where save games are located
        FileStream file = File.Create(Application.persistentDataPath + "/invSaves.sav"); //you can call it anything you want
        bf.Serialize(file, SaveLoad.savedGames);
        file.Close();
    }

    public static void Load()
    {
        if (File.Exists(Application.persistentDataPath + "/invSaves.sav"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/invSaves.sav", FileMode.Open);
            SaveLoad.savedGames = (List<Game>)bf.Deserialize(file);
            Game.current = savedGames[0];
            Debug.Log("loaded " + Game.current.invSlot1);
            file.Close();
        }
    }
}