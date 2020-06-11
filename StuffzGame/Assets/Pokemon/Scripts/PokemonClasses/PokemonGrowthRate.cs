using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonGrowthRate 
{

    public string Description { get; set; }
    public string Formula { get; set; }
    public int Id { get; set; }
    public Dictionary<int, long> LevelExperienceDict { get; set; }
    public string Name { get; set; }
}
