using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpritePool
{
    private volatile static SpritePool uniqueInstance;  //volatile so you instantiate and synchronize lazily
    private static readonly object padlock = new object();
    private static readonly bool enableDebug = false;

    /**
     * Sprite Pool:
     * caches sprites so we dont waste memory when there are many duplicate pokemon in the PC etc.
     * Also speeds up all UI loading.
     *
     * Size must be greater then total number of:
     * type sprites (size: 18),
     * damage class sprites (size 3: physical,special,status),
     * gender sprites (size 3: male,female,genderless),
     * ailment sprites (size 6: fainted,poison,paralyze,burn,freeze,sleep)
     * Hence, min size = 30. Then adjust accordingly for balance between pokemon and item sprites.
     *
     */

    private readonly int MAX_POOL_SIZE = 200;
    private readonly Dictionary<string, int> spriteUseCountDict;
    private readonly Dictionary<string, Sprite> spritePool;

    private SpritePool()
    {
        spritePool = new Dictionary<string, Sprite>();
        spriteUseCountDict = new Dictionary<string, int>();
    }

    #region Singleton

    public static SpritePool GetInstance()
    {
        if (uniqueInstance == null)
        {
            lock (padlock)
            {
                if (uniqueInstance == null) // check again to be thread-safe
                {
                    UnityEngine.Debug.LogWarning($"No {nameof(SpritePool)} instance found. Creating new instance");
                    uniqueInstance = new SpritePool();
                }
            }
        }
        if (enableDebug) { UnityEngine.Debug.Log($"{nameof(SpritePool)} instance found! Returning existing instance"); }
        return uniqueInstance;
    }

    #endregion Singleton

    public Sprite CheckPool(string address)

    {
        if (spritePool.ContainsKey(address))
        {
            if (enableDebug) { Debug.Log($"Found cached sprite in pool for address: {address}. (size: {spritePool.Count}"); }
            spriteUseCountDict[address]++;
            return spritePool[address];
        }
        else
        {
            return null;
        }
    }

    public void AddToPool(string address, Sprite sprite)
    {
        if (spritePool.Count < MAX_POOL_SIZE)
        {
            if (!spritePool.ContainsKey(address))
            {
                AddToDicts(address, sprite);
                if (enableDebug) { Debug.Log($"Added sprite at address: {address} to sprite pool (size: {spritePool.Count}"); }
            }
        }
        else
        {
            string leastUsedAddress = spriteUseCountDict.Aggregate((l, r) => l.Value < r.Value ? l : r).Key;
            RemoveFromDicts(leastUsedAddress);
            AddToDicts(address, sprite);
            if (enableDebug){Debug.Log($"Sprite pool full, Replaced LRU and added sprite at address: {address} to sprite pool (size: {spritePool.Count}");}
        }
    }

    private void AddToDicts(string address, Sprite sprite)
    {
        spriteUseCountDict.Add(address, 0);
        spritePool.Add(address, sprite);
    }

    private void RemoveFromDicts(string address)
    {
        spritePool.Remove(address);
        spriteUseCountDict.Remove(address);
    }

    public void LogPoolSize()
    {
        UnityEngine.Debug.Log($"sprite pool size: {spritePool.Count}, counts: {spriteUseCountDict.Count}");
        foreach (var entry in spriteUseCountDict)
        {
            UnityEngine.Debug.Log($"address: {entry.Key}    count: {entry.Value}");
        }
    }
}