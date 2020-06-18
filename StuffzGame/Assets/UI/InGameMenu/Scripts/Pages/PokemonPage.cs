using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class PokemonPage : MonoBehaviour
{
    public GameObject partySlots;
    public GameObject pokemonSlot;

    // Start is called before the first frame update
    private void Start()
    {
        UpdatePartyUI();
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void UpdatePartyUI()
    {
        Player player = Player.Instance;
        List<Pokemon> party = player.PlayerParty;

        foreach (Pokemon pokemon in party)
        {
            SetPokemonSlotDetails(pokemon);
            GameObject slot = Instantiate(pokemonSlot, partySlots.transform);
        }
    }

    private void SetPokemonSlotDetails(Pokemon pokemon)
    {
        setTextComponents(pokemon);
        setImageComponents(pokemon);
        SetPokemonHPDetails(pokemon);
        SetPokemonMoveDetails(pokemon);
    }

    private void setImageComponents(Pokemon pokemon)
    {
        int MAX_TYPES_COUNT = 2;

        Image[] imageComponents = pokemonSlot.GetComponentsInChildren<Image>();
        SpriteLoader loader = new SpriteLoader();
       
        Image slotBg = imageComponents[1];
        Image pokemonImg = imageComponents[2];
        Image itemImg = imageComponents[3];
        Image faintedImg = imageComponents[4];
        Image genderImg = imageComponents[5];
        Image type1 = imageComponents[6];
        Image type2 = imageComponents[7];

        pokemonImg.sprite = loader.LoadPokemonSprite(pokemon.BasePokemon.Id, pokemon.IsShiny, SpriteType.OVERWORLD_POKEMON);
        if(pokemon.HeldItem!= null)
        {
            itemImg.sprite = loader.LoadItemSprite(pokemon.HeldItem.Name);
        }
        else
        {
            itemImg.color = new Color(0, 0, 0, 0.0f);
        }

        if (pokemon.IsFainted)
        {
            faintedImg.sprite = loader.LoadFaintedSprite();
        }
        else
        {
            faintedImg.color = new Color(0, 0, 0, 0.0f);
        }

        genderImg.sprite = loader.LoadGenderSprite(pokemon.Gender);
        
        if(pokemon.BasePokemon.Types.Count == MAX_TYPES_COUNT)
        {
            type1.sprite = loader.LoadTypeSprite(pokemon.BasePokemon.Types[0]);
            type2.sprite = loader.LoadTypeSprite(pokemon.BasePokemon.Types[1]);
        }
        else
        {
            type1.sprite = loader.LoadTypeSprite(pokemon.BasePokemon.Types[0]);
            type2.color = new Color(0, 0, 0, 0.0f);
        }
    }

    private void setTextComponents(Pokemon pokemon)
    {
        Text[] textComponents = pokemonSlot.GetComponentsInChildren<Text>();
        Text pokemonName = textComponents[0];
        Text pokemonLevel = textComponents[1];
        Text abilityTitle = textComponents[3];
        Text abilityName = textComponents[4];

        pokemonName.text = FormatText(pokemon.BasePokemon.Name, true);
        pokemonLevel.text = $"Lv. {pokemon.CurrentLevel}";
        abilityTitle.text = "Ability";
        abilityName.text = FormatText(pokemon.CurrentAbility.BaseAbility.Name, false);
    }

    private void SetPokemonHPDetails(Pokemon pokemon)
    {
        Image slotBorder = pokemonSlot.GetComponentsInChildren<Image>()[0];
        Text pokemonHP=  pokemonSlot.GetComponentsInChildren<Text>()[2];

        PokemonStat hpStat = pokemon.BasePokemon.Stats.Where(stat => stat.BaseStat.Name == StatName.HP).SingleOrDefault();
        if (hpStat != null)
        {
            pokemonHP.text = $"{hpStat.CurrentValue}/{hpStat.CalculatedValue}";
        }

        Color hpColor = GetColorForHP(hpStat.CurrentValue, hpStat.CalculatedValue);

        slotBorder.color = hpColor;
        // set HP slider via HealthBar prefab?
    }

    private void SetPokemonMoveDetails(Pokemon pokemon)
    {
        GameObject moves = pokemonSlot.transform.Find("Moves").gameObject;
    }

    private string FormatText(string str, bool keepDashes)
    {
        string str1;

        if (!keepDashes)
        {
            //replace dashes with spaces
             str1 = str.Replace('-', ' ');
        }
        else
        {
             str1 = str;
        }
     
        //capitalize first letter
        char capitalFirstChar = (char)(str1.First() - 32);
        return capitalFirstChar + str1.Substring(1);
    }

    private Color GetColorForHP(int currentHP, int maxHP)
    {
        float FIFTY_PERCENT = 0.5f;
        float TEN_PERCENT = 0.1f;

        float hpPercent = currentHP / (float)maxHP;
        if (hpPercent >= FIFTY_PERCENT)
        {
            return ColorPalette.GetColor(ColorName.PRIMARY_GREEN);
        }else if(hpPercent > TEN_PERCENT && hpPercent < FIFTY_PERCENT)
        {
            return ColorPalette.GetColor(ColorName.PRIMARY_YELLOW);

        }
        else
        {
            return ColorPalette.GetColor(ColorName.PRIMARY_RED);
        }

    }
}