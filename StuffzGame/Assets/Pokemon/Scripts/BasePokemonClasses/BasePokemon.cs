using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePokemon
{
   
    public List<PokemonAbility> Abilities { get; set; }
    public int Height { get; set; }
    public int BaseExperienceOnDefeat { get; set; }
    public int Id { get; set; }
    public bool IsDefault { get; set; }
    public List<PokemonMove> PossibleMoveList { get; set; }
    public string Name { get; set; }
    public int Order { get; set; }
    public PokemonSpecies Species { get; set; }
    public List<PokemonStat> Stats { get; set; }
    public List<PokemonType> Types { get; set; }
    public int Weight { get; set; }
}
