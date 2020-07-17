using System.Collections.Generic;
using UnityEngine;

public class PlayerParty
{
    public readonly int MAX_PARTY_SIZE = 6;
    private const bool enableDebug = false;
    private List<Pokemon> partyPokemon;
    public delegate void LoadSprite<T>(T thing, int index);
    public delegate void SwapSprite(int firstIndex, int secondIndex);

    public event LoadSprite<Pokemon> OnPartyPokemonChanged;
    public event SwapSprite OnPartyPokemonSwapped;

    public PlayerParty()
    {
        partyPokemon = new List<Pokemon>();
    }

    public void Add(Pokemon pokemon)
    {
        if (partyPokemon.Count < MAX_PARTY_SIZE)
        {
            partyPokemon.Add(pokemon);
            if (enableDebug) { Debug.Log($"Added {pokemon.BasePokemon.Name} to party"); }
            OnPartyPokemonChanged(pokemon, partyPokemon.Count - 1);
        }
        else
        {
            Debug.Log($"Party full ({partyPokemon.Count}). Adding {pokemon} to PC");
            // Add pokemon to PC instead.
            PokemonStorage PC = PokemonStorage.GetInstance();
            PC.AddPokemon(pokemon);
        }
    }

    public bool Remove(Pokemon pokemon)
    {
        int index = partyPokemon.IndexOf(pokemon);
        if (index == -1)
        {
            Debug.LogError($"{pokemon.BasePokemon.Name} cannot be removed because it does not exist in Party");
            return false;
        }
        else
        {
            OnPartyPokemonChanged(null, index);
            if (enableDebug) { Debug.Log($"Removed {pokemon.BasePokemon.Name} from party"); }
            return partyPokemon.Remove(pokemon);
        }
    }

    public bool Swap(int firstIndex, int secondIndex)
    {
        if (firstIndex < 0 || firstIndex >= partyPokemon.Count || secondIndex < 0 || secondIndex >= partyPokemon.Count)
        {
            Debug.LogError($"Cannot swap pokemon in party at indices: {firstIndex} or {secondIndex} because either is out of bounds");
            return false;
        }
        else
        {
            Pokemon temp = partyPokemon[firstIndex];
            partyPokemon[firstIndex] = partyPokemon[secondIndex];
            partyPokemon[secondIndex] = temp;
            OnPartyPokemonSwapped(firstIndex, secondIndex);
            if (enableDebug) { Debug.Log($"Swapped pokemon at index: {firstIndex} with index: {secondIndex} (size: {PartySize()})"); }
            return true;
        }
    }

    public Pokemon GetPokemonAtIndex(int index)
    {
        if(index < PartySize() && index > -1)
        {
            return partyPokemon[index];
        }
        else
        {
            Debug.LogError($"Cant access party pokemon at index: {index}");
            return null;
        }
    }

    public int PartySize()
    {
        return partyPokemon.Count;
    }
}