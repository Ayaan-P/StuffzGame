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

    private UIManager uiManager;
    private PokemonStorage storage;

    private void OnEnable()
    {
        uiManager = UIManager.Instance;
        storage = PokemonStorage.GetInstance();
        ResetPage();
        SetUpListeners(true);
        PopulateSortByDropdown();
        UpdateStorageUI();
    }

    private void OnDisable()
    {
        SetUpListeners(false);
    }

    private void ResetPage()
    {
        isSortAscending = true;
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

        foreach (PokemonSlotSpriteData storageSlotData in slotDataList)
        {
            GameObject storageSlot = Instantiate(storagePokemonSlot, storageSlots.transform);
            SetStoragePokemonSlotDetails(storageSlotData, storageSlot);
        }
    }

    private void SetStoragePokemonSlotDetails(PokemonSlotSpriteData slotData, GameObject storageSlot)
    {
        Pokemon pokemon = slotData.CurrentObject;
        Image[] imageComponents = storageSlot.GetComponentsInChildren<Image>();
        Image pokemonImage = imageComponents[2];
        Image heldItemImage = imageComponents[3];

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

        foreach (PokemonSlotSpriteData storageSlot in uiManager.StorageSlotDataList)
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
    Level,
    Nature,
    Gender,
    IsShiny,
    Fainted,
    Height,
    Weight
}