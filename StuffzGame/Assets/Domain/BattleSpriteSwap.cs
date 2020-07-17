using UnityEngine;
using UnityEngine.UI;

public class BattleSpriteSwap : AbstractSpriteSwap
{
    private Sprite loadedSprite;
    private bool isSpriteAlreadySet;

    public override void OnStartSetup(SpriteLoader loader)
    {
        isSpriteAlreadySet = false;
        loadedSprite = null;
        LoadSprite(loader);
    }

    public override void OnLateUpdate(SpriteLoader loader)
    {
        if (isSpriteAlreadySet)
        {
            return;
        }

        if (loadedSprite != null)
        {
            SetSprites();
        }
        else
        {
            loadedSprite = LoadSprite(loader);
            if (loadedSprite != null)
            {
                SetSprites();
            }
        }
    }

    private Sprite LoadSprite(SpriteLoader loader)
    {
        if (Pokemon != null && System.Enum.IsDefined(typeof(SpriteType), Sprite))
        {
            return loader.LoadPokemonSprite<Sprite>(Pokemon.BasePokemon.Id, Pokemon.IsShiny, Pokemon.Gender, Sprite);
        }
        else
        {
            Debug.LogError($"Cannot swap sprites because Pokemon is null");
            return null;
        }
    }
    public override void SetSprites()
    {
        if (loadedSprite != null)
        {
            Image spriteImage = GetComponent<Image>();
            spriteImage.sprite = loadedSprite;
            spriteImage.preserveAspect = true;
            isSpriteAlreadySet = true;
        }
    }

}
