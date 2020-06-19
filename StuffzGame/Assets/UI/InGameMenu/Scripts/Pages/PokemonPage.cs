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
    private SpriteLoader loader;
    private bool partyLoaded;

    // Start is called before the first frame update
    private void Start()
    {
        loader = new SpriteLoader();
        partyLoaded = false;
    }

    // Update is called once per frame
    private void Update()
    {
        if (loader.IsReady() && !partyLoaded)
        {
            UpdatePartyUI();
        }
        else
        {
            return;
        }
    }

    private void UpdatePartyUI()
    {
        Player player = Player.Instance;
        List<Pokemon> party = player.PlayerParty;

        foreach (Transform child in partySlots.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (Pokemon pokemon in party)
        {
            GameObject slot = Instantiate(pokemonSlot, partySlots.transform);
            SetPokemonSlotDetails(pokemon, slot);
        }
        partyLoaded = true;
    }

    private void SetPokemonSlotDetails(Pokemon pokemon, GameObject slot)
    {
        SetTextComponents(pokemon, slot);
        SetPokemonHPDetails(pokemon, slot);
        SetImageComponents(pokemon, slot);
        SetPokemonMoveDetails(pokemon, slot);
    }

    private void SetImageComponents(Pokemon pokemon, GameObject slot)
    {
        int MAX_TYPES_COUNT = 2;

        Image[] imageComponents = slot.GetComponentsInChildren<Image>();
        Image slotBg = imageComponents[1];
        Image pokemonImg = imageComponents[2];
        Image itemImg = imageComponents[3];
        Image faintedImg = imageComponents[4];
        Image genderImg = imageComponents[5];
        Image type1 = imageComponents[6];
        Image type2 = imageComponents[7];

        pokemonImg.sprite = loader.LoadPokemonSprite(pokemon.BasePokemon.Id, pokemon.IsShiny, SpriteType.OVERWORLD_POKEMON);

        if (pokemon.HeldItem != null)
        {
            itemImg.sprite = loader.LoadItemSprite(pokemon.HeldItem.Name);
            itemImg.preserveAspect = true;
        }
        else
        {
            itemImg.color = new Color(0, 0, 0, 0);
        }

        if (pokemon.IsFainted)
        {
            slotBg.color = ColorPalette.GetColor(ColorName.FAINTED_RED);
            faintedImg.sprite = loader.LoadFaintedSprite();
            faintedImg.preserveAspect = true;
        }
        else
        {
            faintedImg.color = new Color(0, 0, 0, 0);
        }

        genderImg.sprite = loader.LoadGenderSprite(pokemon.Gender);
        genderImg.preserveAspect = true;

        if (pokemon.BasePokemon.Types.Count == MAX_TYPES_COUNT)
        {
            type1.sprite = loader.LoadTypeSprite(pokemon.BasePokemon.Types[0]);
            type1.preserveAspect = true;
            type2.sprite = loader.LoadTypeSprite(pokemon.BasePokemon.Types[1]);
            type2.preserveAspect = true;
        }
        else
        {
            type1.sprite = loader.LoadTypeSprite(pokemon.BasePokemon.Types[0]);
            type1.preserveAspect = true;
            type2.sprite = loader.LoadTypeSprite(PokemonType.NULL);
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

        pokemonName.text = FormatText(pokemon.BasePokemon.Name, true);
        pokemonLevel.text = $"Lv. {pokemon.CurrentLevel}";
        abilityTitle.text = "Ability";
        abilityName.text = FormatText(pokemon.CurrentAbility.BaseAbility.Name, false);
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

        Color hpColor = GetColorForHP(hpStat.CurrentValue, hpStat.CalculatedValue);
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

    private void SetPokemonMoveDetails(Pokemon pokemon, GameObject slot)
    {
        int MAX_MOVES_COUNT = 4;
      
        GameObject moveObjectList = slot.transform.Find("Moves").gameObject;

        List<PokemonMove> learnedMoves = pokemon.LearnedMoves;
        int numMoves = learnedMoves.Count;
        Image damageClass;
        Text moveName;
        Text movePP;

        for (int i = 0; i < MAX_MOVES_COUNT; i++)
        {
            GameObject moveObject = moveObjectList.transform.GetChild(i).gameObject;
            if (i>= numMoves)
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

                damageClass.sprite = loader.LoadMoveDamageClassSprite(move.BaseMove.MoveDamageClass, false);
                damageClass.preserveAspect = true;

                moveName.text = FormatText(move.BaseMove.Name, false);
                movePP.text = $"{move.CurrentPP}/{move.BaseMove.PP}";
            }
           
        }
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

        // capitalize every word
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str1.ToLower());
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
        else
        {
            return ColorPalette.GetColor(ColorName.PRIMARY_RED);
        }
    }
}