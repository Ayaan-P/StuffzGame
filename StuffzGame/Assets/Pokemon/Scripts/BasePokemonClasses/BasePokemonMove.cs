using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePokemonMove 
{

    public int? Accuracy { get; set; }
    public MoveDamageClass MoveDamageClass { get; set; }
    public float? EffectChance { get; set; }
    public List<string> EffectEntries { get; set; }
    public string FlavorText { get; set; }
    public int Id { get; set; }
    public MoveAilment Ailment { get; set; }
    public float AilmentChance { get; set; }
    public MoveCategory Category { get; set; }
    public int CritRate { get; set; }
    public int Drain { get; set; }
    public float FlinchChance { get; set; }
    public int Healing { get; set; }
    public int? MaxHits { get; set; }
    public int? MinHits { get; set; }
    public int? MaxTurns { get; set; }
    public int? MinTurns { get; set; }
    public float StatChance { get; set; }
    public string Name { get; set; }
    public int? Power { get; set; }
    public int PP { get; set; }
    public MovePriority Priority { get; set; }
    public Dictionary<BasePokemonStat, int> StatChanges { get; set; }
    public PokemonTarget Target { get; set; }
    public PokemonType Type { get; set; }
}
