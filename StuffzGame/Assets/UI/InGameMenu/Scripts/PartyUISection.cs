using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class PartyUISection : MonoBehaviour
{

    public void SetPokemonSlotDetails(PokemonSlotSpriteData pokemonSlotData, GameObject pSlot, GameObject healthBar)
    {
        SetPokemonTextComponents(pokemonSlotData, pSlot);
        SetPokemonImageComponents(pokemonSlotData, pSlot);
        SetPokemonHPDetails(pokemonSlotData.CurrentObject, pSlot, healthBar);
    }

    private void SetPokemonTextComponents(PokemonSlotSpriteData pokemonSlotData, GameObject pSlot)
    {
        Pokemon pokemon = pokemonSlotData.CurrentObject;
        Text[] textComponents = pSlot.GetComponentsInChildren<Text>();

        Text pokemonName = textComponents[0];
        Text pokemonLevel = textComponents[1];
        Text isAbleText = textComponents[3];
        isAbleText.text = "";
        pokemonName.text = pokemon.Nickname ?? UIUtils.FormatText(pokemon.BasePokemon.Name, true);
        pokemonLevel.text = $"Lv. {pokemon.CurrentLevel}";
    }

    private void SetPokemonImageComponents(PokemonSlotSpriteData slotData, GameObject slot)
    {
        Pokemon pokemon = slotData.CurrentObject;
        Image[] imageComponents = slot.GetComponentsInChildren<Image>();
        Image pokemonImage = imageComponents[1];
        Image heldItemImage = imageComponents[2];
        Image ailmentImage = imageComponents[3];
        Image genderImage = imageComponents[4];
        Image type1 = imageComponents[5];
        Image type2 = imageComponents[6];

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

        if (pokemon.IsFainted)
        {
            ailmentImage.sprite = slotData.FaintedSprite;
            ailmentImage.preserveAspect = true;
        }
        else 
        {
            ailmentImage.sprite = slotData.AilmentSprite;
            ailmentImage.preserveAspect = true;
        }

        genderImage.sprite = slotData.GenderSprite;
        genderImage.preserveAspect = true;

        if (pokemon.BasePokemon.Types.Count == Pokemon.MAX_POKEMON_TYPES)
        {
            type1.sprite = slotData.TypeSpriteList[0];
            type1.preserveAspect = true;
            type2.gameObject.SetActive(true);
            type2.sprite = slotData.TypeSpriteList[1];
            type2.preserveAspect = true;
        }
        else
        {
            type1.sprite = slotData.TypeSpriteList[0];
            type1.preserveAspect = true;
            type2.gameObject.SetActive(false);
        }
    }

    private void SetPokemonHPDetails(Pokemon pokemon, GameObject slot, GameObject healthBar)
    {
        Text pokemonHP = slot.GetComponentsInChildren<Text>()[2];

        PokemonStat hpStat = pokemon.GetStat(StatName.HP);
        if (hpStat != null)
        {
            pokemonHP.text = $"{hpStat.CurrentValue}/{hpStat.CalculatedValue}";
        }

        Color hpColor = UIUtils.GetColorForHP(hpStat.CurrentValue, hpStat.CalculatedValue);

        // set HP slider via HealthBar prefab
        GameObject health = slot.transform.Find("Health").gameObject;
        GameObject hpBar = Instantiate(healthBar, health.transform);
        Image[] hpComponents = hpBar.GetComponentsInChildren<Image>();
        Image bg = hpComponents[0];
        Image fill = hpComponents[1];

        bg.preserveAspect = true;
        fill.preserveAspect = true;
        fill.color = hpColor;
        Slider hpSlider = hpBar.GetComponent<Slider>();
        hpSlider.minValue = 0;
        hpSlider.maxValue = hpStat.CalculatedValue;
        hpSlider.value = hpStat.CurrentValue;
    }
}
