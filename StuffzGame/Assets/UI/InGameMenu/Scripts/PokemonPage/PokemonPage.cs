using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PokemonPage : MonoBehaviour
{
    public GameObject partySlots;
    public GameObject pokemonSlot;
    public GameObject healthBar;
    private UIManager uiManager;
     
    private void OnEnable()
    {
        uiManager = UIManager.Instance;
        GameObject swapText = this.transform.Find("SwapText").gameObject;
        swapText.SetActive(false);
        UpdatePartyUI();
    }

    private void UpdatePartyUI()
    {

        foreach (Transform child in partySlots.transform)
        {
            SwapListen swapListener = child.GetComponent<SwapListen>();
            if (swapListener != null)
            {
                swapListener.OnSlotsSwapped -= SwapPokemonSlots;
            }
            Destroy(child.gameObject);
        }

        foreach (PokemonSlotSpriteData slotData in uiManager.PartySlotDataList)
        {
            GameObject slot = Instantiate(pokemonSlot, partySlots.transform);
            SwapListen swapListener = slot.GetComponentInChildren<SwapListen>();
            if (swapListener != null)
            {
                swapListener.OnSlotsSwapped += SwapPokemonSlots;
            }
            SetPokemonSlotDetails(slotData, slot);
        }
    }

    public void UpdateUIAtSlot(PokemonSlotSpriteData slotData, int index)
    {
        if (slotData == null)
        {
            Destroy(partySlots.transform.GetChild(index).gameObject);
        }
        else
        {
            GameObject slot;
            if (partySlots.transform.childCount <= index)
            {
                slot = Instantiate(pokemonSlot, partySlots.transform);
                SetPokemonSlotDetails(slotData, slot);
            }
            else
            {
                Destroy(partySlots.transform.GetChild(index).gameObject);
                slot = Instantiate(pokemonSlot, partySlots.transform);
                slot.transform.SetSiblingIndex(index);
            }

            SetPokemonSlotDetails(slotData, slot);
        }
    }

    private void SetPokemonSlotDetails(PokemonSlotSpriteData slotData, GameObject slot)
    {
        GameObject cardFace = slot.transform.Find("Card/CardFace").gameObject;
        SetTextComponents(slotData.CurrentObject, cardFace);
        SetPokemonHPDetails(slotData.CurrentObject, cardFace);
        SetPokemonEXPDetails(slotData.CurrentObject, cardFace);
        SetImageComponents(slotData, cardFace);

        GameObject moveObjectList = slot.transform.Find("Card/CardInfo").gameObject;
        PokemonType type = slotData.CurrentObject.BasePokemon.Types[0];
        SetPokemonMoveDetails(slotData, moveObjectList, type);
        SetPokemonSlotOrder(slot);

    }

    private void SetImageComponents(PokemonSlotSpriteData slotData, GameObject slot)
    {
        int MAX_TYPES_COUNT = 2;
        Pokemon pokemon = slotData.CurrentObject;

        Image[] imageComponents = slot.GetComponentsInChildren<Image>();
        Image pokemonImg = imageComponents[1];
        Image itemImg = imageComponents[2];
        Image ailmentImg = imageComponents[3];
        Image genderImg = imageComponents[4];
        Image type1 = imageComponents[5];
        Image type2 = imageComponents[6];

        pokemonImg.sprite = slotData.PokemonSprite;

        if (pokemon.HeldItem != null)
        {
            itemImg.sprite = slotData.ItemSprite;
            itemImg.preserveAspect = true;
        }
        else
        {
            itemImg.color = new Color(0, 0, 0, 0);
        }

        if (pokemon.IsFainted)
        {
            ailmentImg.sprite = slotData.FaintedSprite;
            ailmentImg.preserveAspect = true;
        }
        else
        {
            //check ailments and set sprite accordingly else transparent
            ailmentImg.color = new Color(0, 0, 0, 0);
        }

        genderImg.sprite = slotData.GenderSprite;
        genderImg.preserveAspect = true;

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

    private void SetTextComponents(Pokemon pokemon, GameObject slot)
    {
        Text[] textComponents = slot.GetComponentsInChildren<Text>();
        Text pokemonName = textComponents[0];
        Text pokemonLevel = textComponents[1];
        Text abilityName = textComponents[2];

        pokemonName.text =  pokemon.Nickname ?? UIUtils.FormatText(pokemon.BasePokemon.Name, true);
        pokemonLevel.text = $"Lv. {pokemon.CurrentLevel}";
        abilityName.text = UIUtils.FormatText(pokemon.CurrentAbility.BaseAbility.Name, false);
    }

    private void SetPokemonHPDetails(Pokemon pokemon, GameObject slot)
    {
        GameObject health = slot.transform.Find("Health").gameObject;
        Text pokemonHP = health.GetComponentInChildren<Text>();

        PokemonStat hpStat = pokemon.BasePokemon.Stats.Where(stat => stat.BaseStat.Name == StatName.HP).SingleOrDefault();
        if (hpStat != null)
        {
            pokemonHP.text = $"{hpStat.CurrentValue}/{hpStat.CalculatedValue}";
        }

        Color hpColor = UIUtils.GetColorForHP(hpStat.CurrentValue, hpStat.CalculatedValue);
        GridLayoutGroup hpBarContainer = health.GetComponentInChildren<GridLayoutGroup>();
        // set HP slider via HealthBar prefab
        GameObject hpBar = Instantiate(healthBar, hpBarContainer.transform);
        Image fill = hpBar.GetComponentsInChildren<Image>()[1];
        fill.color = hpColor;
        Slider hpSlider = hpBar.GetComponent<Slider>();
        hpSlider.minValue = 0;
        hpSlider.maxValue = hpStat.CalculatedValue;
        hpSlider.value = hpStat.CurrentValue;
    }

    private void SetPokemonEXPDetails(Pokemon pokemon, GameObject slot)
    {
        /*GameObject exp = slot.transform.Find("Exp").gameObject;
        Text pokemonExp = exp.GetComponentInChildren<Text>();

        Color expColor = ColorPalette.GetColor(ColorName.EXP);

        GameObject expBar = Instantiate(healthBar, exp.transform);
        Image fill = expBar.GetComponentsInChildren<Image>()[1];
        fill.color = expColor;
        Slider expSlider = expBar.GetComponent<Slider>();
         set min,current and max value
         */
    }

    private void SetPokemonMoveDetails(PokemonSlotSpriteData slotData, GameObject slot, PokemonType type)
    {
        Image bg = slot.GetComponent<Image>();
        Color bgColor = ColorPalette.AddShade(UIUtils.GetColorForType(type), 1);
        Color textColor = ColorPalette.GetTextContrastedColorFor(bgColor);

        bg.color = bgColor;

        GameObject moveObjectList = slot.transform.Find("Moves").gameObject;

        int MAX_MOVES_COUNT = 4;
        Pokemon pokemon = slotData.CurrentObject;

        List<PokemonMove> learnedMoves = pokemon.LearnedMoves;
        int numMoves = learnedMoves.Count;
        Image damageClass;
        Text moveName;
        Text movePP;

        for (int i = 0; i < MAX_MOVES_COUNT; i++)
        {
            GameObject moveObject = moveObjectList.transform.GetChild(i).gameObject;
            if (i >= numMoves)
            {
                moveObject.SetActive(false);
                continue;
            }
            else
            {
                PokemonMove move = learnedMoves[i];
                damageClass = moveObject.GetComponentInChildren<Image>();
                moveName = moveObject.GetComponentsInChildren<Text>()[0];
                movePP = moveObject.GetComponentsInChildren<Text>()[1];

                damageClass.sprite = slotData.MoveSpriteList[i];
                damageClass.preserveAspect = true;

                moveName.text = UIUtils.FormatText(move.BaseMove.Name, false);
                movePP.text = $"{move.CurrentPP}/{move.BaseMove.PP}";

                moveName.color = textColor;
                movePP.color = textColor;
            }
        }
    }

    private void SetPokemonSlotOrder(GameObject slot)
    {
        GameObject order = slot.transform.Find("Order").gameObject;
        Text orderNumber = order.GetComponentInChildren<Text>();
        orderNumber.text = $"{slot.transform.GetSiblingIndex() + 1}";
    }

    private void SwapPokemonSlots(int from, int to)
    {
        GameObject fromSlot = partySlots.transform.GetChild(from).gameObject;
        GameObject toSlot = partySlots.transform.GetChild(to).gameObject;
        SetPokemonSlotOrder(fromSlot);
        SetPokemonSlotOrder(toSlot);
    }
}