using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader
{
    public const float transitionTime = 1f;
    private const int MAIN_SCENE_INDEX = 0;
    private const int BATTLE_SCENE_INDEX = 1;

    public void LoadMainScene()
    {
        SceneManager.LoadScene(MAIN_SCENE_INDEX);
        TogglePlayerVisibility(true);
    }

    public void LoadBattle()
    {
        SceneManager.LoadScene(BATTLE_SCENE_INDEX);
        TogglePlayerVisibility(false);
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