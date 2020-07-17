using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    private UIManager()
    {
    }

    private bool enableDebug = false;
    public List<SpriteSlotData<Pokemon>> PartySlotDataList { get; } = new List<SpriteSlotData<Pokemon>>();
    public List<SpriteSlotData<Item>> ItemSlotDataList { get; } = new List<SpriteSlotData<Item>>();
    public List<SpriteSlotData<Pokemon>> StorageSlotDataList { get; } = new List<SpriteSlotData<Pokemon>>();

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
            Debug.LogError($"{nameof(Player)} is null, cant subscribe listeners in UIManager.");
        }

        PokemonStorage storage = PokemonStorage.GetInstance();
        storage.OnStoragePokemonChanged += ReloadStoragePokemonSlot;
        if (enableDebug) { Debug.Log("subscribed UI Manager to OnStoragePokemonChanged"); }
        storage.OnStoragePokemonSwapped += SwapStoragePokemonSlot;
        if (enableDebug) { Debug.Log("subscribed UI Manager to OnStoragePokemonChanged"); }
        storage.OnStorageDatasetChanged += SortStorageSlots;
        if (enableDebug) { Debug.Log("subscribed UI Manager to OnStoragePokemonChanged"); }
    }

    private void OnDisable()
    {
        Player player = Player.Instance;
        if (player != null)
        {
            // Causes Null pointer because Player is destroyed before UIManager can unsubscribe from it
            player.Party.OnPartyPokemonChanged -= ReloadPokemonPartySlot;
            if (enableDebug) { Debug.Log("unsubscribed UI Manager to OnPartyPokemonChanged"); }
            player.Party.OnPartyPokemonSwapped -= SwapPokemonPartySlot;
            if (enableDebug) { Debug.Log("unsubscribed UI Manager to OnPartyPokemonSwapped"); }
            player.Inventory.OnInventoryItemChanged -= ReloadInventoryItemSlot;
            if (enableDebug) { Debug.Log("unsubscribed UI Manager to OnInventoryItemChanged"); }
        }
        else
        {
            Debug.LogError($"{nameof(Player)} is null cant unsubscribe listeners in UIManager");
        }

        PokemonStorage storage = PokemonStorage.GetInstance();
        storage.OnStoragePokemonChanged -= ReloadStoragePokemonSlot;
        if (enableDebug) { Debug.Log("unsubscribed UI Manager to OnStoragePokemonChanged"); }
        storage.OnStoragePokemonSwapped -= SwapStoragePokemonSlot;
        if (enableDebug) { Debug.Log("unsubscribed UI Manager to OnStoragePokemonChanged"); }
        storage.OnStorageDatasetChanged -= SortStorageSlots;
        if (enableDebug) { Debug.Log("unsubscribed UI Manager to OnStoragePokemonChanged"); }
    }

    // Update is called once per frame
    private void Update()
    {
        LoadSpritesIfReady<Pokemon>(PartySlotDataList);
        LoadSpritesIfReady<Item>(ItemSlotDataList);
        LoadSpritesIfReady<Pokemon>(StorageSlotDataList);
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
            PokemonSlotSpriteData slotData = new PokemonSlotSpriteData(pokemon);
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
            for (int i = 0; i < ItemSlotDataList.Count; i++)
            {
                SpriteSlotData<Item> slotData = ItemSlotDataList[i];
                if ((slotData.CurrentObject as Item).Id == item.Id)
                {
                    Debug.Log($"Found item to change count of. existing: {slotData.CurrentObject}, new: {item.Count}");
                    ItemSlotDataList[i].CurrentObject.Count = item.Count;
                    break;
                }
            }
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

    private void ReloadStoragePokemonSlot(Pokemon pokemon, int index)
    {
        if (pokemon == null)
        {
            StorageSlotDataList.RemoveAt(index);
        }
        else
        {
            StorageSlotSpriteData slotData = new StorageSlotSpriteData(pokemon);
            slotData.PreLoadSprites();

            if (index >= StorageSlotDataList.Count)
            {
                StorageSlotDataList.Add(slotData);
            }
            else
            {
                StorageSlotDataList[index] = slotData;
            }
        }
    }

    private void SwapStoragePokemonSlot(int firstIndex, int secondIndex)
    {
        SpriteSlotData<Pokemon> temp = StorageSlotDataList[firstIndex];
        StorageSlotDataList[firstIndex] = StorageSlotDataList[secondIndex];
        StorageSlotDataList[secondIndex] = temp;
    }

    private void SortStorageSlots(List<Pokemon> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            ReloadStoragePokemonSlot(list[i], i);
        }
    }
}