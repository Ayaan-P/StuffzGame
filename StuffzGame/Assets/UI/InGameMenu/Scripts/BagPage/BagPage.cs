using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BagPage : MonoBehaviour
{
    public GameObject itemSlots;
    public GameObject pokemonSlots;
    public GameObject pokemonSlot;
    public GameObject itemSlot;
    public GameObject healthBar;
    public GameObject sortDirection;
    private bool isSortAscending;
    public Sprite ascendingSortSprite;
    public Sprite descendingSortSprite;

    public GameObject sortByDropdown;
    public GameObject searchInputField;
    public GameObject descriptionData;

    private UIManager uiManager;
    private List<ItemPocketTab> pocketTabs;
    public ItemPocket CurrentItemPocket { get; private set; } = ItemPocket.MISC;
    private Dictionary<ItemPocket, List<ItemSlotSpriteData>> pocketItemsDict;
    private int? prevItemSelectedIndex;

    private void OnEnable()
    {
        SetUpListeners(true);
        ResetPage();
        uiManager = UIManager.Instance;
        UpdateInventoryUI();
        PopulateSortByDropdown();
    }

    private void OnDisable()
    {
        SetUpListeners(false);
    }

    private void UpdateInventoryUI()
    {
        UpdatePokemonSlots();
        UpdateItemSlots();
    }

    private void UpdateItemSlots()
    {
        foreach (ItemSlotSpriteData itemSlotData in uiManager.ItemSlotDataList)
        {
            PopulateRespectiveItemPocket(itemSlotData);
            UpdateCurrentItemPocketData(pocketItemsDict[CurrentItemPocket]);
        }
    }

    private void UpdatePokemonSlots()
    {
        PartyUISection partyUISection = this.GetComponent<PartyUISection>();
        
        if(partyUISection== null)
        {
            Debug.LogError($"{nameof(PartyUISection)} script not attached!");
            return;
        }
        else
        {
            foreach (Transform child in pokemonSlots.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (PokemonSlotSpriteData pokemonSlotData in uiManager.PartySlotDataList)
            {
                GameObject pSlot = Instantiate(pokemonSlot, pokemonSlots.transform);
                partyUISection.SetPokemonSlotDetails(pokemonSlotData, pSlot, healthBar);
            }
        }
    }

    public void UpdateCurrentItemPocketData(List<ItemSlotSpriteData> itemSlotDataList)
    {
        foreach (Transform child in itemSlots.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (ItemSlotSpriteData currentPocketSlotData in itemSlotDataList)
        {
            GameObject iSlot = Instantiate(itemSlot, itemSlots.transform);
            ItemClick itemClick = iSlot.GetComponent<ItemClick>();
            itemClick.bag = this;
            SetItemSlotDetails(currentPocketSlotData, iSlot);
        }
    }

    private void PopulateRespectiveItemPocket(ItemSlotSpriteData itemSlotData)
    {
        switch (itemSlotData.CurrentObject.Pocket)
        {
            case ItemPocket.MISC: pocketItemsDict[ItemPocket.MISC].Add(itemSlotData); break;
            case ItemPocket.MEDICINE: pocketItemsDict[ItemPocket.MEDICINE].Add(itemSlotData); break;
            case ItemPocket.POKEBALLS: pocketItemsDict[ItemPocket.POKEBALLS].Add(itemSlotData); break;
            case ItemPocket.MACHINES: pocketItemsDict[ItemPocket.MACHINES].Add(itemSlotData); break;
            case ItemPocket.BERRIES: pocketItemsDict[ItemPocket.BERRIES].Add(itemSlotData); break;
            case ItemPocket.BATTLE: pocketItemsDict[ItemPocket.BATTLE].Add(itemSlotData); break;
            case ItemPocket.KEY: pocketItemsDict[ItemPocket.KEY].Add(itemSlotData); break;
            default:
                Debug.LogWarning($"{itemSlotData.CurrentObject.Name} with undisplayed pocket: {itemSlotData.CurrentObject.Pocket} found. Putting in MISC ItemPocket");
                pocketItemsDict[ItemPocket.MISC].Add(itemSlotData);
                break;
        }
    }

    private void SetItemSlotDetails(ItemSlotSpriteData itemSlotData, GameObject iSlot)
    {
        Image icon = iSlot.GetComponentsInChildren<Image>()[1];
        Text[] textComponents = iSlot.GetComponentsInChildren<Text>();
        Text itemName = textComponents[0];
        Text itemCount = textComponents[1];

        Item currentItem = itemSlotData.CurrentObject;

        Sprite itemSprite = itemSlotData.ItemSprite;
        if (itemSprite != null)
        {
            icon.sprite = itemSprite;
        }

        if (currentItem.IsMachine)
        {
            string tmFullName = $"{currentItem.Name.ToUpper()} - {UIUtils.FormatText((currentItem as Machine).MoveName, false)}";
            itemName.text = tmFullName;
        }
        else
        {
            itemName.text = UIUtils.FormatText(currentItem.Name, false);
        }

        if (currentItem.Count != null)
        {
            itemCount.text = $"x {currentItem.Count}";
        }
        else
        {
            itemCount.text = "  -";
        }
    }

    public void OnItemClick(int itemIndex)
    {
        if (prevItemSelectedIndex != null && itemIndex != prevItemSelectedIndex)
        {
            itemSlots.transform.GetChild((int)prevItemSelectedIndex).GetComponent<ItemClick>().ResetItemBGSprite();
        }
        ItemSlotSpriteData selectedSlotData = this.pocketItemsDict[CurrentItemPocket][itemIndex];
        UpdateItemDescription(selectedSlotData);
        SetPokemonIsAble(selectedSlotData.CurrentObject);
        prevItemSelectedIndex = itemIndex;
    }

    private void ResetPokemonAbleText()
    {
        for (int i = 0; i < pokemonSlots.transform.childCount; i++)
        {
            GameObject pSlot = pokemonSlots.transform.GetChild(i).gameObject;
            Text isAbleText = pSlot.GetComponentsInChildren<Text>()[3];
            isAbleText.text = "";
        }
    }

    public void SetPokemonIsAble(Item item)
    {
        var partySlotDataList = uiManager.PartySlotDataList;
        for (int i = 0; i < pokemonSlots.transform.childCount; i++)
        {
            PokemonSlotSpriteData slotData = partySlotDataList[i] as PokemonSlotSpriteData;
            GameObject pSlot = pokemonSlots.transform.GetChild(i).gameObject;
            Text isAbleText = pSlot.GetComponentsInChildren<Text>()[3];
            bool? canUseItem = slotData.CurrentObject.CanUseItem(item);
            if(canUseItem == null)
            {
                isAbleText.text = "";
            }
            else
            {
                isAbleText.text = (bool)canUseItem ? "ABLE" : "UNABLE";
            }
        }
    }

    private void UpdateItemDescription(ItemSlotSpriteData slotData)
    {
        if (descriptionData != null)
        {
            Item item = slotData.CurrentObject;

            descriptionData.SetActive(true);
            Image itemSprite = descriptionData.GetComponentInChildren<Image>();
            Text[] textComponents = descriptionData.GetComponentsInChildren<Text>();
            Text itemName = textComponents[0];
            Text itemFlavorText = textComponents[1];
            Text itemCost = textComponents[2];

            itemSprite.sprite = slotData.ItemSprite;
            if (item.IsMachine)
            {
                string tmFullName = $"{item.Name.ToUpper()} - {UIUtils.FormatText((item as Machine).MoveName, false)}";
                itemName.text = tmFullName;
            }
            else
            {
                itemName.text = UIUtils.FormatText(item.Name, false);
            }

            if (item.FlavorText != null)
            {
                itemFlavorText.text = item.FlavorText.Replace("\n", " ");
            }
            else
            {
                itemFlavorText.text = "";
            }

            itemCost.text = $"Cost: {item.Cost}";
        }
        else
        {
            Debug.LogError("Description box is null");
        }
    }

    private void ResetPage()
    {
        //        CurrentItemPocket = ItemPocket.MISC;
        pocketItemsDict = new Dictionary<ItemPocket, List<ItemSlotSpriteData>> {
        { ItemPocket.MISC, new List<ItemSlotSpriteData>()},
        { ItemPocket.MEDICINE, new List<ItemSlotSpriteData>()},
        { ItemPocket.POKEBALLS, new List<ItemSlotSpriteData>()},
        { ItemPocket.MACHINES, new List<ItemSlotSpriteData>()},
        { ItemPocket.BERRIES, new List<ItemSlotSpriteData>()},
        { ItemPocket.BATTLE, new List<ItemSlotSpriteData>()},
        { ItemPocket.KEY, new List<ItemSlotSpriteData>()},};
        isSortAscending = true;
        ResetSelectedItem();
        ResetPokemonAbleText();
    }

    private void ResetSelectedItem()
    {
        prevItemSelectedIndex = null;
        ResetItemDescriptionBox();
    }

    private void ResetItemDescriptionBox()
    {
        if (descriptionData != null)
        {
            descriptionData.SetActive(false);
        }
        else
        {
            Debug.LogError("Cant Reset Description box, because it is null");
        }
    }

    internal void SubscribeItemPocketTab(ItemPocketTab itemPocketTab)
    {
        if (pocketTabs == null)
        {
            pocketTabs = new List<ItemPocketTab>();
        }
        pocketTabs.Add(itemPocketTab);
    }

    internal void UnsubscribeItemPocketTab(ItemPocketTab itemPocketTab)
    {
        if (pocketTabs != null)
        {
            pocketTabs.Remove(itemPocketTab);
        }
    }

    internal void OnItemPocketTabSelected(ItemPocketTab itemPocketTab)
    {
        ItemPocket selectedPocket = itemPocketTab.ItemPocket;
        ResetSelectedItem();
        ResetPokemonAbleText();
        CurrentItemPocket = selectedPocket;
        UpdateCurrentItemPocketData(pocketItemsDict[CurrentItemPocket]);
    }

    private void PopulateSortByDropdown()
    {
        List<string> options = new List<string>
        {
            "Sort By"
        };
        options.AddRange(Enum.GetNames(typeof(ItemSortBy)).ToList());
        Dropdown sortBy = sortByDropdown.GetComponent<Dropdown>();
        sortBy.ClearOptions();
        sortBy.AddOptions(options);
    }

    private void SetUpListeners(bool toggle)
    {
        Button sortDirectionButton = sortDirection.GetComponent<Button>();
        InputField searchField = searchInputField.GetComponent<InputField>();
        Dropdown sortDropdown = sortByDropdown.GetComponent<Dropdown>();

        if (toggle)
        {
            sortDirectionButton.onClick.AddListener(ChangeSortDirection);
            searchField.onValueChanged.AddListener(SearchItems);
            sortDropdown.onValueChanged.AddListener(SortPocketBy);
        }
        else
        {
            sortDirectionButton.onClick.RemoveAllListeners();
            searchField.onValueChanged.RemoveAllListeners();
            sortDropdown.onValueChanged.RemoveAllListeners();
        }
    }

    private void SortPocketBy(int index)
    {

        Dropdown sortDropdown = sortByDropdown.GetComponent<Dropdown>();
        string selectedOption = sortDropdown.options[index].text;
        if (Enum.TryParse<ItemSortBy>(selectedOption, true, out ItemSortBy option))
        {
            switch (option)
            {
                case ItemSortBy.Name:
                    pocketItemsDict[CurrentItemPocket].Sort((a, b) => a.CurrentObject.Name.CompareTo(b.CurrentObject.Name));
                    UpdateCurrentItemPocketData(pocketItemsDict[CurrentItemPocket]);
                    break;

                case ItemSortBy.Cost:
                    pocketItemsDict[CurrentItemPocket].Sort((a, b) => a.CurrentObject.Cost.CompareTo(b.CurrentObject.Cost));
                    UpdateCurrentItemPocketData(pocketItemsDict[CurrentItemPocket]);
                    break;

                case ItemSortBy.Count:
                    pocketItemsDict[CurrentItemPocket].Sort((a, b) => Nullable.Compare(a.CurrentObject.Count, b.CurrentObject.Count));
                    UpdateCurrentItemPocketData(pocketItemsDict[CurrentItemPocket]);
                    break;

                default:
                    Debug.LogWarning($"{option} is not a member of enum ItemSortBy for this dropdown");
                    break;
            }
        }
        else
        {
            Debug.LogWarning($"{option} is not a member of enum ItemSortBy for this dropdown");
        }
    }

    private void SearchItems(string input)
    {
        List<ItemSlotSpriteData> searchResults = new List<ItemSlotSpriteData>();
        foreach (var key in pocketItemsDict.Keys)
        {
            foreach (var itemSlot in pocketItemsDict[key])
            {
                if (CultureInfo.CurrentCulture.CompareInfo.IndexOf(itemSlot.CurrentObject.Name, input, CompareOptions.IgnoreCase) >= 0)
                {
                    searchResults.Add(itemSlot);
                }
            }
        }
        if (searchResults.Count != 0)
        {
            UpdateCurrentItemPocketData(searchResults);
        }
    }

    private void ChangeSortDirection()
    {
        Image buttonSprite = sortDirection.GetComponent<Image>();
        isSortAscending = !isSortAscending;
        if (isSortAscending)
        {
            buttonSprite.sprite = ascendingSortSprite;
        }
        else
        {
            buttonSprite.sprite = descendingSortSprite;
        }
        pocketItemsDict[CurrentItemPocket].Reverse();
        UpdateCurrentItemPocketData(pocketItemsDict[CurrentItemPocket]);
    }

    private enum ItemSortBy
    {
        Name,
        Cost,
        Count
    }
}