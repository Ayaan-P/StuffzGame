using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon 
{
    public int id;
    public string name;
    public string description;
    public Sprite icon;
    public Dictionary<string, int> stats = new Dictionary<string, int>();

    public Pokemon(int id, string name, string description, Dictionary<string,int> stats)
    {
        this.id=id;
        this.name=name;
        this.description=description;
        this.icon = Resources.Load<Sprite>("Sprites/Pokemon/" + name);
        this.stats = stats;
    }
    public Pokemon(Pokemon pokemon)
    {
        this.id = pokemon.id;
        this.name = pokemon.name;
        this.description = pokemon.description;
        this.icon = Resources.Load<Sprite>("Sprites/Pokemon/" + pokemon.name);
        this.stats = pokemon.stats;
    }
}
