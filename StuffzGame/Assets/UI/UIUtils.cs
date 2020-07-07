using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class UIUtils
{

    public static string FormatText(string str, bool keepDashes)
    {
        string str1 = (keepDashes) ? str : str.Replace('-', ' ');
        // capitalize every word
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str1.ToLower());
    }

    public static Color GetColorForHP(int currentHP, int maxHP)
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

    internal static Color GetColorForType(PokemonType type)
    {
        switch (type)
        {
            case PokemonType.NULL:
                return ColorPalette.GetColor(ColorName.TYPE_NULL);
            case PokemonType.NORMAL:
                return ColorPalette.GetColor(ColorName.TYPE_NORMAL);
            case PokemonType.FIRE:
                return ColorPalette.GetColor(ColorName.TYPE_FIRE);
            case PokemonType.WATER:
                return ColorPalette.GetColor(ColorName.TYPE_WATER);
            case PokemonType.ELECTRIC:
                return ColorPalette.GetColor(ColorName.TYPE_ELECTRIC);
            case PokemonType.GRASS:
                return ColorPalette.GetColor(ColorName.TYPE_GRASS);
            case PokemonType.ICE:
                return ColorPalette.GetColor(ColorName.TYPE_ICE);
            case PokemonType.FIGHTING:
                return ColorPalette.GetColor(ColorName.TYPE_FIGHTING);
            case PokemonType.POISON:
                return ColorPalette.GetColor(ColorName.TYPE_POISON);
            case PokemonType.GROUND:
                return ColorPalette.GetColor(ColorName.TYPE_GROUND);
            case PokemonType.FLYING:
                return ColorPalette.GetColor(ColorName.TYPE_FLYING);
            case PokemonType.PSYCHIC:
                return ColorPalette.GetColor(ColorName.TYPE_PSYCHIC);
            case PokemonType.BUG:
                return ColorPalette.GetColor(ColorName.TYPE_BUG);
            case PokemonType.ROCK:
                return ColorPalette.GetColor(ColorName.TYPE_ROCK);
            case PokemonType.GHOST:
                return ColorPalette.GetColor(ColorName.TYPE_GHOST);
            case PokemonType.DRAGON:
                return ColorPalette.GetColor(ColorName.TYPE_DRAGON);
            case PokemonType.DARK:
                return ColorPalette.GetColor(ColorName.TYPE_DARK);
            case PokemonType.STEEL:
                return ColorPalette.GetColor(ColorName.TYPE_STEEL);
            case PokemonType.FAIRY:
                return ColorPalette.GetColor(ColorName.TYPE_FAIRY);
            default:
                return ColorPalette.GetColor(ColorName.TYPE_NULL);
        }
    }
}
