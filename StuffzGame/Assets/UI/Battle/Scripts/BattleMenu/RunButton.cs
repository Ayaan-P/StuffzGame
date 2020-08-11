using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RunButton : MonoBehaviour
{
    private Button runButton;
    public GameObject runDialog;
    public SceneLoader sceneLoader;
  
    private void OnEnable()
    {
        this.runButton = this.GetComponent<Button>();
        this.runButton.onClick.AddListener(TryRunningFromBattle);
    }

    private void OnDisable()
    {
        this.runButton.onClick.RemoveAllListeners();
    }

    private void TryRunningFromBattle()
    {
        this.runDialog.SetActive(true);
    }

    public void Run() {
        sceneLoader.LoadMainScene();
    }
    
}
