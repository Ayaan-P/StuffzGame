using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePokemonStat
{
 
    public int Id { get; set; }
    public bool IsBattleOnly { get; set; }
    public MoveDamageClass DamageClass { get; set; }
    public StatName Name { get; set; }

}
