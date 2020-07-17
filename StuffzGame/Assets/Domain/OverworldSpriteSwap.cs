using UnityEngine;

public class OverworldSpriteSwap : AbstractSpriteSwap
{
    private Sprite[] loadedSprites;

    public override void OnStartSetup(SpriteLoader loader)
    {
        loadedSprites = null;
        LoadSprites(loader);
    }

    public override void OnLateUpdate(SpriteLoader loader)
    {
        if (loadedSprites != null)
        {
            SetSprites();
        }
        else
        {
            loadedSprites = LoadSprites(loader);
            if (loadedSprites != null)
            {
                SetSprites();
            }
        }
    }

    private Sprite[] LoadSprites(SpriteLoader loader)
    {
        if (Pokemon != null && System.Enum.IsDefined(typeof(SpriteType), Sprite))
        {
            return loader.LoadPokemonSprite<Sprite[]>(Pokemon.BasePokemon.Id, Pokemon.IsShiny, Pokemon.Gender, Sprite);
        }
        else
        {
            Debug.LogError($"Cannot swap sprites because Pokemon is null");
            return null;
        }
    }

    public override void SetSprites()
    {
        if (loadedSprites != null)
        {
            SpriteRenderer renderer = GetComponentInChildren<SpriteRenderer>();
            string currentSpriteName = renderer.sprite.name;
            Sprite newSprite = renderer.sprite;
            foreach (var loadedSprite in loadedSprites)
            {
                if (loadedSprite.name.Equals(currentSpriteName))
                {
                    newSprite = loadedSprite;
                    break;
                }
            }
            renderer.sprite = newSprite;
        }
    }
}