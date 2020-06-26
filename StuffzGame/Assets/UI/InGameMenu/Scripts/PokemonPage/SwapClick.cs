using System;
using UnityEngine;
using UnityEngine.UI;

public class SwapClick : MonoBehaviour
{
    private UnityEngine.UI.Button swapButton;
    private GameObject swapText;
    // Start is called before the first frame update
    private void Start()
    {
        int numSiblings = this.transform.parent.childCount;
        this.swapText = this.transform.parent.parent.Find("SwapText").gameObject;
        this.swapButton = GetComponentInChildren<UnityEngine.UI.Button>(true);
        if (swapButton == null)
        {
            Debug.LogError("PokemonPartySlot prefab has no Swap Button to listen to!");
        }
        else if (numSiblings == 1)
        {
            this.swapButton.enabled = false;
        }
        else
        {
            this.swapButton.enabled = true;
            this.swapButton.onClick.AddListener(CheckIfAnotherSlotClicked);
        }
    }

    private void CheckIfAnotherSlotClicked()
    {
        int currentSiblingIndex = this.transform.GetSiblingIndex();
        Debug.Log($"Clicked Swap button at {currentSiblingIndex}");
        ToggleClickedSwapButtonAndOtherHighlightedStates(true, currentSiblingIndex);
    }

    public void ToggleClickedSwapButtonAndOtherHighlightedStates(bool toggle, int currentSiblingIndex)
    {
        swapText.SetActive(toggle);
        ToggleDraggable(!toggle);
        HighlightOtherSlots(toggle, currentSiblingIndex);

    }

    private void HighlightOtherSlots(bool toggle, int currentSiblingIndex)
    {
        for (int i = 0; i < this.transform.parent.childCount; i++)
        {
            var child = this.transform.parent.GetChild(i);
            if (!child.transform.Equals(this.transform))
            {
                SwapListen slotToListen = child.GetComponentInChildren<SwapListen>(true);
                slotToListen.ToggleCurrentState(toggle, currentSiblingIndex);
            }
        }
    }

    private void ToggleDraggable(bool toggle)
    {
        DraggablePokemonSlot draggable = this.transform.GetComponentInChildren<DraggablePokemonSlot>(true);
        if (draggable != null)
        {
            draggable.enabled = toggle;
        }
    }
}