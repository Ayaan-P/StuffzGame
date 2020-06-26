using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemClick : MonoBehaviour,IPointerClickHandler
{
    public Sprite highlightSprite;
    private Image itemBg;
    private int currentIndex;
    // Start is called before the first frame update
    void Start()
    {
        currentIndex = this.transform.GetSiblingIndex();
        itemBg = this.GetComponentsInChildren<Image>()[0];
    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }
}
