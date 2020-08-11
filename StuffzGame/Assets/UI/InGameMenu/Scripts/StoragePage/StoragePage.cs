using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StoragePage : MonoBehaviour
{

    public GameObject partySlots;
    public GameObject pokemonSlot;
    public GameObject healthBar;

    public GameObject storageSlots;
    public GameObject storagePokemonSlot;

    public GameObject sortDirection;
    private bool isSortAscending;
    public Sprite ascendingSortSprite;
    public Sprite descendingSortSprite;

    public GameObject sortByDropdown;
    public GameObject searchInputField;

    public GameObject pokemonDescription;

    private UIManager uiManager;
    private PokemonStorage storage;
    private int? prevPokemonSelectedIndex;

    private void OnEnable()
    {
        storage = PokemonStorage.GetInstance();
        ResetPage();
        SetUpListeners(true);
        PopulateSortByDropdown();

        uiManager = UIManager.Instance;
        if (uiManager == null)
        {
            Debug.LogError($"{typeof(StoragePage)}: UIManager is null");
        }
        else
        {
            UpdateStorageUI();
        }
    }

       private void OnDisable()
    {
        SetUpListeners(false);
    }

    private void ResetPage()
    {
        isSortAscending = true;
        ResetSelectedSlot();
    }

    private void ResetSelectedSlot()
    {
        prevPokemonSelectedIndex = null;
        ResetPokemonDescriptionBox();
    }

    private void ResetPokemonDescriptionBox()
    {
        if (pokemonDescription != null)
        {
            pokemonDescription.SetActive(false);
        }
        else
        {
            Debug.LogError("Cant reset Pokemon Description box, because it is null");
        }
    }

    internal void OnPokemonSlotClick(int pokemonSlotIndex)
    {
        if (prevPokemonSelectedIndex != null && pokemonSlotIndex != prevPokemonSelectedIndex)
        {
            storageSlots.transform.GetChild((int)prevPokemonSelectedIndex).GetComponent<StoragePokemonClick>().ResetStorageSlotBGSprite();
        }
        StorageSlotSpriteData selectedSlotData = uiManager.StorageSlotDataList[pokemonSlotIndex] as StorageSlotSpriteData;
        UpdatePokemonDescription(selectedSlotData);
        prevPokemonSelectedIndex = pokemonSlotIndex;
    }

    private void UpdatePokemonDescription(StorageSlotSpriteData selectedSlotData)
    {
        if(pokemonDescription!= null)
        {
            pokemonDescription.SetActive(true);
            Image[] imageComponents = pokemonDescription.GetComponentsInChildren<Image>(true);
            Image pokemonImage = imageComponents[1];
            Image shinyImage = imageComponents[2];
            Image genderImage = imageComponents[3];
            Image type1Image = imageComponents[4];
            Image type2Image = imageComponents[5];
            Image heldItemImage = imageComponents[6];

            Pokemon pokemon = selectedSlotData.CurrentObject;
            pokemonImage.sprite = selectedSlotData.SummarySprite;
            pokemonImage.preserveAspect = true;

            if (pokemon.IsShiny)
            {
                shinyImage.gameObject.SetActive(true);
            }
            else
            {
                shinyImage.gameObject.SetActive(false);
            }

            genderImage.sprite = selectedSlotData.GenderSprite;
            genderImage.preserveAspect = true;

            if (pokemon.BasePokemon.Types.Count == Pokemon.MAX_POKEMON_TYPES)
            {
                type1Image.sprite = selectedSlotData.TypeSpriteList[0];
                type1Image.preserveAspect = true;
                type2Image.gameObject.SetActive(true);
                type2Image.sprite = selectedSlotData.TypeSpriteList[1];
                type2Image.preserveAspect = true;
            }
            else
            {
                type1Image.sprite = selectedSlotData.TypeSpriteList[0];
                type1Image.preserveAspect = true;
                type2Image.gameObject.SetActive(false);
            }

            if (pokemon.HeldItem != null)
            {
                heldItemImage.gameObject.SetActive(true);
                heldItemImage.sprite = selectedSlotData.ItemSprite;
                heldItemImage.preserveAspect = true;
            }
            else
            {
                heldItemImage.gameObject.SetActive(false);
            }

            Text[] textComponents = pokemonDescription.GetComponentsInChildren<Text>(true);
            Text pokemonName = textComponents[0];
            Text pokemonLevel = textComponents[1];
            Text heldItemName = textComponents[3];
            Text heldItemFlavorText = textComponents[4];
            Text abilityName = textComponents[6];
            Text abilityDesc = textComponents[7];
            Text pokemonNature = textComponents[9];

            pokemonName.text = pokemon.Nickname ?? UIUtils.FormatText(pokemon.BasePokemon.Name, true);
            pokemonLevel.text = $"Lv. {pokemon.CurrentLevel}";
            Item heldItem = pokemon.HeldItem;
            if (heldItem != null)
            {
                if (pokemon.HeldItem.IsMachine)
                {
                    string tmFullName = $"{heldItem.Name.ToUpper()} - {UIUtils.FormatText((heldItem as Machine).MoveName, false)}";
                    heldItemName.text = tmFullName;
                }
                else
                {
                    heldItemName.text = UIUtils.FormatText(heldItem.Name, false);
                }

                if (heldItem.FlavorText != null)
                {
                    heldItemFlavorText.text = heldItem.FlavorText.Replace("\n", " ");
                }
                else
                {
                    heldItemFlavorText.text = "";
                }
            }
            else
            {
                heldItemName.text = "None";
                heldItemFlavorText.text = "";
            }

            PokemonAbility ability = pokemon.CurrentAbility;
            abilityName.text = UIUtils.FormatText(ability.BaseAbility.Name, false);
            if (ability.BaseAbility.FlavorText != null)
            {
                abilityDesc.text = ability.BaseAbility.FlavorText.Replace("\n", " ");
            }
            else
            {
                abilityDesc.text = "";
            }

            pokemonNature.text = UIUtils.FormatText(pokemon.Nature.Name.ToString(), false);


        }
        else
        {
            Debug.LogError("Pokemon description box is null");
        }
    }

    private void UpdateStorageUI()
    {
        UpdatePartyPokemonSlots();
        UpdateStoragePokemonSlots(uiManager.StorageSlotDataList);
    }

    private void UpdatePartyPokemonSlots()
    {
        PartyUISection partyUISection = this.GetComponent<PartyUISection>();

        if (partyUISection == null)
        {
            Debug.LogError($"{nameof(PartyUISection)} script not attached!");
            return;
        }
        else
        {
            foreach (Transform child in partySlots.transform)
            {
                Destroy(child.gameObject);
            }

            foreach (PokemonSlotSpriteData pokemonSlotData in uiManager.PartySlotDataList)
            {
                GameObject pSlot = Instantiate(pokemonSlot, partySlots.transform);
                partyUISection.SetPokemonSlotDetails(pokemonSlotData, pSlot, healthBar);
            }
        }
    }

    private void UpdateStoragePokemonSlots(List<SpriteSlotData<Pokemon>> slotDataList)
    {
        foreach (Transform child in storageSlots.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (StorageSlotSpriteData storageSlotData in slotDataList)
        {
            GameObject storageSlot = Instantiate(storagePokemonSlot, storageSlots.transform);
            StoragePokemonClick storageSlotClick = storageSlot.GetComponent<StoragePokemonClick>();
            storageSlotClick.storage = this;
            SetStoragePokemonSlotDetails(storageSlotData, storageSlot);
        }
    }

    private void SetStoragePokemonSlotDetails(StorageSlotSpriteData slotData, GameObject storageSlot)
    {
        Pokemon pokemon = slotData.CurrentObject;
        Image[] imageComponents = storageSlot.GetComponentsInChildren<Image>();
        Image pokemonImage = imageComponents[2];
        Image heldItemImage = imageComponents[3];

        pokemonImage.sprite = slotData.PokemonSprite;
        pokemonImage.preserveAspect = true;

        if (pokemon.HeldItem != null)
        {
            heldItemImage.gameObject.SetActive(true);
            heldItemImage.sprite = slotData.ItemSprite;
            heldItemImage.preserveAspect = true;
        }
        else
        {
            heldItemImage.gameObject.SetActive(false);
        }
    }

    private void SetUpListeners(bool toggle)
    {
        Button sortDirectionButton = sortDirection.GetComponent<Button>();
        InputField searchField = searchInputField.GetComponent<InputField>();
        Dropdown sortDropdown = sortByDropdown.GetComponent<Dropdown>();

        if (toggle)
        {
            sortDirectionButton.onClick.AddListener(ChangeSortDirection);
            searchField.onValueChanged.AddListener(SearchPokemon);
            sortDropdown.onValueChanged.AddListener(SortStorageBy);
        }
        else
        {
            sortDirectionButton.onClick.RemoveAllListeners();
            searchField.onValueChanged.RemoveAllListeners();
            sortDropdown.onValueChanged.RemoveAllListeners();
        }
       
    }

    private void PopulateSortByDropdown()
    {
        List<string> options = new List<string>
        {
            "Sort By"
        };
        options.AddRange(Enum.GetNames(typeof(PokemonSortBy)).ToList());
        Dropdown sortBy = sortByDropdown.GetComponent<Dropdown>();
        sortBy.ClearOptions();
        sortBy.AddOptions(options);
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

        storage.Reverse();
        UpdateStoragePokemonSlots(uiManager.StorageSlotDataList);
    }

    private void SearchPokemon(string input)
    {
        List<SpriteSlotData<Pokemon>> searchResults = new List<SpriteSlotData<Pokemon>>();

        foreach (StorageSlotSpriteData storageSlot in uiManager.StorageSlotDataList)
        {
            if (CultureInfo.CurrentCulture.CompareInfo.IndexOf(storageSlot.CurrentObject.BasePokemon.Name, input, CompareOptions.IgnoreCase) >= 0)
            {
                searchResults.Add(storageSlot);
            }
        }

        if (searchResults.Count != 0)
        {
            UpdateStoragePokemonSlots(searchResults);
        }
    }

    private void SortStorageBy(int index)
    {
        Dropdown sortDropdown = sortByDropdown.GetComponent<Dropdown>();
        string selectedOption = sortDropdown.options[index].text;
        if (Enum.TryParse<PokemonSortBy>(selectedOption, true, out PokemonSortBy option))
        {
            storage.SortBy(option);
            UpdateStoragePokemonSlots(uiManager.StorageSlotDataList);
        }
        else
        {
            Debug.LogWarning($"{option} is not a member of enum PokemonSortBy");
        }
    }
}
public enum PokemonSortBy
{
    Name,
    ID,
    Level,
    Nature,
    Gender,
    IsShiny,
    Fainted,
    Height,
    Weight
}