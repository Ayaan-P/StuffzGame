using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 2f;
    public GameObject player;
    void Update()
    {
        if(Input.GetAxisRaw("Submit")>=0)
        {
            
           
            var heading = transform.position - player.GetComponent<Transform>().position;
            var distance = heading.magnitude;
            var direction = heading / distance;
            var lookingin = player.GetComponent<PlayerMovement>().lookingin;

            if(distance<=radius && Input.GetAxisRaw("Submit")>0)
            {
              
                if( (Mathf.Round(direction.x) == lookingin.x) || (Mathf.Round(direction.y) == lookingin.y))
                {
                    
                        Debug.Log(" You found a Blazikenite");
                        Destroy(gameObject);
                 
                }
                
            }
     
        }
    }
}
