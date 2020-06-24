using Boo.Lang;
using System;
using System.Linq;
using UnityEngine;

public class UIManager : Singleton
{
    private static UIManager _instance;
    private static readonly object Lock = new object(); //thread-safe volatile locking

    [SerializeField]
    private bool _persistent = true;

    private bool enableDebug = false;
    public List<SpriteSlotData<Pokemon>> PartySlotDataList { get; } = new List<SpriteSlotData<Pokemon>>();
    public List<SpriteSlotData<Item>> ItemSlotDataList { get; } = new List<SpriteSlotData<Item>>();
    public GameObject menu;

    #region Singleton

    public static UIManager Instance
    {
        get
        {
            if (Quitting)
            {
                Debug.LogWarning($"[{nameof(Singleton)}<{typeof(UIManager)}>] Instance will not be returned because the application is quitting.");
                return null;
            }
            lock (Lock)
            {
                if (_instance != null)
                {
                    return _instance;
                }
                var instances = FindObjectsOfType<UIManager>();
                var count = instances.Length;

                if (count == 0)
                {
                    // no instances found, create one
                    Debug.Log($"[{nameof(Singleton)}<{typeof(UIManager)}>] An instance is needed in the scene and no existing instances were found, so a new instance will be created.");
                    return _instance = new GameObject($"({nameof(Singleton)}){typeof(UIManager)}")
                               .AddComponent<UIManager>();
                }
                else if (count == 1)
                {
                    // singular instance found as expected
                    return _instance = instances[0];
                }
                else
                {
                    // erroneous condition where multiple singleton instances found.

                    Debug.LogWarning($"[{nameof(Singleton)}<{typeof(UIManager)}>] There should never be more than one {nameof(Singleton)} of type {typeof(UIManager)} in the scene, but {count} were found. The first instance found will be used, and all others will be destroyed.");
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

    // Use this for initialization
    private void Start()
    {
        Player player = Player.Instance;
        if (player != null)
        {
            player.Party.OnPartyPokemonChanged += ReloadPokemonPartySlot;
            if (enableDebug) { Debug.Log("subscribed UI Manager to OnPartyPokemonChanged"); }
            player.Party.OnPartyPokemonSwapped += SwapPokemonPartySlot;
            if (enableDebug) { Debug.Log("subscribed UI Manager to OnPartyPokemonSwapped"); }
            player.Inventory.OnInventoryItemChanged += ReloadInventoryItemSlot;
            if (enableDebug) { Debug.Log("subscribed UI Manager to OnInventoryItemChanged"); }
        }
        else
        {
            Debug.LogError($"{nameof(Player)} is null but shouldn't be.");
        }

        // start without menu active
        menu.gameObject.SetActive(false);
    }
    private void OnDisable()
    {
        Player player = Player.Instance;

        // Causes Null pointer because Player is destroyed before UIManager can unsubscribe from it
       // player.Party.OnPartyPokemonChanged -= ReloadPokemonPartySlot;
        if (enableDebug) { Debug.Log("unsubscribed UI Manager to OnPartyPokemonChanged"); }
        //player.Party.OnPartyPokemonSwapped -= SwapPokemonPartySlot;
        if (enableDebug) { Debug.Log("unsubscribed UI Manager to OnPartyPokemonSwapped"); }
        //player.Inventory.OnInventoryItemChanged -= ReloadInventoryItemSlot;
        if (enableDebug) { Debug.Log("unsubscribed UI Manager to OnInventoryItemChanged"); }
    }

    // Update is called once per frame
    private void Update()
    {
        ToggleInGameMenu();
        LoadSpritesIfReady<Pokemon>(PartySlotDataList);
        LoadSpritesIfReady<Item>(ItemSlotDataList);

    }

    private void LoadSpritesIfReady<T>(List<SpriteSlotData<T>> spriteSlotDataList)
    {
        for (int index = 0; index < spriteSlotDataList.Count; index++)
        {
            SpriteSlotData<T> slotData = spriteSlotDataList[index];
            if (slotData.AreSpritesReady())
            {
                continue;
            }
            else
            {
                slotData.PreLoadSprites();
            }
        }
    }

    private void SwapPokemonPartySlot(int firstIndex, int secondIndex)
    {
        SpriteSlotData<Pokemon> temp = PartySlotDataList[firstIndex];
        PartySlotDataList[firstIndex] = PartySlotDataList[secondIndex];
        PartySlotDataList[secondIndex] = temp;
    }


    private void ReloadPokemonPartySlot(Pokemon pokemon, int index)
    {
        if (pokemon == null)
        {
            PartySlotDataList.RemoveAt(index);
        }
        else
        {
            PartySlotSpriteData slotData = new PartySlotSpriteData(pokemon);
            slotData.PreLoadSprites();
            if (index >= PartySlotDataList.Count)
            {
                PartySlotDataList.Add(slotData);
            }
            else
            {
                PartySlotDataList[index] = slotData;
            }
        }
    }

    private void ReloadInventoryItemSlot(Item item, int index, bool isOnlyDataChange)
    {
        if (item == null)
        {
            ItemSlotDataList.RemoveAt(index);
        }
        else if (isOnlyDataChange)
        {
            SpriteSlotData<Item> slotData =ItemSlotDataList.Where(it => ((it.CurrentObject) as Item).Id == item.Id).SingleOrDefault();
            slotData.CurrentObject.Count = item.Count;
        }
        else
        {
            ItemSlotSpriteData slotData = new ItemSlotSpriteData(item);
            slotData.PreLoadSprites();
            if (index >= ItemSlotDataList.Count)
            {
                ItemSlotDataList.Add(slotData);
            }
            else
            {
                ItemSlotDataList[index] = slotData;
            }
        }
    }

    private void ToggleInGameMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool menuToggle = !menu.gameObject.activeSelf;
            Debug.Log("Escape key was pressed. Menu toggled: " + menuToggle);

            int timeScale = menuToggle ? 0 : 1;

            PauseOrContinue(menuToggle, timeScale);
        }
    }

    private void PauseOrContinue(bool toggle, int timescale)
    {
        Time.timeScale = timescale;
        menu.gameObject.SetActive(toggle);

        //Disable movement since it's NOT dependent on timescale
        GameObject player = GameObject.Find("Player");
        player.GetComponent<PlayerMovement>().enabled = !toggle;
    }
}