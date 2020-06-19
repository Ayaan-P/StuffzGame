using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

   

public class SpriteSwap : MonoBehaviour
{
    public string pokemon_name;
    private Sprite[] spriteArray;
    public bool done ;

    public int Id { get; set; }

    private string FormatId(int value)
    {
        if(value < 10)
        {
            return $"00{value}";
        }else if(value < 100)
        {
            return $"0{value}";
        }
        else
        {
            return $"{value}";
        }
    }

    void Start()
    {
        string formattedId = FormatId(Id);
        AsyncOperationHandle<Sprite[]> spriteHandle = Addressables.LoadAssetAsync<Sprite[]>($"Assets/Pokemon/Sprites/Ovworld/{formattedId}.png");
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
               if(sp.name==sprite_name)
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
