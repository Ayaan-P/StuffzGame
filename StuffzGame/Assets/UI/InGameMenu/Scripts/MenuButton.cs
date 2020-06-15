using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButton : MonoBehaviour,IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{

    public MenuGroup menuGroup;

    // Start is called before the first frame update
    void Start()
    {
        menuGroup.SubscribeButton(this);
    }

    // Update is called once per frame
    void Update()
    {
        
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
