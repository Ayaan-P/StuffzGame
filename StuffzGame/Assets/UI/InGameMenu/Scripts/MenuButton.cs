using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour,IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{

    public MenuGroup menuGroup;

    void OnEnable()
    {
        menuGroup.SubscribeButton(this);
    }

    void OnDisable()
    {
        menuGroup.UnsubscribeButton(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        menuGroup.OnTabSelected(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        menuGroup.OnTabEnter(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        menuGroup.OnTabExit(this);
    }
}
