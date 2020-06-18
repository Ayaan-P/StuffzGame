using System;
using UnityEngine;

public class InGameMenuManager : MonoBehaviour
{
    public GameObject menu;

    private void Start()
    {
        // start without menu active
        menu.gameObject.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            bool menuToggle = !menu.gameObject.activeSelf;
            Debug.Log("Escape key was pressed. Menu toggled: " + menuToggle);

            int timeScale = menuToggle ? 0 : 1;

            PauseOrContinue(menuToggle, timeScale);
        }
    }

    private void PauseOrContinue(bool toggle, int timescale)
    {
        Time.timeScale = timescale;
        menu.gameObject.SetActive(toggle);

        //Disable movement since it's NOT dependent on timescale
        GameObject player = GameObject.Find("Player");
        player.GetComponent<PlayerMovement>().enabled = !toggle;
    }
}