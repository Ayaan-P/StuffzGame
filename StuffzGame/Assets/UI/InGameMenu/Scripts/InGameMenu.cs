using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    public GameObject menu;

    // Start is called before the first frame update
    void Start()
    {       
        // start without menu active
        menu.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        ToggleInGameMenu();

    }

    private void ToggleInGameMenu()
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
        GameObject player = Player.Instance.gameObject;
        if(player == null)
        {
            Debug.LogError($"{typeof(InGameMenu)}: Player is null");
        }
        else
        {
            player.GetComponent<PlayerMovement>().enabled = !toggle;
        }
    }
}
