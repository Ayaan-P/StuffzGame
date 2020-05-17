using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonDB : MonoBehaviour
{
    public List<Pokemon> pokemon = new List<Pokemon>();

    private void Awake()
    {
        buildDB();
    }

    public Pokemon getPokemon(int id)
    {
        return pokemon.Find(pokemon => pokemon.id == id);
    }
    public Pokemon getPokemon(string name)
    {
        return pokemon.Find(pokemon => pokemon.name == name);
    }
    void buildDB()
    {
        pokemon.Add(new Pokemon(0, "Torterra", "A Grass Pokemon",
            new Dictionary<string,int>
            {
                {"Catch Rate", 10},
                {"Attack", 25},
                {"Defense", 40},
                {"Speed", 10},
                {"Special Attack",45},
                {"Special Defense",40}

            }));
        pokemon.Add(new Pokemon(1, "Greninja", "A Water Pokemon",
            new Dictionary<string,int>
            {
                {"Catch Rate", 10},
                {"Attack", 25},
                {"Defense", 40},
                {"Speed", 10},
                {"Special Attack",45},
                {"Special Defense",40}

            }));;
        pokemon.Add(new Pokemon(3, "Blaziken", "A Fire Pokemon",
            new Dictionary<string,int>
            {
                {"Catch Rate", 10},
                {"Attack", 2500},
                {"Defense", 40},
                {"Speed", 1000},
                {"Special Attack",4500},
                {"Special Defense",40}

            }));
        
    }
   
}