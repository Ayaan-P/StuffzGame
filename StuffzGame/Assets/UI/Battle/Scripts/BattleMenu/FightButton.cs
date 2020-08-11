using UnityEngine;
using UnityEngine.UI;

public class FightButton : MonoBehaviour
{

    public GameObject moveSelection;
    private Animator moveSelectionAnimator;
    private Button fightButton;
    private void Start()
    {
        this.moveSelectionAnimator = moveSelection.GetComponent<Animator>();
    }
    private void OnEnable()
    {
        this.fightButton = this.GetComponent<Button>();
        this.fightButton.onClick.AddListener(OnFightSelected);
    }

    private void OnFightSelected()
    {
        moveSelection.SetActive(true);
        moveSelectionAnimator.SetTrigger("MoveIn");
    }

    private void OnDisable()
    {
        this.fightButton.onClick.RemoveAllListeners();
    }



}
