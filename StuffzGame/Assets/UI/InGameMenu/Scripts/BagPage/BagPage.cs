using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

public class BagPage : MonoBehaviour
{
    public GameObject itemSlots;
    public GameObject pokemonSlots;
    public GameObject pokemonSlot;
    public GameObject itemSlot;
    public GameObject healthBar;
    private UIManager uiManager;

    private void OnEnable()
    {
        uiManager = UIManager.Instance;
        UpdateInventoryUI();

    }

    private void UpdateInventoryUI()
    {
        UpdatePokemonSlots();
        UpdateItemSlots();
    }

    private void UpdateItemSlots()
    {
        foreach (Transform child in itemSlots.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (ItemSlotSpriteData itemSlotData in uiManager.ItemSlotDataList)
        {
            GameObject iSlot = Instantiate(itemSlot, itemSlots.transform);
            SetItemSlotDetails(itemSlotData, iSlot);
        }
    }
    private void UpdatePokemonSlots()
    {
        foreach (Transform child in pokemonSlots.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (PartySlotSpriteData pokemonSlotData in uiManager.PartySlotDataList)
        {
            GameObject pSlot = Instantiate(pokemonSlot, pokemonSlots.transform);
            SetPokemonSlotDetails(pokemonSlotData, pSlot);
        }
    }

    private void SetItemSlotDetails(ItemSlotSpriteData itemSlotData, GameObject iSlot)
    {
        Image icon = iSlot.GetComponentsInChildren<Image>()[1];
        Text[] textComponents= iSlot.GetComponentsInChildren<Text>();
        Text itemName = textComponents[0];
        Text itemCount = textComponents[1];

        Item currentItem = itemSlotData.CurrentObject;

        Sprite itemSprite = itemSlotData.ItemSprite;
        if (itemSprite != null)
        {
            icon.sprite = itemSprite;
        }
        itemName.text = FormatText(currentItem.Name, false);

        if (currentItem.Attributes.Contains(ItemAttribute.COUNTABLE)){
            itemCount.text = $"x {currentItem.Count}";
        }
        else
        {
            itemCount.text = "  -";
        }
    }

    private void SetPokemonSlotDetails(PartySlotSpriteData pokemonSlotData, GameObject pSlot)
    {
        
    }

    private string FormatText(string str, bool keepDashes)
    {
        string str1 = (keepDashes) ? str : str.Replace('-', ' ');
        // capitalize every word
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str1.ToLower());
    }
}
