using UnityEngine;

public class ItemSlotSpriteData
{
    public Item CurrentItem { get; }

    private readonly SpriteLoader loader;
    private Sprite itemSprite;
    public Sprite ItemSprite { get => itemSprite; }

    public ItemSlotSpriteData(Item item)
    {
        this.CurrentItem = item;
        this.loader = new SpriteLoader();
    }

    public void PreLoadSprites()
    {
        PreLoadItemSprite();
    }

    private void PreLoadItemSprite()
    {
        if (CurrentItem != null)
        {
            this.itemSprite = loader.LoadItemSprite(CurrentItem.Name);
        }
        else
        {
            this.itemSprite = null;
        }
    }

    public bool AreSpritesReady()
    {
        return itemSprite != null;
    }
}