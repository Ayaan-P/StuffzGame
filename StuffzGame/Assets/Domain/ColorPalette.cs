using System;
using System.Collections.Generic;
using UnityEngine;

public static class ColorPalette
{
    internal class Entry
    {
        public ColorName Name { get; set; }
        public Color Color { get; set; }
    }

    private static readonly List<Entry> colorEntries = new List<Entry> {
            new Entry
            {
                Name = ColorName.PRIMARY_RED,
                Color = new Color(0.933f, 0.322f, 0.325f, 1)
            },
            new Entry
            {
                Name = ColorName.SECONDARY_RED,
                Color = new Color(1f, 0.420f, 0.420f, 1)
            },
            new Entry
            {
                Name = ColorName.FAINTED_RED,
                Color = new Color(1f, 0.549f, 0.549f, 1)
            },
            new Entry
            {
                Name = ColorName.PRIMARY_GREEN,
                Color = new Color(0.063f, 0.675f, 0.518f, 1)
            },
            new Entry
            {
                Name = ColorName.SECONDARY_GREEN,
                Color = new Color(0.114f, 0.820f, 0.631f, 1)
            },
            new Entry
            {
                Name = ColorName.PRIMARY_YELLOW,
                Color = new Color(1f, 0.624f, 0.263f, 1)
            },
            new Entry
            {
                Name = ColorName.SECONDARY_YELLOW,
                Color = new Color(0.996f, 0.792f, 0.341f, 1)
            },
             new Entry
            {
                Name = ColorName.PRIMARY_GREY,
                Color = new Color(0.427f, 0.431f, 0.443f, 1)
            },
              new Entry
            {
                Name = ColorName.SECONDARY_GREY,
                Color = new Color(0.501f, 0.509f, 0.521f, 1)
            },
                 new Entry
            {
                Name = ColorName.TYPE_NULL,
                Color = new Color(0.125f,0.125f,0.125f,1.000f)
            },
                 new Entry
            {
                Name = ColorName.TYPE_NORMAL,
                Color = new Color(0.67f, 0.65f, 0.62f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_FIRE,
                Color = new Color(1.00f, 0.25f, 0.20f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_WATER,
                Color = new Color(0.06f, 0.74f, 0.98f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_ELECTRIC,
                Color = new Color(1.00f, 0.83f, 0.16f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_GRASS,
                Color = new Color(0.23f, 0.89f, 0.45f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_ICE,
                Color = new Color(0.21f, 0.76f, 0.84f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_FIGHTING,
                Color = new Color(0.72f, 0.08f, 0.25f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_POISON,
                Color = new Color(0.56f, 0.24f, 0.59f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_GROUND,
                Color = new Color(0.88f, 0.69f, 0.17f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_FLYING,
                Color = new Color(0.48f, 0.53f, 0.76f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_PSYCHIC,
                Color = new Color(0.91f, 0.12f, 0.39f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_BUG,
                Color = new Color(0.69f, 0.71f, 0.21f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_ROCK,
                Color = new Color(0.80f, 0.68f, 0.38f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_GHOST,
                Color = new Color(0.37f, 0.15f, 0.80f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_DRAGON,
                Color = new Color(0.12f, 0.22f, 0.60f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_DARK,
                Color = new Color(0.24f, 0.15f, 0.14f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_STEEL,
                Color = new Color(0.50f, 0.56f, 0.65f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_FAIRY,
                Color = new Color(0.99f, 0.65f, 0.87f,1.000f)
            },
        };

    public static Color GetColor(ColorName name)
    {
        Entry entry = colorEntries.Find(c => c.Name == name);
        if (entry != null)
        {
            return entry.Color;
        }
        else
        {
            Debug.LogWarning($"Color {name} not found in ColorPalette.");
            return Color.black;
        }
    }

    public static Color GetTextContrastedColorFor(Color color)
    {
        var primaryGray = GetColor(ColorName.PRIMARY_GREY);
        var contrastThreshold = 4.5; //4.5 minimum, 7 is ideally better.
        var lumGray = RelativeLuminance(primaryGray);

        var lumColor = RelativeLuminance(color);
        var lighterColor = Math.Max(lumColor, lumGray);
        var darkerColor = Math.Min(lumColor, lumGray);

        var contrastRatio = (lighterColor + 0.05) / (darkerColor + 0.05);

        if (contrastRatio >= contrastThreshold)
        {
            return primaryGray;
        }
        else
        {
            //select between black and white:
            return (lumColor <= 0.1833) ? Color.white : Color.black;
        }
    }

    public static Color AddTint(Color color, int steps)
    {
        var stepValue = 0.1f;
        var shift = stepValue * steps;
        var newR = (color.r + shift < 1f) ? color.r + shift : 1f;
        var newG = (color.g + shift < 1f) ? color.g + shift : 1f;
        var newB = (color.b + shift < 1f) ? color.b + shift : 1f;

        return new Color(newR, newG, newB);
    }

    public static Color AddShade(Color color, int steps)
    {
        var stepValue = 0.1f;
        var shift = stepValue * steps;
        var newR = (color.r - shift >= 0f) ? color.r - shift : 0f;
        var newG = (color.g - shift >= 0f) ? color.g - shift : 0f;
        var newB = (color.b - shift >= 0f) ? color.b - shift : 0f;

        return new Color(newR, newG, newB);
    }

    private static double RelativeLuminance(Color color)
    {
        //Using relative luminance formula from: https://www.w3.org/TR/WCAG20/#relativeluminancedef
        var sRed = (double)color.r;
        var sGreen = (double)color.g;
        var sBlue = (double)color.b;
        var R = (sRed <= 0.03928) ? sRed / 12.92 : Math.Pow((sRed + 0.055) / 1.055, 2.4);
        var G = (sGreen <= 0.03928) ? sGreen / 12.92 : Math.Pow((sGreen + 0.055) / 1.055, 2.4);
        var B = (sBlue <= 0.03928) ? sBlue / 12.92 : Math.Pow((sBlue + 0.055) / 1.055, 2.4);

        return (0.2126 * R) + (0.7152 * G) + (0.0722 * B);
    }
}

public enum ColorName
{
    PRIMARY_RED,
    SECONDARY_RED,
    FAINTED_RED,
    PRIMARY_GREEN,
    SECONDARY_GREEN,
    PRIMARY_YELLOW,
    SECONDARY_YELLOW,
    PRIMARY_GREY,
    SECONDARY_GREY,
    TYPE_NULL,
    TYPE_NORMAL,
    TYPE_FIRE,
    TYPE_WATER,
    TYPE_ELECTRIC,
    TYPE_GRASS,
    TYPE_ICE,
    TYPE_FIGHTING,
    TYPE_POISON,
    TYPE_GROUND,
    TYPE_FLYING,
    TYPE_PSYCHIC,
    TYPE_BUG,
    TYPE_ROCK,
    TYPE_GHOST,
    TYPE_DRAGON,
    TYPE_DARK,
    TYPE_STEEL,
    TYPE_FAIRY
}