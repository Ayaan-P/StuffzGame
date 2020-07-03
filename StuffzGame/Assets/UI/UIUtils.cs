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
}
