using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class loader : MonoBehaviour
{
    public Animator transition;
    public float transitiontime = 1f;
    // Start is called before the first frame update
 
    // Update is called once per frame
    void Update()
    {
        // if(Input.GetMouseButtonDown(0))
        // {
        //     loadBattle();
        // }
    }
    public void loadBattle()
    {
        StartCoroutine(loadscene(1));
    }
    IEnumerator loadscene(int scene_index)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitiontime);

        SceneManager.LoadScene(scene_index);
    }
}
