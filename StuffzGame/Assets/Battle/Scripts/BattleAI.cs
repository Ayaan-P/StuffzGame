using UnityEngine;
using System.Collections.Generic;

public class BattleAI
{
    private readonly AIAgent ai;
    private List<Pokemon> playerParty;
    private List<Pokemon> aiParty;
    
    public BattleAI (AIAgent agent, List<Pokemon> playerParty, List<Pokemon> aiParty)
    {
        this.ai = agent;
        this.playerParty = playerParty;
        this.aiParty = aiParty;
    }

    public void PerformAction()
    {
        Pokemon playerPokemon = playerParty[0];
        Pokemon aiPokemon = aiParty[0];
        KeyValuePair<AIAction,int> actionIndexPair = ai.GetNextAction(playerPokemon, aiPokemon);

        switch (actionIndexPair.Key)
        {
            case AIAction.USE_MOVE:
                break;
            case AIAction.USE_ITEM:
                break;
            case AIAction.SWITCH_OUT:
                break;
            default:
                Debug.LogError($"{actionIndexPair.Key} is not a valid AI action that can be performed");
                break;
        }
    }




}
