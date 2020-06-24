using System;
using System.Linq;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 2f;
    public GameObject player;
    public int itemId;
    private Item item;
    private bool isSpriteLoaded;
    private SpriteLoader spriteLoader;

    private void Start()
    {
        ItemFactory itemFactory = new ItemFactory();
        this.item = itemFactory.CreateItem(itemId);
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        spriteLoader = new SpriteLoader();
        if (spriteRenderer.sprite == null)
        {
            isSpriteLoaded = false;
            LoadSprite(spriteRenderer);
        }
        else
        {
            this.isSpriteLoaded = true;
            if (!spriteRenderer.sprite.name.Equals(item.Name))
            {
                Debug.LogError($"Overworld Item has assigned item named {item.Name} but has sprite for {spriteRenderer.sprite.name} instead!");
            }
        }
            
    }

    private void Update()
    {
        if (!isSpriteLoaded)
        {
            LoadSprite(GetComponent<SpriteRenderer>());
        }

        ListenForItemInteraction();

    }

    private void ListenForItemInteraction()
    {
        if (Input.GetAxisRaw("Submit") >= 0)
        {
            var heading = transform.position - player.GetComponent<Transform>().position;
            var distance = heading.magnitude;
            var direction = heading / distance;
            var lookingInDirection = player.GetComponent<PlayerMovement>().lookingInDirection;

            if (distance <= radius && Input.GetAxisRaw("Submit") > 0)
            {
                if ((Mathf.Round(direction.x) == lookingInDirection.x) || (Mathf.Round(direction.y) == lookingInDirection.y))
                {
                    Player.Instance.Inventory.Add(this.item);
                    Debug.Log($"You found a {this.item.Name}!");
                    Destroy(gameObject);
                }
            }
        }
    }

    private void LoadSprite(SpriteRenderer spriteRenderer)
    {
        
        Sprite itemSprite = spriteLoader.LoadItemSprite(item.Name);
        if (itemSprite != null)
        {
            spriteRenderer.sprite = itemSprite;
            isSpriteLoaded = true;
        }
    }
}