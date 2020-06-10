using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasePokemonTemplate: BasePokemon
{
    public List<int> AbilityIdList { get; set; }
    public List<bool> IsAbilityHiddenList { get; set; }
    public List<int> AbilitySlotList { get; set; }
    public List<int> PossibleMoveIdList { get; set; }
    public List<List<MoveLearnDetails>> MoveLearnDetailsList { get; set; }
    public int SpeciesId { get; set; }
    public List<int> BaseStatIdList { get; set; }
    public List<int> EVsGainedOnDefeatList { get; set; }
    public List<int> BaseStatValueList { get; set; }
}
