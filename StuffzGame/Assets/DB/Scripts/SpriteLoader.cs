using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SpriteLoader
{
    private readonly bool enableDebug = false;
    private SpritePool spritePool;
    public SpriteLoader()
    {
        this.spritePool = SpritePool.GetInstance();
    }

    public Sprite LoadPokemonSprite(int id, bool isShiny, SpriteType type)
    {
        string shinyChar = isShiny ? "s" : "";
        string formattedId = FormatId(id);

        string address = $"{GetAddressForSpriteType(type)}{formattedId}{shinyChar}.png";
        if (enableDebug) { Debug.Log($"Loading pokemon sprite at address: {address}"); }
        return GetSpriteAsync(address);
    }

    public Sprite LoadTypeSprite(PokemonType type)
    {
        string address = $"Assets/Pokemon/Sprites/Types/{type.ToString().ToLower()}.png";
        if (enableDebug) { Debug.Log($"Loading sprite at address: {address}"); }
        return GetSpriteAsync(address);
    }

    public Sprite LoadGenderSprite(Gender gender)
    {
        string address = $"Assets/Pokemon/Sprites/Gender/{gender.ToString().ToLower()}.png";
        if (enableDebug) { Debug.Log($"Loading sprite at address: {address}"); }
        return GetSpriteAsync(address);
    }

    public Sprite LoadFaintedSprite()
    {
        string address = "Assets/UI/InGameMenu/Sprites/fainted.png";
        if (enableDebug) { Debug.Log($"Loading sprite at address: {address}"); }
        return GetSpriteAsync(address);
    }

    public Sprite LoadItemSprite(string itemName)
    {
        string address = $"Assets/Items/Sprites/{itemName}.png";
        if (enableDebug) { Debug.Log($"Loading sprite at address: {address}"); }

        return GetSpriteAsync(address);
    }

    public Sprite LoadTMSprite(string name, PokemonType type)
    {
        string tmOrHM = name.Contains("tm") ? "tm" : "hm";
        string address = $"Assets/Items/Sprites/{tmOrHM}-{type.ToString().ToLower()}.png";
        if (enableDebug) { Debug.Log($"Loading sprite at address: {address}"); }
        return GetSpriteAsync(address);
    }

    public Sprite LoadMoveDamageClassSprite(MoveDamageClass damageClass, bool isLong)
    {
        string address;
        if (isLong)
        {
            address = $"Assets/Pokemon/Sprites/MoveDamageClass/{damageClass.ToString().ToLower()}_long.png";
        }
        else
        {
            address = $"Assets/Pokemon/Sprites/MoveDamageClass/{damageClass.ToString().ToLower()}.png";
        }
        if (enableDebug) { Debug.Log($"Loading sprite at address: {address}"); }
        return GetSpriteAsync(address);
    }

    private Sprite GetSpriteAsync(string address)
    {

        Sprite cachedSprite = spritePool.CheckPool(address);
        if (cachedSprite != null)
        {
            return cachedSprite;
        }
        else
        {
            AsyncOperationHandle<Sprite> spriteHandle = Addressables.LoadAssetAsync<Sprite>(address);
            if (!spriteHandle.IsDone)
            {
                if (enableDebug) { Debug.LogWarning($"Sprite only loaded {spriteHandle.PercentComplete * 100} %. Wait for completion."); }
                return null;
            }

            if (spriteHandle.Status == AsyncOperationStatus.Succeeded)
            {
                Sprite result = spriteHandle.Result;
                spritePool.AddToPool(address, result);
                return spriteHandle.Result;
            }
            else
            {
                Debug.LogError($"Sprite not found at {address}");
                return null;
            }
        }
    }

    private string GetAddressForSpriteType(SpriteType type)
    {
        switch (type)
        {
            case SpriteType.OVERWORLD_POKEMON: return "Assets/Pokemon/Sprites/Overworld/";
            case SpriteType.BATTLE_FRONT: return "Assets/Pokemon/Sprites/Front/";
            case SpriteType.BATTLE_BACK: return "Assets/Pokemon/Sprites/Back/";
            case SpriteType.SUMMARY_POKEMON: return "Assets/Pokemon/Sprites/Summary/";
            default: Debug.LogError($"No sprite address available for {type}"); return null;
        }
    }

    private string FormatId(int id)
    {
        if (id < 10)
        {
            return $"00{id}";
        }
        else if (id < 100)
        {
            return $"0{id}";
        }
        else
        {
            return $"{id}";
        }
    }
}

public enum SpriteType
{
    OVERWORLD_POKEMON,
    BATTLE_FRONT,
    BATTLE_BACK,
    SUMMARY_POKEMON
}