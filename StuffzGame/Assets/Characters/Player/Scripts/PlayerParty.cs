using System.Collections.Generic;
using UnityEngine;

public class PlayerParty
{
    public readonly int MAX_PARTY_SIZE = 6;
    private const bool enableDebug = false;
    public List<Pokemon> PartyPokemon { get; }
    public delegate void LoadSprite<T>(T thing, int index);
    public delegate void SwapSprite(int firstIndex, int secondIndex);

    public event LoadSprite<Pokemon> OnPartyPokemonChanged;
    public event SwapSprite OnPartyPokemonSwapped;

    public PlayerParty()
    {
        PartyPokemon = new List<Pokemon>();
    }

    public void Add(Pokemon pokemon)
    {
        if (PartyPokemon.Count < MAX_PARTY_SIZE)
        {
            PartyPokemon.Add(pokemon);
            if (enableDebug) { Debug.Log($"Added {pokemon.BasePokemon.Name} to party"); }
            OnPartyPokemonChanged(pokemon, PartyPokemon.Count - 1);
        }
        else
        {
            Debug.Log($"Party full ({PartyPokemon.Count}). Adding {pokemon} to PC");
            // Add pokemon to PC instead.
            PokemonStorage PC = PokemonStorage.GetInstance();
            PC.AddPokemon(pokemon);
        }
    }

    public bool Remove(Pokemon pokemon)
    {
        int index = PartyPokemon.IndexOf(pokemon);
        if (index == -1)
        {
            Debug.LogError($"{pokemon.BasePokemon.Name} cannot be removed because it does not exist in Party");
            return false;
        }
        else
        {
            OnPartyPokemonChanged(null, index);
            if (enableDebug) { Debug.Log($"Removed {pokemon.BasePokemon.Name} from party"); }
            return PartyPokemon.Remove(pokemon);
        }
    }

    public bool Swap(int firstIndex, int secondIndex)
    {
        if (firstIndex == -1 || firstIndex >= PartyPokemon.Count || secondIndex == -1 || secondIndex >= PartyPokemon.Count)
        {
            Debug.LogError($"Cannot swap pokemon in party at indices: {firstIndex} or {secondIndex} because either is out of bounds");
            return false;
        }
        else
        {
            Pokemon temp = PartyPokemon[firstIndex];
            PartyPokemon[firstIndex] = PartyPokemon[secondIndex];
            PartyPokemon[secondIndex] = temp;
            OnPartyPokemonSwapped(firstIndex, secondIndex);
            if (enableDebug) { Debug.Log($"Swapped pokemon at index: {firstIndex} with index: {secondIndex} (size: {PartySize()})"); }
            return true;
        }
    }

    public Pokemon GetPokemonAtIndex(int index)
    {
        return PartyPokemon[index];
    }

    public int PartySize()
    {
        return PartyPokemon.Count;
    }
}