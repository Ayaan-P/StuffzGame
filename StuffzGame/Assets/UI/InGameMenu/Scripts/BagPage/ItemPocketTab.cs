using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemPocketTab : MonoBehaviour,IPointerClickHandler
{
    public BagPage bag;
    [SerializeField]
    private ItemPocket itemPocket;
    public ItemPocket ItemPocket { get => itemPocket; }
    void OnEnable()
    {
        bag.SubscribeItemPocketTab(this);
    }

    void OnDisable()
    {
        bag.UnsubscribeItemPocketTab(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        bag.OnItemPocketTabSelected(this);
    }

}
