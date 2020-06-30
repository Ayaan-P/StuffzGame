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
        SetUpListeners();
        ResetPage();
        uiManager = UIManager.Instance;
        UpdateInventoryUI();
        PopulateSortByDropdown();
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
            string tmFullName = $"{currentItem.Name.ToUpper()} - {FormatText((currentItem as Machine).MoveName, false)}";
            itemName.text = tmFullName;
        }
        else
        {
            itemName.text = FormatText(currentItem.Name, false);
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

    private void SetPokemonSlotDetails(PartySlotSpriteData pokemonSlotData, GameObject pSlot)
    {
        SetPokemonTextComponents(pokemonSlotData, pSlot);
        SetPokemonImageComponents(pokemonSlotData, pSlot);
        SetPokemonHPDetails(pokemonSlotData.CurrentObject, pSlot);
    }

    private void SetPokemonTextComponents(PartySlotSpriteData pokemonSlotData, GameObject pSlot)
    {
        Pokemon pokemon = pokemonSlotData.CurrentObject;
        Text[] textComponents = pSlot.GetComponentsInChildren<Text>();

        Text pokemonName = textComponents[0];
        Text pokemonLevel = textComponents[1];
        Text isAbleText = textComponents[3];
        isAbleText.text = "";
        pokemonName.text = FormatText(pokemon.BasePokemon.Name, true);
        pokemonLevel.text = $"Lv. {pokemon.CurrentLevel}";
    }

    private void SetPokemonImageComponents(PartySlotSpriteData slotData, GameObject slot)
    {
        int MAX_TYPES_COUNT = 2;
        Pokemon pokemon = slotData.CurrentObject;
        Image[] imageComponents = slot.GetComponentsInChildren<Image>();
        Image pokemonImage = imageComponents[1];
        Image heldItemImage = imageComponents[2];
        Image statusImage = imageComponents[3];
        Image genderImage = imageComponents[4];
        Image type1 = imageComponents[5];
        Image type2 = imageComponents[6];

        pokemonImage.sprite = slotData.PokemonSprite;

        if (pokemon.HeldItem != null)
        {
            heldItemImage.sprite = slotData.ItemSprite;
            heldItemImage.preserveAspect = true;
        }
        else
        {
            heldItemImage.color = new Color(0, 0, 0, 0);
        }

        if (pokemon.IsFainted)
        {
            statusImage.sprite = slotData.FaintedSprite;
            statusImage.preserveAspect = true;
        }
        else
        {
            statusImage.color = new Color(0, 0, 0, 0);
        }

        genderImage.sprite = slotData.GenderSprite;
        genderImage.preserveAspect = true;

        if (pokemon.BasePokemon.Types.Count == MAX_TYPES_COUNT)
        {
            type1.sprite = slotData.TypeSpriteList[0];
            type1.preserveAspect = true;
            type2.sprite = slotData.TypeSpriteList[1];
            type2.preserveAspect = true;
        }
        else
        {
            type1.sprite = slotData.TypeSpriteList[0];
            type1.preserveAspect = true;
            type2.sprite = slotData.TypeSpriteList[0];
            type2.preserveAspect = true;
            type2.color = new Color(0, 0, 0, 0);
        }
    }

    private void SetPokemonHPDetails(Pokemon pokemon, GameObject slot)
    {
        Text pokemonHP = slot.GetComponentsInChildren<Text>()[2];

        PokemonStat hpStat = pokemon.BasePokemon.Stats.Where(stat => stat.BaseStat.Name == StatName.HP).SingleOrDefault();
        if (hpStat != null)
        {
            pokemonHP.text = $"{hpStat.CurrentValue}/{hpStat.CalculatedValue}";
        }

        Color hpColor = GetColorForHP(hpStat.CurrentValue, hpStat.CalculatedValue);

        // set HP slider via HealthBar prefab
        GameObject health = slot.transform.Find("Health").gameObject;
        GameObject hpBar = Instantiate(healthBar, health.transform);
        Image fill = hpBar.GetComponentsInChildren<Image>()[1];
        fill.color = hpColor;
        Slider hpSlider = hpBar.GetComponent<Slider>();
        hpSlider.minValue = 0;
        hpSlider.maxValue = hpStat.CalculatedValue;
        hpSlider.value = hpStat.CurrentValue;
    }

    private Color GetColorForHP(int currentHP, int maxHP)
    {
        float FIFTY_PERCENT = 0.5f;
        float TEN_PERCENT = 0.1f;

        float hpPercent = currentHP / (float)maxHP;

        if (hpPercent >= FIFTY_PERCENT)
        {
            return ColorPalette.GetColor(ColorName.PRIMARY_GREEN);
        }
        else if (hpPercent > TEN_PERCENT && hpPercent < FIFTY_PERCENT)
        {
            return ColorPalette.GetColor(ColorName.PRIMARY_YELLOW);
        }
        //HP less than 10%
        else
        {
            return ColorPalette.GetColor(ColorName.PRIMARY_RED);
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
            PartySlotSpriteData slotData = partySlotDataList[i] as PartySlotSpriteData;
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
                string tmFullName = $"{item.Name.ToUpper()} - {FormatText((item as Machine).MoveName, false)}";
                itemName.text = tmFullName;
            }
            else
            {
                itemName.text = FormatText(item.Name, false);
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

    private string FormatText(string str, bool keepDashes)
    {
        string str1 = (keepDashes) ? str : str.Replace('-', ' ');
        // capitalize every word
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str1.ToLower());
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
        List<string> options = new List<string>();
        options.Add("Sort By");
        options.AddRange(Enum.GetNames(typeof(SortBy)).ToList());
        Dropdown sortBy = sortByDropdown.GetComponent<Dropdown>();
        //Destroy(sortByDropdown.GetComponentInChildren<Canvas>().gameObject);
        sortBy.ClearOptions();
        sortBy.AddOptions(options);
    }

    private void SetUpListeners()
    {
        Button sortDirectionButton = sortDirection.GetComponent<Button>();
        sortDirectionButton.onClick.AddListener(ChangeSortDirection);

        InputField searchField = searchInputField.GetComponent<InputField>();
        searchField.onValueChanged.AddListener(SearchItems);

        Dropdown sortDropdown = sortByDropdown.GetComponent<Dropdown>();
        sortDropdown.onValueChanged.AddListener(SortPocketBy);
    }

    private void SortPocketBy(int index)
    {

        Dropdown sortDropdown = sortByDropdown.GetComponent<Dropdown>();
        string selectedOption = sortDropdown.options[index].text;
        if (Enum.TryParse<SortBy>(selectedOption, true, out SortBy option))
        {
            switch (option)
            {
                case SortBy.Name:
                    pocketItemsDict[CurrentItemPocket].Sort((a, b) => a.CurrentObject.Name.CompareTo(b.CurrentObject.Name));
                    UpdateCurrentItemPocketData(pocketItemsDict[CurrentItemPocket]);
                    break;

                case SortBy.Cost:
                    pocketItemsDict[CurrentItemPocket].Sort((a, b) => a.CurrentObject.Cost.CompareTo(b.CurrentObject.Cost));
                    UpdateCurrentItemPocketData(pocketItemsDict[CurrentItemPocket]);
                    break;

                case SortBy.Count:
                    pocketItemsDict[CurrentItemPocket].Sort((a, b) => Nullable.Compare(a.CurrentObject.Count, b.CurrentObject.Count));
                    UpdateCurrentItemPocketData(pocketItemsDict[CurrentItemPocket]);
                    break;

                default:
                    Debug.LogWarning($"{option} is not a member of enum SortBy for this dropdown");
                    break;
            }
        }
        else
        {
            Debug.LogWarning($"{option} is not a member of enum SortBy for this dropdown");
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

    private enum SortBy
    {
        Name,
        Cost,
        Count
    }
}