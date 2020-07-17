using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteDropshadow : MonoBehaviour
{
    public Sprite dropShadowSprite;
    public float transparency = 0.25f;
    private GameObject shadowGameObject;
    private SpriteRenderer shadowSpriteRenderer;
    private Vector3 SHADOW_SCALE = new Vector3(0.3f, 0.06f);

    private void Start()
    {
        shadowGameObject = new GameObject($"{this.name} Shadow");

        SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
        shadowSpriteRenderer = shadowGameObject.AddComponent<SpriteRenderer>();

        shadowSpriteRenderer.sortingLayerName = spriteRenderer.sortingLayerName;
        shadowSpriteRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;

        Color shadowColor = Color.black;
        shadowColor.a = transparency;
        shadowSpriteRenderer.color = shadowColor;
        shadowSpriteRenderer.sprite = dropShadowSprite;

        if (spriteRenderer.sprite != null)
        {
            SetShadowScale(spriteRenderer.sprite);
        }
        else
        {
            shadowGameObject.transform.localScale = new Vector3(0, 0);
        }

    }

    // Update is called once per frame
    private void Update()
    {
       SpriteRenderer spriteRenderer = this.GetComponent<SpriteRenderer>();
       Sprite renderedSprite = spriteRenderer.sprite;

        if (renderedSprite != null)
        {
           // Debug.Log("Updating shadow");
            SetShadowScale(renderedSprite);
            float positionOffset = (renderedSprite.rect.height - renderedSprite.textureRect.height) / (2f * renderedSprite.rect.height);
            shadowGameObject.transform.localPosition = this.transform.localPosition + new Vector3(0, -renderedSprite.bounds.extents.y + positionOffset + shadowGameObject.transform.localScale.y);
        }

        shadowGameObject.transform.localRotation = this.transform.localRotation;
        shadowGameObject.transform.localEulerAngles = transform.localEulerAngles;
    }

    private void SetShadowScale(Sprite sprite)
    {
        if (sprite != null)
        {
            Rect textureRect = sprite.textureRect;
            Rect spriteRect = sprite.rect;
            float widthRatio = textureRect.width / spriteRect.width;
            float heightRatio = textureRect.height / spriteRect.height;
            shadowGameObject.transform.localScale = new Vector3(SHADOW_SCALE.x * ( widthRatio), SHADOW_SCALE.y * (1f+ heightRatio));
        }
    }
}