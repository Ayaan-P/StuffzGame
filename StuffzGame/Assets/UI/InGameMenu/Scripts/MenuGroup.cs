using System.Collections.Generic;
using UnityEngine;

public class MenuGroup : MonoBehaviour
{
    private List<MenuButton> menuButtons;
    public List<GameObject> menuPages;

    public void SubscribeButton(MenuButton button)
    {
        if (menuButtons == null)
        {
            menuButtons = new List<MenuButton>();
        }
        menuButtons.Add(button);
    }

    public void OnTabEnter(MenuButton button) { }
    public void OnTabSelected(MenuButton button) {
        int buttonIndex = button.transform.GetSiblingIndex();

        for(int pageIndex = 0; pageIndex < menuPages.Count; pageIndex++)
        {
            if (pageIndex == buttonIndex)
            {
                menuPages[pageIndex].SetActive(true);
            }
            else
            {
                menuPages[pageIndex].SetActive(false);

            }
        }
    }
    public void OnTabExit(MenuButton button) { }


}