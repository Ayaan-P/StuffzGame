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
            Destroy(child.gameObject);
        }

        foreach (PokemonSlotSpriteData slotData in uiManager.PartySlotDataList)
        {
            GameObject slot = Instantiate(pokemonSlot, partySlots.transform);
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
        SetTextComponents(slotData.CurrentObject, slot);
        SetPokemonHPDetails(slotData.CurrentObject, slot);
        SetImageComponents(slotData, slot);
        SetPokemonMoveDetails(slotData, slot);
    }

    private void SetImageComponents(PokemonSlotSpriteData slotData, GameObject slot)
    {
        int MAX_TYPES_COUNT = 2;
        Pokemon pokemon = slotData.CurrentObject;

        Image[] imageComponents = slot.GetComponentsInChildren<Image>();
        Image slotBg = imageComponents[1];
        Image pokemonImg = imageComponents[2];
        Image itemImg = imageComponents[3];
        Image faintedImg = imageComponents[4];
        Image genderImg = imageComponents[5];
        Image type1 = imageComponents[6];
        Image type2 = imageComponents[7];

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
            slotBg.color = ColorPalette.GetColor(ColorName.FAINTED_RED);
            faintedImg.sprite = slotData.FaintedSprite;
            faintedImg.preserveAspect = true;
        }
        else
        {
            faintedImg.color = new Color(0, 0, 0, 0);
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
        Text abilityTitle = textComponents[3];
        Text abilityName = textComponents[4];

        pokemonName.text =  pokemon.Nickname ?? UIUtils.FormatText(pokemon.BasePokemon.Name, true);
        pokemonLevel.text = $"Lv. {pokemon.CurrentLevel}";
        abilityTitle.text = "Ability";
        abilityName.text = UIUtils.FormatText(pokemon.CurrentAbility.BaseAbility.Name, false);
    }

    private void SetPokemonHPDetails(Pokemon pokemon, GameObject slot)
    {
        Image slotBorder = slot.GetComponentsInChildren<Image>()[0];
        Text pokemonHP = slot.GetComponentsInChildren<Text>()[2];

        PokemonStat hpStat = pokemon.BasePokemon.Stats.Where(stat => stat.BaseStat.Name == StatName.HP).SingleOrDefault();
        if (hpStat != null)
        {
            pokemonHP.text = $"{hpStat.CurrentValue}/{hpStat.CalculatedValue}";
        }

        Color hpColor = UIUtils.GetColorForHP(hpStat.CurrentValue, hpStat.CalculatedValue);
        slotBorder.color = hpColor;

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

    private void SetPokemonMoveDetails(PokemonSlotSpriteData slotData, GameObject slot)
    {
        int MAX_MOVES_COUNT = 4;
        Pokemon pokemon = slotData.CurrentObject;
        GameObject moveObjectList = slot.transform.Find("Moves").gameObject;

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
            }
        }
    }
}