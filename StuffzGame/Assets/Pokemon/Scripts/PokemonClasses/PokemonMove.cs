using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PokemonMove
{

    public BasePokemonMove BaseMove { get; set; }
    public int CurrentPP { get; set; }
    public  List<MoveLearnDetails> MoveLearnDetails { get; set; }
    public const int MAX_MOVES = 4;
}


public class MoveLearnDetails
{
    public int LevelLearnedAt { get; set; }
    public MoveLearnMethod MoveLearnMethod { get; set; }
}