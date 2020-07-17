using UnityEngine;
using System.Collections;

public abstract class AbstractSpriteSwap : MonoBehaviour
{
    public Pokemon Pokemon { get; set; }
    public SpriteType Sprite { get; set; }
    private SpriteLoader loader;

    public abstract void OnStartSetup(SpriteLoader loader);
    public abstract void OnLateUpdate(SpriteLoader loader);
    public abstract void SetSprites();

    void Start()
    {
        loader = new SpriteLoader();
        OnStartSetup(loader);
    }

    void LateUpdate()
    {
        OnLateUpdate(loader);
    }
}
