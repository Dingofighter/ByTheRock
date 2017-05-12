using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ButtonAnimation : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float widthIncrease = 0;
    public float heightIncrease = 10;

    Animator animator;

    float startWidth;
    float startHeight;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        startWidth = GetComponent<RectTransform>().rect.width;
        startHeight = GetComponent<RectTransform>().rect.height;
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        animator.SetBool("Hovering", true);
        GetComponent<RectTransform>().sizeDelta = new Vector2(startWidth + widthIncrease, startHeight + heightIncrease);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        animator.SetBool("Hovering", false);
        GetComponent<RectTransform>().sizeDelta = new Vector2(startWidth, startHeight);
    }
}
