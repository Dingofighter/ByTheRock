using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class itemManager : MonoBehaviour {

    public Image img1, img2, img3, img4;
    Image[] images;

    public Image imgBar1, imgBar2, imgBar3, imgBar4;
    Image[] imgBars;

    public Text text1, text2, text3, text4;
    Text[] texts;

    public Sprite imgItem0, imgItem1, imgItem2, imgItem3, imgItem4, imgItem5, imgItem6, imgItem7, imgItem8;
    Sprite[] imgItems;

    public string textItem0, textItem1, textItem2, textItem3, textItem4, textItem5, textItem6, textItem7, textItem8;
    string[] imgTexts;

    public Sprite invBar1, invBar2, invBar3;
    Sprite[] invBars;

    // Use this for initialization
    void Start () {
        
        images = new Image[] { img1, img2, img3, img4 };
        imgBars = new Image[] { imgBar1, imgBar2, imgBar3, imgBar4 };

        texts = new Text[] { text1, text2, text3, text4 };

        imgItems = new Sprite[] { imgItem0, imgItem1, imgItem2, imgItem3, imgItem4, imgItem5, imgItem6, imgItem7, imgItem8};
        imgTexts = new string[] { textItem0, textItem1, textItem2, textItem3, textItem4, textItem5, textItem6, textItem7, textItem8};

        invBars = new Sprite[] { invBar1, invBar2, invBar3 };

        imgBars[0].sprite = invBars[0];
        imgBars[1].sprite = invBars[1];
        imgBars[2].sprite = invBars[2];
        imgBars[3].sprite = invBars[0];

        /*
        images[0].sprite = invBars[0];
        images[1].sprite = invBars[1];
        images[2].sprite = invBars[2];
        images[3].sprite = invBars[0];
        */

        images[0].gameObject.SetActive(false);
        images[1].gameObject.SetActive(false);
        images[2].gameObject.SetActive(false);
        images[3].gameObject.SetActive(false);

        imgBars[0].gameObject.SetActive(false);
        imgBars[1].gameObject.SetActive(false);
        imgBars[2].gameObject.SetActive(false);
        imgBars[3].gameObject.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void addItem(int itemID)
    {
        Debug.Log("adding item " + GameManager.instance.itemID1 + " " + GameManager.instance.itemID2 + " " + GameManager.instance.itemID3 + " " + GameManager.instance.itemID4);
        int slot = 0;

        if (GameManager.instance.itemID1 == -1) { slot = 0; GameManager.instance.itemID1 = itemID; Game.current.invSlot1 = itemID; }
        else if (GameManager.instance.itemID2 == -1) { slot = 1; GameManager.instance.itemID2 = itemID; Game.current.invSlot2 = itemID; }
        else if (GameManager.instance.itemID3 == -1) { slot = 2; GameManager.instance.itemID3 = itemID; Game.current.invSlot3 = itemID; }
        else if (GameManager.instance.itemID4 == -1) { slot = 3; GameManager.instance.itemID4 = itemID; Game.current.invSlot4 = itemID; }
        else return;

        Debug.Log("adding item " + itemID + " on slot " + slot);
        images[slot].gameObject.SetActive(true);
        images[slot].sprite = imgItems[itemID];
        imgBars[slot].gameObject.SetActive(true);
        texts[slot].text = imgTexts[itemID];
        texts[slot].fontSize = 40 - texts[slot].text.Length;
        
    }

    public void removeItem(int itemID)
    {

        int slot = 0;

        if (GameManager.instance.itemID1 == itemID)
        {
            GameManager.instance.itemID1 = GameManager.instance.itemID2;
            GameManager.instance.itemID2 = GameManager.instance.itemID3;
            GameManager.instance.itemID3 = GameManager.instance.itemID4;
            GameManager.instance.itemID4 = -1;

            for (int i = 0; i < 3; i++)
            {
                images[i].sprite = images[i + 1].sprite;
                texts[i].text = texts[i + 1].text;
                texts[i].fontSize = texts[i + 1].fontSize;
            }

            Game.current.invSlot1 = Game.current.invSlot2;
            Game.current.invSlot2 = Game.current.invSlot3;
            Game.current.invSlot3 = Game.current.invSlot4;
            Game.current.invSlot4 = -1;
            if (GameManager.instance.itemID1 == -1) slot = 0;
            else if (GameManager.instance.itemID2 == -1) slot = 1;
            else if (GameManager.instance.itemID3 == -1) slot = 2;
            else if (GameManager.instance.itemID4 == -1) slot = 3;

        }
        else if (GameManager.instance.itemID2 == itemID)
        {
            GameManager.instance.itemID2 = GameManager.instance.itemID3;
            GameManager.instance.itemID3 = GameManager.instance.itemID4;
            GameManager.instance.itemID4 = -1;

            for (int i = 1; i < 3; i++)
            {
                images[i].sprite = images[i + 1].sprite;
                texts[i].text = texts[i + 1].text;
                texts[i].fontSize = texts[i + 1].fontSize;
            }

            Game.current.invSlot2 = Game.current.invSlot3;
            Game.current.invSlot3 = Game.current.invSlot4;
            Game.current.invSlot4 = -1;
            if (GameManager.instance.itemID2 == -1) slot = 1;
            else if (GameManager.instance.itemID3 == -1) slot = 2;
            else if (GameManager.instance.itemID4 == -1) slot = 3;

        }
        else if (GameManager.instance.itemID3 == itemID)
        {
            GameManager.instance.itemID3 = GameManager.instance.itemID4;
            GameManager.instance.itemID4 = -1;
            
            images[2].sprite = images[3].sprite;
            texts[2].text = texts[3].text;
            texts[2].fontSize = texts[3].fontSize;
            
            Game.current.invSlot3 = Game.current.invSlot4;
            Game.current.invSlot4 = -1;
            if (GameManager.instance.itemID3 == -1) slot = 2;
            else if (GameManager.instance.itemID4 == -1) slot = 3;
        }
        else if (GameManager.instance.itemID4 == itemID) { slot = 3; GameManager.instance.itemID4 = -1; Game.current.invSlot4 = -1; }
        else return;

        Debug.Log("removing item " + itemID + " from slot " + slot);

        images[slot].gameObject.SetActive(false);
        imgBars[slot].gameObject.SetActive(false);
        texts[slot].text = "";

    }

}
