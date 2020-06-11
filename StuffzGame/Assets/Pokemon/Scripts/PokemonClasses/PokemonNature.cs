using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonNature 
{
    public int Id { get; set; }
    public Nature Name { get; set; }
    public BasePokemonStat IncreasedStat { get; set; }
    public BasePokemonStat DecreasedStat { get; set; }
    public BerryFlavor LikedBerryFlavor { get; set; }
    public BerryFlavor DislikedBerryFlavor { get; set; }
}
