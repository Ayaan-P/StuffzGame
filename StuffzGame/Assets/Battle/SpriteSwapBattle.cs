using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

// public class SpriteSwapBattle : MonoBehaviour
// {
//     // Start is called before the first frame update
    
//     public string pokemon_name;

//     void LateUpdate()
//     {
//         var sprite = Resources.Load<Sprite>("BattleFront/" + pokemon_name);
//         Sprite new_sprite;
//         // foreach( var renderer in GetComponentsInChildren<SpriteRenderer>())
//         // {
//         //     string sprite_name = renderer.sprite.name;
//         //     new_sprite = renderer.sprite;
//         //    foreach( var sp in sprites)
//         //    {
//         //        if(sp.name==sprite_name)
//         //        {
//         //            new_sprite = sp;
//         //            break;
//         //        }
//         //    }
//             var renderer = GetComponent<SpriteRenderer>();
//             renderer.sprite = sprite;
        
       
//     }
// }


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
            //Debug.Log("hi");
            done=true;
        }
        
    }
    void LateUpdate()
    {
        
        if(done){
            //Debug.Log("hi2");
            Sprite new_sprite;

            var renderer = GetComponent<SpriteRenderer>();
            renderer.sprite = spriteArray[0];
  
        }
        
    }
}