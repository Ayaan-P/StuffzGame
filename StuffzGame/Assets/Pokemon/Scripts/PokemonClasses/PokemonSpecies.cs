using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using UnityEngine;

public class PokemonSpecies
{

    public int BaseHappiness { get; set; }
    public float CaptureRate { get; set; }
    public List<EggGroup> EggGroups { get; set; }
    public PokemonEvolution EvolvesFrom { get; set; }
    public int EvolvesFromSpeciesId { get; set; }
    public string FlavorText { get; set; }
    public bool FormsSwitchable { get; set; }
    public GenderRate GenderRate { get; set; }
    public string Genus { get; set; }
    public PokemonGrowthRate GrowthRate { get; set; }
    public bool HasGenderDifferences { get; set; }
    public int HatchCounter { get; set; }
    public int Id { get; set; }
    public bool IsBaby { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
    public Dictionary<int, bool> PokemonVarieties { get; set; } // maps pokemon id to boolean (is_default)
}
