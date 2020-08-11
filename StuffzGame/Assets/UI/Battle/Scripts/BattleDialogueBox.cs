using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleDialogueBox : MonoBehaviour
{
    public BattleSystem battleSystem;
    private Text dialogueContainer;
    private Button continueButton;
    private Animator dialogueAnimator;

    private Queue<string> dialogues;
    private void Start()
    {
        this.dialogues = new Queue<string>();
        this.dialogueContainer = this.GetComponentInChildren<Text>();
        this.continueButton = this.GetComponentInChildren<Button>();
        this.dialogueAnimator = this.GetComponentInChildren<Animator>();
    }
    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }

    private void SetDialogueText(string dialogue)
    {
        this.dialogueContainer.text = dialogue;
    }
}