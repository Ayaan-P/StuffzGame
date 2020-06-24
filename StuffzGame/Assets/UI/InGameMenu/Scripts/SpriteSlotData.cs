using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpriteSlotData<T>
{
    public abstract T CurrentObject { get; }
    public abstract void PreLoadSprites();
    public abstract bool AreSpritesReady();
}
