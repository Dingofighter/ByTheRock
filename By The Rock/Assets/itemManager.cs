using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class itemManager : MonoBehaviour {

    public Image img1, img2, img3, img4;
    Image[] images;

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
        texts = new Text[] { text1, text2, text3, text4 };

        imgItems = new Sprite[] { imgItem0, imgItem1, imgItem2, imgItem3, imgItem4, imgItem5, imgItem6, imgItem7, imgItem8};
        imgTexts = new string[] { textItem0, textItem1, textItem2, textItem3, textItem4, textItem5, textItem6, textItem7, textItem8};

        invBars = new Sprite[] { invBar1, invBar2, invBar3 };

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
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void addItem(int slot, int itemID)
    {
        if (itemID == -1)
        {
            images[slot].gameObject.SetActive(false);
            texts[slot].text = "";
        }
        else
        {
            images[slot].gameObject.SetActive(true);
            images[slot].sprite = imgItems[itemID];
            texts[slot].text = imgTexts[itemID];
            texts[slot].fontSize = 40 - texts[slot].text.Length;
        }
    }
}
