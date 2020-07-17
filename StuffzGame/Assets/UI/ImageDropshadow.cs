using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageDropshadow : MonoBehaviour
{
    public Sprite dropShadowSprite;
    public float transparency = 0.25f;
    private GameObject shadowGameObject;
    private Image shadowImageSprite;
    private Vector3 SHADOW_SCALE = new Vector3(0.3f, 0.06f,1f);
    private bool isStaticShadowSet;

    private void Start()
    {
        isStaticShadowSet = false;
        shadowGameObject = new GameObject($"{this.name} Shadow");

        Image attachedImage = this.GetComponent<Image>();
        shadowImageSprite = shadowGameObject.AddComponent<Image>();

        Color shadowColor = Color.black;
        shadowColor.a = transparency;
        shadowImageSprite.color = shadowColor;
        shadowImageSprite.sprite = dropShadowSprite;

        Sprite attachedSprite = attachedImage.sprite;
        if (attachedSprite != null)
        {
            SetShadowScale(attachedSprite);
            float positionOffset = (attachedSprite.rect.height - attachedSprite.textureRect.height) / (2f * attachedSprite.rect.height);
            shadowGameObject.transform.localPosition = this.transform.localPosition + new Vector3(0, -attachedSprite.bounds.extents.y + positionOffset + shadowGameObject.transform.localScale.y);
        }
        else
        {
            shadowGameObject.transform.localScale = new Vector3(0, 0,1f);
        }
        shadowGameObject.transform.localRotation = this.transform.localRotation;
        shadowGameObject.transform.localEulerAngles = transform.localEulerAngles;
    }

    private void Update()
    {
        if (isStaticShadowSet)
        {
            return;
        }
        else
        {
            Image attachedImage = this.GetComponent<Image>();
            Sprite attachedSprite = attachedImage.sprite;
            if (attachedSprite != null)
            {
                SetShadowScale(attachedSprite);
                float positionOffset = (attachedSprite.rect.height - attachedSprite.textureRect.height) / (2f * attachedSprite.rect.height);
                shadowGameObject.transform.localPosition = this.transform.localPosition + new Vector3(0, -attachedSprite.bounds.extents.y + positionOffset + shadowGameObject.transform.localScale.y);
            }
        }
    }
    private void SetShadowScale(Sprite sprite)
    {
        if (sprite != null)
        {
            Rect textureRect = sprite.textureRect;
            Rect spriteRect = sprite.rect;
            float widthRatio = textureRect.width / spriteRect.width;
            float heightRatio = textureRect.height / spriteRect.height;
            shadowGameObject.transform.localScale = new Vector3(SHADOW_SCALE.x * (widthRatio), SHADOW_SCALE.y * (1f + heightRatio), SHADOW_SCALE.z);
            isStaticShadowSet = true;
        }
    }
}