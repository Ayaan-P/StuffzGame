using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SpriteSwapBattle : MonoBehaviour
{
    public string pokemon_name;
    public int id;
    public Sprite[] spriteArray;
    public bool done ;
    public bool shiny;
    public string orientation;
    void Start()
    {
      
        AsyncOperationHandle<Sprite[]> spriteHandle = Addressables.LoadAssetAsync<Sprite[]>("Assets/Pokemon/Sprites/"+orientation+"/"+id+".png");
        spriteHandle.Completed += LoadSpritesWhenReady;
        done=false;
    }   

    void LoadSpritesWhenReady(AsyncOperationHandle<Sprite[]> handleToCheck)
    { 
        if(handleToCheck.Status == AsyncOperationStatus.Succeeded)
        {
            spriteArray = handleToCheck.Result;
           
            done=true;
        }
        
    }
    void LateUpdate()
    {
        
        if(done){
          
            Sprite new_sprite;

            var renderer = GetComponent<SpriteRenderer>();
            renderer.sprite = spriteArray[0];
  
        }
        
    }
}