using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public Animator transition;
    public const float transitionTime = 1f;

    public void LoadBattle()
    {
        StartCoroutine(Loadscene(1));
    }
    IEnumerator Loadscene(int sceneIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(sceneIndex);
    }
}
