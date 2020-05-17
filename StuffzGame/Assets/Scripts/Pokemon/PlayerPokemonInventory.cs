using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPokemonInventory : MonoBehaviour
{
    public List<Pokemon> char_pokemon = new List<Pokemon>(); //player item list
    public PokemonDB pokemon_db; // database of all items

    private void Start()
    {
        givePokemon(0);
        removePokemon(0);
    }
        
//gives item to player based
    public void givePokemon( int id )
    {
        Pokemon pokemon = pokemon_db.getPokemon(id); //lookup item by id in db
        char_pokemon.Add(pokemon);
        Debug.Log(" Gave Pokemon: " + pokemon.name); // post to console
    }

//checks and returns Pokemon if player has it
    public Pokemon checkForPokemon( int id)
    {
        return char_pokemon.Find(pokemon => pokemon.id == id);
    }

//removes item from player inventory
    public void removePokemon(int id)
    {
        Pokemon pokemon = checkForPokemon(id);
        if(pokemon!=null)
        {
            char_pokemon.Remove(pokemon);
            Debug.Log(" Removed Pokemon: " + pokemon.name); // post to console
        }
    }
}
