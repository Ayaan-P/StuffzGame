using System;
using System.Linq;
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
        int size = rand.Next(30, 200);
        for (int i = 0; i < size; i++)
        {
            int randLevel = rand.Next(15, 101);
            int randPokemonId = rand.Next(1, 649);
            Pokemon randPokemon = pokemonFactory.CreatePokemon(randPokemonId, randLevel);
            PokemonStat hpStat = randPokemon.BasePokemon.Stats.Where(it => it.BaseStat.Name == StatName.HP).SingleOrDefault();
            float hpPercent = (float)rand.NextDouble();

            hpStat.CurrentValue = Convert.ToInt32(hpPercent * hpStat.CalculatedValue);
            if (hpStat.CurrentValue <= float.Epsilon)
            {
                randPokemon.IsFainted = true;
            }
            int randItemId = rand.Next(1, 400);
            Item randItem = itemFactory.CreateItem(randItemId);
            bool wasItemGiven = randPokemon.GiveItem(randItem);
            player.Party.Add(randPokemon);
        }
        Item anotherRandItem;
        for (int j = 1; j <=50; j++)
        {
            int randNum = rand.Next(1, 400);
            anotherRandItem = itemFactory.CreateItem(randNum);
            player.Inventory.Add(anotherRandItem);
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