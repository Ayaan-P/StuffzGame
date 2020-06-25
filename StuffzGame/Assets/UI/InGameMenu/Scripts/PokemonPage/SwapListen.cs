using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SwapListen : MonoBehaviour, IPointerClickHandler
{
    public Sprite highlightedSprite;
    public int? SwapWithIndex { get; set; }
    private Sprite originalSprite;
    private Color originalColor;

    private void Start()
    {
        Image border = this.transform.GetComponentsInChildren<Image>()[0];
        originalSprite = border.sprite;
        originalColor = border.color;

        // disable at start. Only enable when some other slot clicks "swap" button
        ToggleCurrentState(false, null);
    }

    public void ToggleCurrentState(bool toggle, int? swapWithIndex)
    {
        SwapWithIndex = swapWithIndex;
        ToggleHighlightedState(toggle);
        SetSlotState(toggle);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int currentSiblingIndex = this.transform.GetSiblingIndex();
        if (SwapWithIndex != null)
        {
            int swapWithIndex = (int)SwapWithIndex;
            GameObject toSwapWith = this.transform.parent.GetChild(swapWithIndex).gameObject;
            SwapClick slot = toSwapWith.GetComponent<SwapClick>();
            slot.ToggleClickedSwapButtonAndOtherHighlightedStates(false, swapWithIndex);
            //reset this slot back to how it was
            ToggleCurrentState(false, null);

            SwapSlots(currentSiblingIndex, swapWithIndex);

        }
    }

    private void SwapSlots(int currentIndex, int toIndex)
    {
        this.transform.parent.GetChild(toIndex).SetSiblingIndex(currentIndex);
        this.transform.SetSiblingIndex(toIndex);
        Player player = Player.Instance;
        if (player != null)
        {
            player.Party.Swap(currentIndex, toIndex);
        }
        else
        {
            Debug.LogError($"Cant swap pokemon at index {currentIndex} to index {toIndex}, because Player is null");
        }
    }

    private void SetSlotState(bool toggle)
    {
        this.enabled = toggle;
    }

    private void ToggleHighlightedState(bool toggle)
    {
        Button swapButton = this.transform.GetComponentInChildren<Button>(true);
        DraggablePokemonSlot draggable = this.transform.GetComponentInChildren<DraggablePokemonSlot>(true);

        if ( draggable != null)
        {
            draggable.enabled = !toggle;
        }
        else
        {
            Debug.LogError($"Draggable is null");
        }

        if (swapButton != null)
        {
            swapButton.transform.gameObject.SetActive(!toggle);
        }
        else
        {
            Debug.LogError($"Swap button is null");
        }

        HighlightSlot(toggle);
    }

    private void HighlightSlot(bool toggle)
    {
        Image border = this.transform.GetComponentsInChildren<Image>()[0];

        if (border == null)
        {
            Debug.LogError($"Slot border sprite is null");

        }
        else
        {
            if (toggle)
            {
                border.sprite = highlightedSprite;
                border.color = ColorPalette.GetColor(ColorName.PRIMARY_RED);
            }
            else
            {
                border.sprite = originalSprite;
                border.color = originalColor;
            }
        }
    }
}