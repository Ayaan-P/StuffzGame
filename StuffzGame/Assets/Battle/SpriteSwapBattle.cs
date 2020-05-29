using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwapBattle : MonoBehaviour
{
    // Start is called before the first frame update
    
    public string pokemon_name;

    void LateUpdate()
    {
        var sprite = Resources.Load<Sprite>("BattleFront/" + pokemon_name);
        Sprite new_sprite;
        // foreach( var renderer in GetComponentsInChildren<SpriteRenderer>())
        // {
        //     string sprite_name = renderer.sprite.name;
        //     new_sprite = renderer.sprite;
        //    foreach( var sp in sprites)
        //    {
        //        if(sp.name==sprite_name)
        //        {
        //            new_sprite = sp;
        //            break;
        //        }
        //    }
            var renderer = GetComponent<SpriteRenderer>();
            renderer.sprite = sprite;
        
       
    }
}
