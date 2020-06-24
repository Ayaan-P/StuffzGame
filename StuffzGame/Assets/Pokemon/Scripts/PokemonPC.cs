using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PokemonPC
{
    public ICollection<Pokemon> PokemonList { get; }
    private static volatile PokemonPC uniqueInstance;  //volatile so you instantiate and synchronize lazily
    private static readonly object padlock = new object();
    private static readonly bool enableDebug = false;

    private PokemonPC()
    {
        PokemonList = new LinkedList<Pokemon>();
    }

    #region Singleton
    public static PokemonPC GetInstance()
    {
        if (uniqueInstance == null)
        {
            lock (padlock)
            {
                if (uniqueInstance == null) // check again to be thread-safe
                {
                    UnityEngine.Debug.LogWarning($"No {nameof(PokemonPC)} instance found. Creating new instance");
                    uniqueInstance = new PokemonPC();
                }
            }
        }
        if (enableDebug) { UnityEngine.Debug.Log($"{nameof(PokemonPC)} instance found! Returning existing instance"); }
        return uniqueInstance;
    }
    #endregion

    public void AddPokemon(Pokemon pokemon)
    {
        PokemonList.Add(pokemon);
    }

    public bool RemovePokemon(Pokemon pokemon)
    {
        return PokemonList.Remove(pokemon);
    }
}