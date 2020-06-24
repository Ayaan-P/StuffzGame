using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public Animator transition;
    public float transitiontime = 1f;

    public void LoadBattle()
    {
        StartCoroutine(Loadscene(1));
    }
    IEnumerator Loadscene(int scene_index)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitiontime);

        SceneManager.LoadScene(scene_index);
    }
}
