using System.Collections.Generic;
using UnityEngine;

public static class ColorPalette
{
    private static readonly List<Entry> colorEntries = new List<Entry> {
            new Entry
            {
                Name = ColorName.PRIMARY_RED,
                Color = new Color(238, 82, 83, 1.0f)
            },
            new Entry
            {
                Name = ColorName.SECONDARY_RED,
                Color = new Color(255, 107, 107, 1.0f)
            },
            new Entry
            {
                Name = ColorName.PRIMARY_GREEN,
                Color = new Color(16, 172, 132, 1.0f)
            },
            new Entry
            {
                Name = ColorName.SECONDARY_GREEN,
                Color = new Color(29, 209, 161, 1.0f)
            },
            new Entry
            {
                Name = ColorName.PRIMARY_YELLOW,
                Color = new Color(255, 159, 67, 1.0f)
            },
            new Entry
            {
                Name = ColorName.SECONDARY_YELLOW,
                Color = new Color(254, 202, 87, 1.0f)
            }
        };

    public static Color GetColor(ColorName name)
    {
        var entry = colorEntries.Find(c => c.Name == name);
        if (entry != null)
        {
            return entry.Color;
        }
        else
        {
            Debug.LogWarning($"Color {name} not found in ColorPalette.");
            return Color.white;
        }
    }

    public class Entry
    {
        public ColorName Name { get; set; }
        public Color Color { get; set; }
    }
}

public enum ColorName
{
    PRIMARY_RED,
    SECONDARY_RED,
    PRIMARY_GREEN,
    SECONDARY_GREEN,
    PRIMARY_YELLOW,
    SECONDARY_YELLOW
}