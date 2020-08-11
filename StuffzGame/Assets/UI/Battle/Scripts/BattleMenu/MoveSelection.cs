using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveSelection : MonoBehaviour
{
    public BattleSystem battle;
    private GameObject moveSelectionPanel;
    private Button[] moveButtons;
    private Button backButton;
    private Animator animator;
    // Start is called before the first frame update
    private void OnEnable()
    {
        moveSelectionPanel = this.transform.GetChild(0).gameObject;
        backButton = this.transform.GetChild(1).gameObject.GetComponent<Button>();
        moveButtons = moveSelectionPanel.GetComponentsInChildren<Button>();
        animator =  this.GetComponentInChildren<Animator>();

        ToggleListeners(true);
    }

    private void OnDisable()
    {
        ToggleListeners(false);
    }

    public void PopulateMoves(int partyIndex)
    {
        PokemonSlotSpriteData pokemonSlot = (PokemonSlotSpriteData)UIManager.Instance.PartySlotDataList[partyIndex];
        List<PokemonMove> learnedMoves = pokemonSlot.CurrentObject.LearnedMoves;
        int numMoves = learnedMoves.Count;
        Image moveDamageClass;
        Image moveType;
        Text moveName;
        Text movePP;

        for (int i = 0; i < PokemonMove.MAX_MOVES; i++)
        {
            GameObject moveObject = moveSelectionPanel.transform.GetChild(i).gameObject;
            if (i >= numMoves)
            {
                moveObject.SetActive(false);
            }
            else
            {
                Image[] imageComponents = moveObject.GetComponentsInChildren<Image>();
                Text[] textComponents = moveObject.GetComponentsInChildren<Text>();

                PokemonMove move = learnedMoves[i];
                moveDamageClass = imageComponents[1];
                moveType = imageComponents[2];
                moveName = textComponents[0];
                movePP = textComponents[1];

                moveDamageClass.sprite = pokemonSlot.MoveDamageClassLongSpriteList[i];
                moveDamageClass.preserveAspect = true;

                moveType.sprite = pokemonSlot.MoveTypesSpriteList[i];
                moveType.preserveAspect = true;

                moveName.text = UIUtils.FormatText(move.BaseMove.Name, false);
                movePP.text = $"{move.CurrentPP}/{move.BaseMove.PP}";
            }
        }
    }

    private void ToggleListeners(bool toggle)
    {
        for (int i=0;i< moveButtons.Length; i++)
        {
            if (toggle)
            {
                moveButtons[i].onClick.AddListener(() => OnMoveSelected(i));
            }
            else
            {
                moveButtons[i].onClick.RemoveAllListeners();
            }
        }

        if (toggle)
        {
            this.backButton.onClick.AddListener(OnBackPressed);
        }
        else
        {
            this.backButton.onClick.RemoveListener(OnBackPressed);
        }
    }

    private void OnMoveSelected(int buttonIndex)
    {
        battle.MoveSelected(buttonIndex);
    }

    private void OnBackPressed()
    {
        animator.SetTrigger("MoveOut");
    }

    // Called from Animation itself, see MoveSelection's MoveOut Animation to check when it's triggered.
    private void OnAnimationFinished()
    {
        this.gameObject.SetActive(false);
    }
}
