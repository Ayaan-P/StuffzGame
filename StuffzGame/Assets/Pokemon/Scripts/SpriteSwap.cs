using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

   

public class SpriteSwap : MonoBehaviour
{
    public string pokemon_name;
    public Sprite[] spriteArray;
    public bool done ;
    void Start()
    {
        AsyncOperationHandle<Sprite[]> spriteHandle = Addressables.LoadAssetAsync<Sprite[]>("Assets/Pokemon/Sprites/Overworld/"+pokemon_name+".png");
        spriteHandle.Completed += LoadSpritesWhenReady;
        done=false;
    }   

    void LoadSpritesWhenReady(AsyncOperationHandle<Sprite[]> handleToCheck)
    { 
        if(handleToCheck.Status == AsyncOperationStatus.Succeeded)
        {
            spriteArray = handleToCheck.Result;
            //Debug.Log("hi");
            done=true;
        }
        
    }
    void LateUpdate()
    {
        
        if(done){
            //Debug.Log("hi2");
            Sprite new_sprite;
            foreach( var renderer in GetComponentsInChildren<SpriteRenderer>())
            {          
                string sprite_name = renderer.sprite.name;
                new_sprite = renderer.sprite;
            foreach( var sp in spriteArray)
            {
               if(sp.name[sp.name.Length-1]==sprite_name[sprite_name.Length-1])
               {
                   new_sprite = sp;
                   break;
               }
           }
            
                renderer.sprite = new_sprite;
            }
            //done=true;
        }
        
    }
}
