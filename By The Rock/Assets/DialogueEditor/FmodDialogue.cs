using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

#if UNITY_EDITOR
[Serializable]
public class FmodDialogue : GUIDrawer {

    Rect rect2;

    public static int width = 200;
    public static int defaultHeight = 180;
    public static int padding = 15;
    public static int heightIncrease = 55;
    public int currentHeight = defaultHeight;
    public bool fmod = false;

    int[] DayNR = new int[4] {1,2,3,4};
    int[] CharNR = new int[4] { 1, 2, 3, 4};
    int[] DiaNR = new int[17] {0, -7, -6, -5, -4, -3, -2, -1, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
    int[] VoxNR = new int[21] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20 };

    string[] Days = new string[4] { "1","2","3","4" };
    string[] Char = new string[4] { "Ougrah", "Garegh", "Hania", "Bear" };
    string[] Dia1 = new string[17] {"NULL", "IG7", "IG6", "IG5", "IG4", "IG3", "IG2", "IG1", "D1", "D2", "D3", "D4", "D5", "D6", "D7", "D8", "D9" };
    string[] Voxs = new string[21] {"NULL", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20" };

    public int DaySelect;
    public int CharSelect;
    public int DiaSelect;
    public int VoxSelect;

    public string VoiceClip = "VoiceClip", Character = "CHAR";
    public string Dialogues = "Dialogues", Ingame = "In-game";



    public void FMODAddon(Rect rect, string title, GUIStyle style, int height)
    {
        rect2 = new Rect(rect.position, new Vector2(rect.width, 50));
        rect2.y = rect.y + height - 15;

        GUI.Box(rect2, title, style);
        GUILayout.BeginArea(new Rect(rect2.position.x + padding, rect2.position.y + padding, width - padding * 2, currentHeight - 50 - padding * 2));
        EditorStyles.textField.wordWrap = true;
        GUIStyle centerTextS = new GUIStyle(GUI.skin.textField);
        centerTextS.alignment = TextAnchor.MiddleCenter;
        fmod = EditorGUILayout.Foldout(fmod, "FMOD");
        GUILayout.EndArea();
        

        if (fmod)
        {
            rect.height = defaultHeight;
            rect.y = rect.y + height + 15;


            GUI.Box(rect, title, style);


            GUILayout.BeginArea(new Rect(rect.position.x + padding, rect.position.y + padding, width - padding * 2, currentHeight - padding * 2));
            // Make text inside textArea wrap when reaching edge
            EditorStyles.textField.wordWrap = true;


            GUIStyle centerTextStyle = new GUIStyle(GUI.skin.textField);
            centerTextStyle.alignment = TextAnchor.MiddleCenter;
            /* Specify contents of node here */

            EditorGUILayout.LabelField("Day Select:");
            DaySelect = EditorGUILayout.IntPopup(DaySelect, Days, DayNR);
            EditorGUILayout.LabelField("Character Select:");
            CharSelect = EditorGUILayout.IntPopup(CharSelect, Char, CharNR);
            EditorGUILayout.LabelField("Dialogue Select:");
            DiaSelect = EditorGUILayout.IntPopup(DiaSelect, Dia1, DiaNR);
            EditorGUILayout.LabelField("Voice clip Select:");
            VoxSelect = EditorGUILayout.IntPopup(VoxSelect, Voxs, VoxNR);

            GUILayout.EndArea();
        }
    }
    

    public int setDay()
    {
        return DaySelect;
    }

    public int setChar()
    {
        return CharSelect;
    }

    public int setClip()
    {
        return VoxSelect;
    }

    public int setDialogue()
    {
        return DiaSelect;
    }

    public int getBool()
    {
        if (fmod)
            return 1;
        else
            return 0;
    }

    public int getDay()
    {
        return DaySelect;
    }

}
#endif
