using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunDialogWindow : MonoBehaviour
{
    private Button okButton;
    private Button cancelButton;
    public GameObject menu;

    private void OnEnable()
    {
        ToggleMenuButtons(false);
        Button[] buttons = this.GetComponentsInChildren<Button>();
        this.okButton = buttons[0];
        this.cancelButton = buttons[1];

        this.okButton.onClick.AddListener(ConfirmRun);
        this.cancelButton.onClick.AddListener(CancelRun);
    }

    private void OnDisable()
    {
        this.okButton.onClick.RemoveAllListeners();
        this.cancelButton.onClick.RemoveAllListeners();
    }

    private void ConfirmRun()
    {
        RunButton runButton = menu.GetComponentInChildren<RunButton>();
        runButton.Run();
    }
    private void CancelRun()
    {
        ToggleMenuButtons(true);
        this.gameObject.SetActive(false);
    }

    private void ToggleMenuButtons(bool toggle)
    {
        foreach(Button btn in menu.GetComponentsInChildren<Button>())
        {
            btn.enabled = toggle;
        }
    }

}
