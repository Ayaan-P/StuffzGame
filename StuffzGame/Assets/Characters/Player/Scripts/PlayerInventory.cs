using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerInventory
{
    public delegate void LoadSprite<T>(T thing, int index, bool isOnlyDataChange);

    public event LoadSprite<Item> OnInventoryItemChanged;

    public List<Item> ItemList { get; }

    public PlayerInventory()
    {
        ItemList = new List<Item>();
    }

    public void Add(Item item)
    {
        bool isCountable = item.Attributes.Contains(ItemAttribute.COUNTABLE);
        if (isCountable)
        {
            Item foundItem = ItemList.Where(it => it.Id == item.Id).SingleOrDefault();
            if (foundItem != null)
            {
                foundItem.Count++;
                OnInventoryItemChanged(foundItem, ItemList.Count - 1, true);
            }
            else
            {
                item.Count = 1;
                ItemList.Add(item);
                OnInventoryItemChanged(item, ItemList.Count - 1, false);
            }
        }
        else
        {
            ItemList.Add(item);
            OnInventoryItemChanged(item, ItemList.Count - 1, false);
        }
    }

    public bool Remove(Item item)
    {
        int index = ItemList.IndexOf(item);
        if (index == -1)
        {
            Debug.LogError($"{item} cannot be removed because it does not exist in Inventory");
            return false;
        }
        else
        {
            OnInventoryItemChanged(null, index, false);
            return ItemList.Remove(item);
        }
    }

    public void SortBy(InventorySortBy type)
    {
        switch (type)
        {
            case InventorySortBy.ITEM_NAME:
                ItemList.Sort((a, b) => a.Name.CompareTo(b.Name));
                break;

            case InventorySortBy.ITEM_COST:
                ItemList.Sort((a, b) => a.Cost.CompareTo(b.Cost));
                break;

            case InventorySortBy.ITEM_COUNT:
                var sortedList = ItemList.OrderBy(item => item.Count != null).ThenBy(item => item.Count);
                ItemList.Clear();
                ItemList.AddRange(sortedList);
                break;

            case InventorySortBy.ITEM_ID:
                ItemList.Sort((a, b) => a.Id.CompareTo(b.Id));
                break;
        }
    }

    public int Size()
    {
        return ItemList.Count;
    }
}

public enum InventorySortBy
{
    ITEM_NAME,
    ITEM_COST,
    ITEM_COUNT,
    ITEM_ID
}