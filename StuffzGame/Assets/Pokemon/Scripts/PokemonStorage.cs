using System;
using System.Collections.Generic;
using UnityEngine;

public class PokemonStorage
{
    public List<Pokemon> PokemonList { get; }
    private static volatile PokemonStorage uniqueInstance;  //volatile so you instantiate and synchronize lazily
    private static readonly object padlock = new object();
    private static readonly bool enableDebug = false;

    public delegate void LoadSprite<T>(T thing, int index);
    public delegate void SortDataset<T>(List<T> list);
    public delegate void SwapSprite(int firstIndex, int secondIndex);

    public event LoadSprite<Pokemon> OnStoragePokemonChanged;
    public event SortDataset<Pokemon> OnStorageDatasetChanged;
    public event SwapSprite OnStoragePokemonSwapped;

    private PokemonStorage()
    {
        PokemonList = new List<Pokemon>();
    }

    #region Singleton

    public static PokemonStorage GetInstance()
    {
        if (uniqueInstance == null)
        {
            lock (padlock)
            {
                if (uniqueInstance == null) // check again to be thread-safe
                {
                    UnityEngine.Debug.LogWarning($"No {nameof(PokemonStorage)} instance found. Creating new instance");
                    uniqueInstance = new PokemonStorage();
                }
            }
        }
        if (enableDebug) { UnityEngine.Debug.Log($"{nameof(PokemonStorage)} instance found! Returning existing instance"); }
        return uniqueInstance;
    }

    #endregion Singleton

    public void AddPokemon(Pokemon pokemon)
    {
        PokemonList.Add(pokemon);
        OnStoragePokemonChanged(pokemon, PokemonList.Count - 1);
    }

    public bool RemovePokemon(Pokemon pokemon)
    {
        int index = PokemonList.IndexOf(pokemon);
        if (index == -1)
        {
            Debug.LogError($"{pokemon.BasePokemon.Name} cannot be removed because it does not exist in Storage");
            return false;
        }
        else
        {
            OnStoragePokemonChanged(null, index);
            if (enableDebug) { Debug.Log($"Removed {pokemon.BasePokemon.Name} from Storage"); }
            return PokemonList.Remove(pokemon);
        }
    }

    public bool Swap(int firstIndex, int secondIndex)
    {
        if (firstIndex == -1 || firstIndex >= PokemonList.Count || secondIndex == -1 || secondIndex >= PokemonList.Count)
        {
            Debug.LogError($"Cannot swap pokemon in Storage at indices: {firstIndex} or {secondIndex} because either is out of bounds");
            return false;
        }
        else
        {
            Pokemon temp = PokemonList[firstIndex];
            PokemonList[firstIndex] = PokemonList[secondIndex];
            PokemonList[secondIndex] = temp;
            OnStoragePokemonSwapped(firstIndex, secondIndex);
            if (enableDebug) { Debug.Log($"Swapped pokemon at index: {firstIndex} with index: {secondIndex} in storage (size: {PokemonList.Count})"); }
            return true;
        }
    }

    public void Reverse()
    {
        PokemonList.Reverse();
        OnStorageDatasetChanged(PokemonList);

    }

    public void SortBy(PokemonSortBy sort)
    {
        switch (sort)
        {
            case PokemonSortBy.Name:
                PokemonList.Sort((a, b) => a.BasePokemon.Name.CompareTo(b.BasePokemon.Name));
                OnStorageDatasetChanged(PokemonList);
                break;
            case PokemonSortBy.Level:
                PokemonList.Sort((a, b) => a.CurrentLevel.CompareTo(b.CurrentLevel));
                OnStorageDatasetChanged(PokemonList);
                break;
            case PokemonSortBy.Nature:
                PokemonList.Sort((a, b) => a.Nature.Name.CompareTo(b.Nature.Name));
                OnStorageDatasetChanged(PokemonList);
                break;
            case PokemonSortBy.Gender:
                PokemonList.Sort((a, b) => a.Gender.CompareTo(b.Gender));
                OnStorageDatasetChanged(PokemonList);
                break;
            case PokemonSortBy.IsShiny:
                PokemonList.Sort((a, b) => b.IsShiny.CompareTo(a.IsShiny)); //flipped so that shiny is first in list
                OnStorageDatasetChanged(PokemonList);
                break;
            case PokemonSortBy.Fainted:
                PokemonList.Sort((a, b) => a.IsFainted.CompareTo(b.IsFainted));
                OnStorageDatasetChanged(PokemonList);
                break;
            case PokemonSortBy.Height:
                PokemonList.Sort((a, b) => a.BasePokemon.Height.CompareTo(b.BasePokemon.Height));
                OnStorageDatasetChanged(PokemonList);
                break;
            case PokemonSortBy.Weight:
                PokemonList.Sort((a, b) => a.BasePokemon.Weight.CompareTo(b.BasePokemon.Weight));
                OnStorageDatasetChanged(PokemonList);
                break;
            default:
                Debug.LogWarning($"{sort} is not a member of enum PokemonSortBy");
                break;
        }
    }
}
