using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDB : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    private void Awake()
    {
        buildInventory();
    }

    public Item getItem(int id)
    {
        return items.Find(item => item.id == id);
    }
    public Item getItem(string name)
    {
        return items.Find(item => item.name == name);
    }
    void buildInventory()
    {
        items.Add(new Item(0, "Poke Ball", "A tool used to capture wild pokemon.",
            new Dictionary<string,int>
            {
                {"Catch Rate", 10}
            }));
        items.Add(new Item(1, "Great Ball", "A better tool used to capture wild pokemon.",
            new Dictionary<string,int>
            {
                {"Catch Rate", 30}
            }));
        items.Add(new Item(2, "Ultra Ball", "The best tool used to capture wild pokemon.",
            new Dictionary<string,int>
            {
                {"Catch Rate", 70}
            }));
        
    }
   
}
