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
            }
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
}