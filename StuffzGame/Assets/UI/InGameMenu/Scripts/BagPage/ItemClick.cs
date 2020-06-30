using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ItemClick : MonoBehaviour,IPointerClickHandler
{
    public Sprite highlightSprite;
    private Sprite itemBg;
    public BagPage bag;
    // Start is called before the first frame update
    private void Start()
    {
        itemBg = this.GetComponentsInChildren<Image>()[0].sprite;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (bag != null)
        {
            this.GetComponentsInChildren<Image>()[0].sprite = highlightSprite;
            bag.OnItemClick(this.transform.GetSiblingIndex());

        }
        else
        {
            Debug.LogError($"Item was clicked but not assigned BagPage is null");
        }
    }

    public void ResetItemBGSprite()
    {
        if (this.itemBg != null)
        {
            this.GetComponentsInChildren<Image>()[0].sprite = itemBg;
        }
        else
        {
            Debug.LogError("Selected Item has no Image component for BG");
        }
    }
}
