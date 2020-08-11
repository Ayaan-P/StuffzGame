using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public float transitionTime = 0.5f;
    public Animator sceneTransition;
    private const int MAIN_SCENE_INDEX = 0;
    private const int BATTLE_SCENE_INDEX = 1;

    public void LoadMainScene()
    {
        StartCoroutine(LoadScene(MAIN_SCENE_INDEX, true));
    }

    public void LoadBattle()
    {
        StartCoroutine(LoadScene(BATTLE_SCENE_INDEX, false));
    }

    private IEnumerator LoadScene(int sceneIndex, bool playerToggle)
    {
        sceneTransition.SetTrigger("BeginTransition");
        yield return new WaitForSeconds(transitionTime);
        TogglePlayerVisibility(playerToggle);
        SceneManager.LoadScene(sceneIndex);

    }
    private void TogglePlayerVisibility(bool toggle)
    {
        GameObject player = Player.Instance.gameObject;
        if (player != null)
        {
            player.GetComponent<PlayerMovement>().enabled = toggle;
            player.GetComponent<SpriteRenderer>().enabled = toggle;

        }
        else
        {
            Debug.LogError($"{typeof(SceneLoader)}: Player is null, cant re/deactivate movement or visibility before scene change");
        }

    }
}