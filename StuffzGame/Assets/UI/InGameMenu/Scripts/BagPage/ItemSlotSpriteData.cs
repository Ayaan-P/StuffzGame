using UnityEngine;

public class ItemSlotSpriteData : SpriteSlotData<Item>
{
    public override Item CurrentObject { get; }

    private readonly SpriteLoader loader;
    private Sprite itemSprite;
    public Sprite ItemSprite { get => itemSprite; }

    public ItemSlotSpriteData(Item item)
    {
        this.CurrentObject = item;
        this.loader = new SpriteLoader();
    }

    public override void PreLoadSprites()
    {
        PreLoadItemSprite();
    }

    private void PreLoadItemSprite()
    {
        if (CurrentObject != null)
        {
            this.itemSprite = loader.LoadItemSprite(CurrentObject.Name);
        }
        else
        {
            this.itemSprite = null;
        }
    }

    public override bool AreSpritesReady()
    {
        return itemSprite != null;
    }
}