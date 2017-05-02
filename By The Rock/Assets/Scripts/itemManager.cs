using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class itemManager : MonoBehaviour {

    public Image img1;
    public Image img2;
    public Image img3;
    public Image img4;
    Image[] images;
    public Text text1;
    public Text text2;
    public Text text3;
    public Text text4;
    Text[] texts;

    public Sprite imgItem0;
    public Sprite imgItem1;
    Sprite[] imgItems;

    public string textItem0;
    public string textItem1;
    string[] imgTexts;

    // Use this for initialization
    void Start () {
        
        images = new Image[] { img1, img2, img3, img4 };
        texts = new Text[] { text1, text2, text3, text4 };

        imgItems = new Sprite[] { imgItem0, imgItem1 };
        imgTexts = new string[] { textItem0, textItem1 };

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
        }
    }
}
