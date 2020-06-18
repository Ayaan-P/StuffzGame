using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class PokemonPC : Singleton
{

    private static PokemonPC _instance;
    private static readonly object Lock = new object(); //thread-safe volatile locking
    [SerializeField]
    private bool _persistent = true;
    private List<PokemonBox> PokemonBoxList { get; } = new List<PokemonBox>();
    #region Singleton
    public static PokemonPC Instance
    {
        get
        {
            if (Quitting)
            {
                Debug.LogWarning($"[{nameof(Singleton)}<{typeof(PokemonPC)}>] Instance will not be returned because the application is quitting.");
                return null;
            }
            lock (Lock)
            {
                if (_instance != null)
                {
                    return _instance;
                }
                var instances = FindObjectsOfType<PokemonPC>();
                var count = instances.Length;

                if (count == 0)
                {
                    // no instances found, create one
                    Debug.Log($"[{nameof(Singleton)}<{typeof(PokemonPC)}>] An instance is needed in the scene and no existing instances were found, so a new instance will be created.");
                    return _instance = new GameObject($"({nameof(Singleton)}){typeof(PokemonPC)}")
                               .AddComponent<PokemonPC>();
                }
                else if (count == 1)
                {
                    // singular instance found as expected
                    return _instance = instances[0];
                }
                else
                {
                    // erroneous condition where multiple singleton instances found.

                    Debug.LogWarning($"[{nameof(Singleton)}<{typeof(PokemonPC)}>] There should never be more than one {nameof(Singleton)} of type {typeof(PokemonPC)} in the scene, but {count} were found. The first instance found will be used, and all others will be destroyed.");
                    for (var i = 1; i < instances.Length; i++)
                    {
                        Destroy(instances[i]);
                    }
                    return _instance = instances[0];
                }
            }
        }
    }
    #endregion

    private void Awake()
    {
        if (_persistent)
        {
            DontDestroyOnLoad(gameObject);
        }
        OnAwake();
    }

    // OnAwake() called before Start()
    protected virtual void OnAwake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPokemonToBox(Pokemon pokemon)
    {
        if (PokemonBoxList.Count == 0 || PokemonBoxList.Last().IsBoxFull())
        {
            PokemonBox newBox = new PokemonBox(PokemonBoxList.Count+1);
            newBox.AddPokemon(pokemon);
            PokemonBoxList.Add(newBox);
        }
        else
        {
            PokemonBoxList.Last().AddPokemon(pokemon);
        }
    }

    public bool RemovePokemonFromBox(Pokemon pokemon, int boxNumber)
    {
        return PokemonBoxList[boxNumber].RemovePokemon(pokemon);
    }
}

public class PokemonBox
{
    private readonly int boxNumber;
    private readonly int BOX_SIZE = 30;
    public List<Pokemon> BoxPokemon { get; } = new List<Pokemon>();

    public PokemonBox(int boxNumber)
    {
        this.boxNumber = boxNumber;
    }

    public bool IsBoxFull()
    {
        return BoxPokemon.Count == BOX_SIZE;
    }

    public void AddPokemon(Pokemon pokemon)
    {
        BoxPokemon.Add(pokemon);
    }

    public bool RemovePokemon(Pokemon pokemon)
    {
        if (ContainsPokemon(pokemon))
        {
            return BoxPokemon.Remove(pokemon);
        }
        else
        {
            Debug.LogError($"{pokemon.BasePokemon.Name} ({pokemon.BasePokemon.Id}) not found in Box {boxNumber}");
            return false;
        }
    }

    public bool ContainsPokemon(Pokemon pokemon)
    {
        return BoxPokemon.Contains(pokemon);
    }
}
