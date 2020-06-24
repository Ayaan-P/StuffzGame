using System;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor.SceneManagement;
using UnityEngine;

public class GameManager : Singleton
{
    private static GameManager _instance;
    private static readonly object Lock = new object(); //thread-safe volatile locking

    [SerializeField]
    private bool _persistent = true;

    #region Singleton

    public static GameManager Instance
    {
        get
        {
            if (Quitting)
            {
                Debug.LogWarning($"[{nameof(Singleton)}<{typeof(GameManager)}>] Instance will not be returned because the application is quitting.");
                return null;
            }
            lock (Lock)
            {
                if (_instance != null)
                {
                    return _instance;
                }
                var instances = FindObjectsOfType<GameManager>();
                var count = instances.Length;

                if (count == 0)
                {
                    // no instances found, create one
                    Debug.Log($"[{nameof(Singleton)}<{typeof(GameManager)}>] An instance is needed in the scene and no existing instances were found, so a new instance will be created.");
                    return _instance = new GameObject($"({nameof(Singleton)}){typeof(GameManager)}")
                               .AddComponent<GameManager>();
                }
                else if (count == 1)
                {
                    // singular instance found as expected
                    return _instance = instances[0];
                }
                else
                {
                    // erroneous condition where multiple singleton instances found.

                    Debug.LogWarning($"[{nameof(Singleton)}<{typeof(GameManager)}>] There should never be more than one {nameof(Singleton)} of type {typeof(GameManager)} in the scene, but {count} were found. The first instance found will be used, and all others will be destroyed.");
                    for (var i = 1; i < instances.Length; i++)
                    {
                        Destroy(instances[i]);
                    }
                    return _instance = instances[0];
                }
            }
        }
    }

    #endregion Singleton

    private void Awake()
    {
        if (_persistent)
        {
            DontDestroyOnLoad(gameObject);
        }
        OnAwake();
    }

    protected virtual void OnAwake()
    {
    }

    private void Start()
    {
        Player player = Player.Instance;
        PokemonFactory pokemonFactory = new PokemonFactory();
        ItemFactory itemFactory = new ItemFactory();
        System.Random rand = new System.Random();
        int[] pokemonIds = new int[] { rand.Next(1, 231), rand.Next(1, 231), rand.Next(1, 231), rand.Next(1, 231), rand.Next(1, 231), rand.Next(1, 231) };
        int[] itemIds = new int[] { rand.Next(1, 101), rand.Next(1, 101), rand.Next(1, 101), rand.Next(1, 101), rand.Next(1, 101), rand.Next(1, 101) };
        float[] hpPercents = new float[] { (float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble(), (float)rand.NextDouble() };
        int size = rand.Next(3, 7);
        for (int i = 0; i < size; i++)
        {
            int randLevel = rand.Next(15, 101);
            Pokemon randPokemon = pokemonFactory.CreatePokemon(pokemonIds[i], randLevel);
            PokemonStat hpStat = randPokemon.BasePokemon.Stats.Where(it => it.BaseStat.Name == StatName.HP).SingleOrDefault();
            hpStat.CurrentValue = Convert.ToInt32(hpPercents[i] * hpStat.CalculatedValue);
            if (hpPercents[i] <= float.Epsilon)
            {
                randPokemon.IsFainted = true;
            }
            Item randItem = itemFactory.CreateItem(itemIds[i]);
            randPokemon.HeldItem = randItem;
            player.Party.Add(randPokemon);
            player.Inventory.Add(randItem);
        }
        Item newRandItem = itemFactory.CreateItem(107);
        for (int j = 0; j< 20; j++)
        {
            player.Inventory.Add(newRandItem);
        }
    }

}

public abstract class Singleton : MonoBehaviour
{
    public static bool Quitting { get; private set; } = false;

    private void OnApplicationQuit()
    {
        Quitting = true;
    }
}