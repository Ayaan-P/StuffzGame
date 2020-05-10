using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenuManager : MonoBehaviour
{
    public GameObject menu;

    void Start()
    {
        // start without menu active
        menu.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Boolean menuToggle = !menu.gameObject.activeSelf;
            Debug.Log("Escape key was pressed. Menu toggled: " + menuToggle);

            int timescale = menuToggle? 0 : 1;

            PauseOrContinue(menuToggle, timescale);
        }
    }

    void PauseOrContinue(Boolean toggle, int timescale)
    {
        Time.timeScale = timescale;
        menu.gameObject.SetActive(toggle);

        //Disable movement since it's NOT dependent on timescale
        GameObject player = GameObject.Find("Player");
        (player.GetComponent("PlayerMovement") as MonoBehaviour).enabled = !toggle;
    }

}
