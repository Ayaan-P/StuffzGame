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
                Color = new Color(0.847f,0.804f,0.788f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_FIRE,
                Color = new Color(0.945f,0.349f,0.173f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_WATER,
                Color = new Color(0.188f,0.643f,0.863f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_ELECTRIC,
                Color = new Color(0.988f,0.847f,0.204f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_GRASS,
                Color = new Color(0.282f,0.690f,0.310f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_ICE,
                Color = new Color(0.212f,0.761f,0.839f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_FIGHTING,
                Color = new Color(0.718f,0.125f,0.145f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_POISON,
                Color = new Color(0.565f,0.243f,0.596f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_GROUND,
                Color = new Color(0.922f,0.710f,0.251f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_FLYING,
                Color = new Color(0.475f,0.525f,0.761f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_PSYCHIC,
                Color = new Color(0.918f,0.106f,0.392f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_BUG,
                Color = new Color(0.686f,0.710f,0.212f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_ROCK,
                Color = new Color(0.804f,0.682f,0.384f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_GHOST,
                Color = new Color(0.396f,0.286f,0.620f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_DRAGON,
                Color = new Color(0.267f,0.333f,0.647f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_DARK,
                Color = new Color(0.239f,0.149f,0.133f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_STEEL,
                Color = new Color(0.561f,0.643f,0.682f,1.000f)
            },new Entry
            {
                Name = ColorName.TYPE_FAIRY,
                Color = new Color(0.949f,0.502f,0.671f,1.000f)
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