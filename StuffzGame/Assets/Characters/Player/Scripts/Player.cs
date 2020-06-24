using UnityEngine;

public class Player : Singleton
{
    private static Player _instance;
    private static readonly object Lock = new object(); //thread-safe volatile locking

    [SerializeField]
    private bool _persistent = true;

    public PlayerInventory Inventory { get; } = new PlayerInventory();
    public PlayerParty Party { get; } = new PlayerParty();


    #region Singleton

    public static Player Instance
    {
        get
        {
            if (Quitting)
            {
                Debug.LogWarning($"[{nameof(Singleton)}<{typeof(Player)}>] Instance will not be returned because the application is quitting.");
                return null;
            }
            lock (Lock)
            {
                if (_instance != null)
                {
                    return _instance;
                }
                var instances = FindObjectsOfType<Player>();
                var count = instances.Length;

                if (count == 0)
                {
                    // no instances found, create one
                    Debug.Log($"[{nameof(Singleton)}<{typeof(Player)}>] An instance is needed in the scene and no existing instances were found, so a new instance will be created.");
                    return _instance = new GameObject($"({nameof(Singleton)}){typeof(Player)}")
                               .AddComponent<Player>();
                }
                else if (count == 1)
                {
                    // singular instance found as expected
                    return _instance = instances[0];
                }
                else
                {
                    // erroneous condition where multiple singleton instances found.

                    Debug.LogWarning($"[{nameof(Singleton)}<{typeof(Player)}>] There should never be more than one {nameof(Singleton)} of type {typeof(Player)} in the scene, but {count} were found. The first instance found will be used, and all others will be destroyed.");
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

    // OnAwake() called before Start()
    protected virtual void OnAwake()
    {
    }

    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}