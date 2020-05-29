using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

   

public class SpriteSwap : MonoBehaviour
{
    public string pokemon_name;

    void LateUpdate()
    {
        var sprites = Resources.LoadAll<Sprite>("OverworldPokemon/" + pokemon_name);
        Sprite new_sprite;
        foreach( var renderer in GetComponentsInChildren<SpriteRenderer>())
        {
            string sprite_name = renderer.sprite.name;
            new_sprite = renderer.sprite;
           foreach( var sp in sprites)
           {
               if(sp.name==sprite_name)
               {
                   new_sprite = sp;
                   break;
               }
           }
        
            renderer.sprite = new_sprite;
        }
       
    }
}
