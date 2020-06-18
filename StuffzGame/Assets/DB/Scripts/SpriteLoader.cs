using UnityEngine;
using UnityEngine.AddressableAssets;

public class SpriteLoader
{
    private Sprite currentLoadedSprite;

    public Sprite LoadPokemonSprite(int id, bool isShiny, SpriteType type)
    {
        string shinyChar = isShiny ? "s" : ""; 
        string address = $"{GetAddressForSpriteType(type)}{id}{shinyChar}.png";
        Debug.Log($"Loading sprite at address: {address}");
        Addressables.LoadAssetAsync<Sprite>(address).Completed += OnSpriteLoaded;
        return currentLoadedSprite;
    }

    public Sprite LoadTypeSprite(PokemonType type)
    {
        string address = $"Assets/Pokemon/Sprites/Types/{type.ToString().ToLower()}.png";
        Debug.Log($"Loading sprite at address: {address}");
        Addressables.LoadAssetAsync<Sprite>(address).Completed += OnSpriteLoaded;
        return currentLoadedSprite;
    }

    public Sprite LoadGenderSprite(Gender gender)
    {
        string address = $"Assets/Pokemon/Sprites/Gender/{gender.ToString().ToLower()}.png";
        Debug.Log($"Loading sprite at address: {address}");
        Addressables.LoadAssetAsync<Sprite>(address).Completed += OnSpriteLoaded;
        return currentLoadedSprite;
    }

    public Sprite LoadFaintedSprite()
    {
        string address = "Assets/UI/InGameMenu/Sprites/fainted.png";
        Debug.Log($"Loading sprite at address: {address}");
        Addressables.LoadAssetAsync<Sprite>(address).Completed += OnSpriteLoaded;
        return currentLoadedSprite;
    }

    public Sprite LoadItemSprite(string itemName)
    {
        string address = $"Assets/Items/Sprites/{itemName}.png";
        Debug.Log($"Loading sprite at address: {address}");
        Addressables.LoadAssetAsync<Sprite>(address).Completed += OnSpriteLoaded;
        return currentLoadedSprite;
    }

    private void OnSpriteLoaded(UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationHandle<Sprite> sprite)
    {
        if(sprite.Result == null)
        {
            Debug.LogError("Sprite not found");
        }
        else
        {
            currentLoadedSprite = sprite.Result;
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
}

public enum SpriteType
{
    OVERWORLD_POKEMON,
    BATTLE_FRONT,
    BATTLE_BACK,
    SUMMARY_POKEMON
}
