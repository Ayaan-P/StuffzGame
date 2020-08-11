using System;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SpriteLoader
{
    private readonly bool enableDebug = false;
    private readonly SpritePool spritePool;

    public SpriteLoader()
    {
        this.spritePool = SpritePool.GetInstance();
    }

    public T LoadPokemonSprite<T>(int id, bool isShiny, Gender gender, SpriteType type)
    {
        string formattedId = FormatId(id, isShiny, gender, type);
        string address = $"{GetAddressForSpriteType(type)}{formattedId}.png";
        if (gender == Gender.FEMALE && !File.Exists(address))
        {
            // Not all female pokemon have special sprites, so use male one instead.
            formattedId = FormatId(id, isShiny, Gender.MALE, type);
            address = $"{GetAddressForSpriteType(type)}{formattedId}.png";
        }

        if (enableDebug) { Debug.Log($"Loading pokemon sprite at address: {address}"); }
        if (type == SpriteType.OVERWORLD)
        {
            return (T)Convert.ChangeType(GetSpriteAsync<Sprite[]>(address), typeof(T));
        }
        else
        {
            return (T)Convert.ChangeType(GetSpriteAsync<Sprite>(address), typeof(T));
        }
    }

    public Sprite LoadTypeSprite(PokemonType type)
    {
        string address = $"Assets/Pokemon/Sprites/Types/{type.ToString().ToLower()}.png";
        if (enableDebug) { Debug.Log($"Loading sprite at address: {address}"); }
        return GetSpriteAsync<Sprite>(address);
    }

    public Sprite LoadGenderSprite(Gender gender)
    {
        string address = $"Assets/Pokemon/Sprites/Gender/{gender.ToString().ToLower()}.png";
        if (enableDebug) { Debug.Log($"Loading sprite at address: {address}"); }
        return GetSpriteAsync<Sprite>(address);
    }

    public Sprite LoadFaintedSprite()
    {
        string address = "Assets/UI/InGameMenu/Sprites/fainted.png";
        if (enableDebug) { Debug.Log($"Loading sprite at address: {address}"); }
        return GetSpriteAsync<Sprite>(address);
    }

    public Sprite LoadAilmentSprite(MoveAilment ailment)
    {
        if(!DoesAilmentHaveSprite(ailment))
        {
            return null;
        }
        string address = $"Assets/Pokemon/Sprites/Ailments/{ailment.ToString().ToLower()}.png";
        if (enableDebug) { Debug.Log($"Loading sprite at address: {address}"); }
        return GetSpriteAsync<Sprite>(address);
    }

    public Sprite LoadItemSprite(string itemName)
    {
        string address = $"Assets/Items/Sprites/{itemName}.png";
        if (enableDebug) { Debug.Log($"Loading sprite at address: {address}"); }

        return GetSpriteAsync<Sprite>(address);
    }

    public Sprite LoadTMSprite(string name, PokemonType type)
    {
        string tmOrHM = name.Contains("tm") ? "tm" : "hm";
        string address = $"Assets/Items/Sprites/{tmOrHM}-{type.ToString().ToLower()}.png";
        if (enableDebug) { Debug.Log($"Loading sprite at address: {address}"); }
        return GetSpriteAsync<Sprite>(address);
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
        return GetSpriteAsync<Sprite>(address);
    }

    private T GetSpriteAsync<T>(string address)
    {
        Sprite cachedSprite = spritePool.CheckPool(address);
        if (cachedSprite != null)
        {
            return (T) Convert.ChangeType(cachedSprite, typeof(T));
        }
        else
        {
            AsyncOperationHandle<T> spriteHandle = Addressables.LoadAssetAsync<T>(address);
            if (!spriteHandle.IsDone)
            {
                if (enableDebug) { Debug.LogWarning($"Sprite only loaded {spriteHandle.PercentComplete * 100} %. Wait for completion."); }
                return default;
            }

            if (spriteHandle.Status == AsyncOperationStatus.Succeeded)
            {
                T result = spriteHandle.Result;
                if(!result.GetType().IsArray)
                {
                    Sprite resultSprite = result as Sprite;
                    if (resultSprite != null)
                    {
                        spritePool.AddToPool(address, resultSprite);
                    }
                    else
                    {
                        Debug.LogError($"Cannot cache sprite at address: {address}");
                    }
                }
                return result;
            }
            else
            {
                Debug.LogError($"Sprite not found at {address}");
                return default;
            }
        }
    }

    private string GetAddressForSpriteType(SpriteType type)
    {
        switch (type)
        {
            case SpriteType.OVERWORLD: return "Assets/Pokemon/Sprites/Overworld/";
            case SpriteType.BATTLE_FRONT: return "Assets/Pokemon/Sprites/Front/";
            case SpriteType.BATTLE_BACK: return "Assets/Pokemon/Sprites/Back/";
            case SpriteType.BOX: return "Assets/Pokemon/Sprites/Box/";
            default: Debug.LogError($"No sprite address available for {type}"); return null;
        }
    }

    private string FormatId(int id, bool isShiny, Gender gender, SpriteType spriteType)
    {
        string genderChar, formattedId, shinyChar;

        if (spriteType == SpriteType.OVERWORLD)
        {
            genderChar = "";
            shinyChar = isShiny ? "s" : "";
            if (id < 10)
            {
                formattedId = $"00{id}{genderChar}{shinyChar}";
            }
            else if (id < 100)
            {
                formattedId = $"0{id}{genderChar}{shinyChar}";
            }
            else
            {
                formattedId = $"{id}{genderChar}{shinyChar}";
            }
        }
        else
        {
            genderChar = (gender == Gender.FEMALE) ? "f" : "";
            shinyChar = isShiny ? "s" : "";
            formattedId = $"{id}{genderChar}{shinyChar}";
        }
        return formattedId;
    }

    public bool DoesAilmentHaveSprite(MoveAilment ailment)
    {
        switch (ailment)
        {
            case MoveAilment.POISON:
            case MoveAilment.BADLY_POISON:
            case MoveAilment.BURN:
            case MoveAilment.PARALYSIS:
            case MoveAilment.SLEEP:
            case MoveAilment.FREEZE:
                return true;
            default:
                return false;
        }
    }
}

public enum SpriteType
{
    OVERWORLD,
    BATTLE_FRONT,
    BATTLE_BACK,
    BOX
}