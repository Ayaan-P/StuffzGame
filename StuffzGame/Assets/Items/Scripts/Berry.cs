using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Berry : Item
{
    public BerryFirmness Firmness { get; set; }
    public List<BerryFlavor> Flavors { get; set; }
    public int GrowthTime { get; set; }
    public int ItemId { get; set; }
    public int MaxHarvest { get; set; }
    public string BerryName { get; set; }
    public int NaturalGiftPower { get; set; }
    public int NaturalGiftTypeId { get; set; }
    public int Size { get; set; }
    public int Smoothness { get; set; }
    public int Soil_dryness { get; set; }
}
