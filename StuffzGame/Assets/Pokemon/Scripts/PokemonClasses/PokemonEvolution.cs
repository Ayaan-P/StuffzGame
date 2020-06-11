using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonEvolution
{
 
    public int BabyTriggerItemId { get; set; }
    public int Id { get; set; }
    public bool IsBabyPokemon { get; set; }
    public int PokemonSpeciesId { get; set; }
    public List<EvolutionDetail> EvolutionDetails { get; set; }
}


public class EvolutionDetail {

    public Gender RequiredGender { get; set; }
    public int RequiredHeldItemId { get; set; }
    public int EvolutionItemId { get; set; }
    public int KnownMoveId { get; set; }
    public PokemonType KnownMoveType { get; set; }
    public int? MinHappiness { get; set; }
    public int? MinLevel { get; set; }
    public bool NeedsOverworldRain { get; set; }
    public int PartySpeciesId { get; set; }
    public PokemonType PartyType { get; set; }

    /*The required relation between the Pokémon's Attack and Defense stats.
   * 1 means Attack > Defense.
   * 0 means Attack = Defense.
   * -1 means Attack < Defense.
   * For: Tyrogue */
    public RelativePhysicalStatDifference RelativePhysicalStats { get; set; }
    public TimeOfDay RequiredTimeOfDay { get; set; }
    public int TradeSpeciesId { get; set; } //Pokemon species for which this one must be traded
    public EvolutionTrigger Trigger { get; set; }
}