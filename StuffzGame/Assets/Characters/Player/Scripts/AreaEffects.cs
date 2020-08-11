using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaEffects : MonoBehaviour
{
    // Start is called before the first frame update
   void OnTriggerEnter2D(Collider2D collider)
   {
       string effect = collider.gameObject.name;
       Debug.Log("entered"+ effect );
       
           gameObject.GetComponent<PlayerMovement>().isControllable = false;
           gameObject.GetComponent<PlayerMovement>().moveSpeed = 9f;
     
    //    else if(effect=="IceEnd")
    //    {
    //        gameObject.GetComponent<PlayerMovement>().isControllable = true;
    //        gameObject.GetComponent<PlayerMovement>().moveSpeed = 5f;
    //    }
   }
   void OnTriggerExit2D(Collider2D collider)
   {
       string effect = collider.gameObject.name;
       Debug.Log("entered"+ effect );
     
           gameObject.GetComponent<PlayerMovement>().isControllable = true;
           gameObject.GetComponent<PlayerMovement>().moveSpeed = 5f;
       
   }
}
