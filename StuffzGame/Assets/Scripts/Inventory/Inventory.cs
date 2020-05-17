using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> char_items = new List<Item>(); //player item list
    public ItemDB item_db; // database of all items

    private void Start()
    {
        giveItem(0);
        removeItem(0);
    }
        
//gives item to player based
    public void giveItem( int id )
    {
        Item item=item_db.getItem(id); //lookup item by id in db
        char_items.Add(item);
        Debug.Log(" Gave Item: " + item.name); // post to console
    }

//checks and returns Item if player has it
    public Item checkForItems( int id)
    {
        return char_items.Find(item => item.id == id);
    }

//removes item from player inventory
    public void removeItem(int id)
    {
        Item item = checkForItems(id);
        if(item!=null)
        {
            char_items.Remove(item);
            Debug.Log(" Removed Item: " + item.name); // post to console
        }
    }
}
