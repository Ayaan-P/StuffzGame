using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{
    public List<ItemAttribute> Attributes { get; set; }
    public int BabyTriggerForEvolutionId { get; set; }
    public ItemCategory Category { get; set; }
    public long Cost { get; set; }
    public List<string> EffectEntries { get; set; }
    public string FlavorText { get; set; }
    public ItemFlingEffect FlingEffect { get; set; }
    public int? FlingPower { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public int? Count { get; set; }
  
    
   
  
  
}
