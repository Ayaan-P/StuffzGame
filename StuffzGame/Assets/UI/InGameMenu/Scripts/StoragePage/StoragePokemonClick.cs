using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StoragePokemonClick : MonoBehaviour, IPointerClickHandler
{
    public Sprite highlightSprite;
    private Sprite originalSlotBg;
    public StoragePage storage;
    private GameObject group;

    // Start is called before the first frame update
    private void Start()
    {
        group = this.transform.Find("Group").gameObject;
        originalSlotBg = group.GetComponentsInChildren<Image>()[0].sprite;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (storage != null)
        {
            group.GetComponentsInChildren<Image>()[0].sprite = highlightSprite;
            storage.OnPokemonSlotClick(this.transform.GetSiblingIndex());

        }
        else
        {
            Debug.LogError($"Storage Pokemon Slot was clicked but assigned StoragePage is null");
        }
    }

    public void ResetStorageSlotBGSprite()
    {
        if (this.originalSlotBg != null)
        {
            group.GetComponentsInChildren<Image>()[0].sprite = originalSlotBg;
        }
        else
        {
            Debug.LogError("Selected Pokemon Storage slot has no Image component for BG");
        }
    }
}
