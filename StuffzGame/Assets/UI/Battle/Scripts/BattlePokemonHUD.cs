using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattlePokemonHUD : MonoBehaviour
{
    public GameObject healthBar;
    public Pokemon Pokemon { get; set; }
    public bool IsEnemyHUD { get; set; }

    private GameObject hpPanel;
    private GameObject expPanel;

    private SpriteLoader loader;
    private Dictionary<HUDSprite, Sprite> hudSpriteDict;
    private Text[] textComponents;
    private Image[] imageComponents;
    private bool isHUDInit = false;

    private void Start()
    {
        isHUDInit = false;
        InitHUD();
    }

    private void Update()
    {
        if (!AreHUDSpritesReady())
        {
            LoadHUDSprites();
        }
    }

    public void UpdateHUD()
    {
        if (!isHUDInit)
        {
            InitHUD();
        }
        else
        {
            ResetHUDSpriteDict();
        }

        SetImageComponents();
        SetTextComponents();
        SetHP();
        if (!IsEnemyHUD)
        {
            SetEXP();
        }
    }

    private void SetImageComponents()
    {
        Image shadow = imageComponents[0];
        Image bg = imageComponents[1];
        Image gender = imageComponents[2];
        Image ailment = imageComponents[3];
        shadow.preserveAspect = true;
        bg.preserveAspect = true;

        gender.sprite = hudSpriteDict[HUDSprite.GENDER];
        gender.preserveAspect = true;

        ailment.sprite = hudSpriteDict[HUDSprite.AILMENT];
        ailment.preserveAspect = true;
    }

    private void SetTextComponents()
    {
        Text level = textComponents[0];
        Text name = textComponents[1];

        level.text = $"Lv. {Pokemon.CurrentLevel}";
        name.text = Pokemon.Nickname ?? UIUtils.FormatText(Pokemon.BasePokemon.Name, true);
    }

    private void SetHP()
    {
        Text pokemonHP = hpPanel.GetComponentInChildren<Text>();
        PokemonStat hpStat = Pokemon.GetStat(StatName.HP);
        if (hpStat != null)
        {
            pokemonHP.text = $"{hpStat.CurrentValue}/{hpStat.CalculatedValue}";
        }
        var layout = hpPanel.GetComponentInChildren<HorizontalLayoutGroup>();
        GameObject hpBar = Instantiate(healthBar, layout.transform, false);
        Color hpColor = UIUtils.GetColorForHP(hpStat.CurrentValue, hpStat.CalculatedValue);
        Image hpBarBg = hpBar.GetComponentsInChildren<Image>()[0];
        Image fill = hpBar.GetComponentsInChildren<Image>()[1];

        hpBarBg.preserveAspect = true;
        fill.preserveAspect = true;
        hpBarBg.pixelsPerUnitMultiplier = 2;
        fill.pixelsPerUnitMultiplier = 2;
        fill.color = hpColor;
        Slider hpSlider = hpBar.GetComponent<Slider>();
        hpSlider.minValue = 0;
        hpSlider.maxValue = hpStat.CalculatedValue;
        hpSlider.value = hpStat.CurrentValue;
    }

    private void SetEXP()
    {
        GameObject expBar = Instantiate(healthBar, expPanel.transform, false);
        
        Image[] hpComponents = expBar.GetComponentsInChildren<Image>();
        Image bg = hpComponents[0];
        Image fill = hpComponents[1];

        bg.preserveAspect = true;
        fill.preserveAspect = true;

        fill.color = ColorPalette.GetColor(ColorName.TYPE_WATER);
        Slider expSlider = expBar.GetComponent<Slider>();

        if (Pokemon.CurrentLevel == Pokemon.MAX_LEVEL)
        {
            expSlider.minValue = 0;
            expSlider.maxValue = 0;
            expSlider.value = 0;
        }
        else
        {
            expSlider.minValue = Pokemon.BasePokemon.Species.GrowthRate.LevelExperienceDict[Pokemon.CurrentLevel];
            expSlider.maxValue = Pokemon.BasePokemon.Species.GrowthRate.LevelExperienceDict[Pokemon.CurrentLevel + 1];
            expSlider.value = Pokemon.BasePokemon.Species.GrowthRate.CurrentExperience ?? 0;
        }
    }

    private void InitHUD()
    {
        loader = new SpriteLoader();
        textComponents = this.GetComponentsInChildren<Text>();
        imageComponents = this.GetComponentsInChildren<Image>();
        hpPanel = this.transform.Find("ContentArea/Panel/HpExpPanel/Health").gameObject;
        expPanel = this.transform.Find("ContentArea/Panel/HpExpPanel/Exp").gameObject;
        if (hpPanel == null || expPanel == null)
        {
            Debug.LogError($"No hpPanel or expPanel found in {typeof(BattlePokemonHUD)}");
        }
        ResetHUDSpriteDict();
        isHUDInit = true;
    }

    private void ResetHUDSpriteDict()
    {
        hudSpriteDict = new Dictionary<HUDSprite, Sprite> {
            { HUDSprite.GENDER, null},
            { HUDSprite.AILMENT, null}
        };
    }

    private void LoadHUDSprites()
    {
        if (Pokemon != null)
        {
            foreach (var key in Enum.GetNames(typeof(HUDSprite)))
            {
                Enum.TryParse<HUDSprite>(key, false, out HUDSprite result);
                hudSpriteDict.TryGetValue(result, out Sprite value);
                if (value == null)
                {
                    switch (result)
                    {
                        case HUDSprite.GENDER:
                            Sprite genderSprite = loader.LoadGenderSprite(Pokemon.Gender);
                            if (genderSprite != null)
                            {
                                hudSpriteDict[HUDSprite.GENDER] = genderSprite;
                                SetImageComponents();
                            }
                            break;

                        case HUDSprite.AILMENT:
                            Sprite ailmentSprite = loader.LoadAilmentSprite(Pokemon.Ailment);
                            if (ailmentSprite != null)
                            {
                                hudSpriteDict[HUDSprite.AILMENT] = ailmentSprite;
                                SetImageComponents();
                            }
                            break;

                        default:
                            Debug.LogError($"{key} has no valid HUD sprite to load");
                            break;
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Cant load HUD sprites, Pokemon is null");
        }
    }

    private bool AreHUDSpritesReady()
    {
        if (hudSpriteDict[HUDSprite.GENDER] == null ||
           loader.DoesAilmentHaveSprite(Pokemon.Ailment) && hudSpriteDict[HUDSprite.AILMENT] == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private enum HUDSprite
    {
        GENDER,
        AILMENT
    }
}