using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    private enum BattleState { START, PLAYER_TURN, ENEMY_TURN, WON, LOST }
    public GameObject sceneLoader;
    private AIAgent aiAgent;
    private Dictionary<BattleState, Action> battleStateFunction;
    private Pokemon playerPokemon;
    private Pokemon enemyPokemon;
    public delegate void SeekInput();
    public event SeekInput OnPlayerTurn;

    public delegate void SetGeneralDialogue(BattleDialogueLoader.GeneralDialogue dialogue, BattleDialogueLoader.Actor actor, Pokemon pokemon, Item item);
    public event SetGeneralDialogue OnActionTaken;

    public delegate void SetEffectDialogue(BattleDialogueLoader.EffectDialogue dialogue, BattleDialogueLoader.Actor actor, Pokemon pokemon);
    public event SetEffectDialogue OnAilmentReceived;
    public event SetEffectDialogue OnStatChanged;


    // Start is called before the first frame update
    
    private void Start()
    {
        aiAgent = new RandomAgent();
        InitBattleFunctions();

        var player = Player.Instance;
        var encounterData = EncounterData.Instance;
        if(player == null || encounterData == null)
        {
            Debug.LogError($"Cannot start battle: {typeof(Player)} is null? {player == null} or {typeof(EncounterData)} is null? {encounterData == null}");
        }
        else
        {
            int currentPokemonIndex = 0;
            playerPokemon = player.Party.GetPokemonAtIndex(currentPokemonIndex);
            List<Pokemon> enemyParty = encounterData.GetCurrentEncounterData();
            enemyPokemon = enemyParty[currentPokemonIndex];
            battleStateFunction[BattleState.START].Invoke();
        }
    }

    private void InitBattleFunctions()
    {
        if (this.battleStateFunction == null)
        {
            this.battleStateFunction = new Dictionary<BattleState, Action>{
        { BattleState.START, StartBattle },
        { BattleState.PLAYER_TURN, PlayerTurn },
        { BattleState.ENEMY_TURN, EnemyTurn },
        { BattleState.WON, Won },
        { BattleState.LOST, Lost }
            };
        }
    }

    private void StartBattle()
    {
        this.battleStateFunction[BattleState.PLAYER_TURN].Invoke();
    }

    private void PlayerTurn()
    {
        OnPlayerTurn?.Invoke();
    }

    public void MoveSelected(int moveIndex)
    {
        PokemonMove selectedMove = playerPokemon.LearnedMoves[moveIndex];
        this.battleStateFunction[BattleState.ENEMY_TURN].Invoke();

    }

    private void EnemyTurn()
    {

        KeyValuePair<AIAction, int> actionIndexPair = aiAgent.GetNextAction(playerPokemon, enemyPokemon);

        switch (actionIndexPair.Key)
        {
            case AIAction.USE_MOVE:
                PokemonMove moveUsed = enemyPokemon.LearnedMoves[actionIndexPair.Value];
                break;
            case AIAction.USE_ITEM:
                int itemIndex = actionIndexPair.Value;
                break;
            case AIAction.SWITCH_OUT:
                int swapWithIndex = actionIndexPair.Value;
                break;
            default:
                Debug.LogError($"{actionIndexPair.Key} is not a valid AI action that can be performed");
                break;
        }
    }

    private void Won()
    {
    }

    private void Lost()
    {
    }

    private void ConductCombat()
    {

    }

    /*void playerTurn()
    {
        Debug.Log(" player turn");
        move1.text=playerMoves[0].BaseMove.Name;
        move2.text=playerMoves[1].BaseMove.Name;
        move3.text=playerMoves[2].BaseMove.Name;
        move4.text=playerMoves[3].BaseMove.Name;
        // StartCoroutine(conductCombat());
    }

    IEnumerator conductCombat(PokemonMove player_move, PokemonMove enemy_move)
    {
        Debug.Log(" conduct combat");
        int player_speed = PlayerPokemon.BasePokemon.Stats.Where(it => it.BaseStat.Name == StatName.SPEED).SingleOrDefault().CurrentValue;
        int enemy_speed = EnemyPokemon.BasePokemon.Stats.Where(it => it.BaseStat.Name == StatName.SPEED).SingleOrDefault().CurrentValue;
        if (player_speed>=enemy_speed)
        {
            bool enemy_dead = EnemyPokemon.TakeDamage(player_move, PlayerPokemon);
            enemyHUD.SetHP();
            yield return new WaitForSeconds(2f);

            if(enemy_dead)
            {
                state = BATTLE_STATE.WIN;
                playerWin();
            }
            else
            {
               bool player_dead = PlayerPokemon.TakeDamage(enemy_move, EnemyPokemon);
               playerHUD.SetHP();

               yield return new WaitForSeconds(2f);

               if(player_dead)
               {
                   state = BATTLE_STATE.LOST;
                   playerLose();
               }
               else
               {
                   state = BATTLE_STATE.PLAYER_TURN;
                   playerTurn();
               }
            }
        }
        else
        {
            bool player_dead = PlayerPokemon.TakeDamage(enemy_move, EnemyPokemon);
            playerHUD.SetHP();
            yield return new WaitForSeconds(2f);

            if(player_dead)
            {
                state = BATTLE_STATE.LOST;
                playerLose();
            }
            else
            {
               bool enemy_dead = EnemyPokemon.TakeDamage(player_move, PlayerPokemon);
               enemyHUD.SetHP();
               yield return new WaitForSeconds(2f);

               if(enemy_dead)
               {
                   state = BATTLE_STATE.WIN;
                   playerWin();
               }
               else
               {
                   state = BATTLE_STATE.PLAYER_TURN;
                   playerTurn();
               }
            }
        }
    }

    public void playerWin()
    {
        Debug.Log(" win");
        Destroy(wildData);
        SceneLoader loader = new SceneLoader();
        loader.LoadMainScene();
    }

     public void playerLose()
    {
        Debug.Log(" lose");
        Destroy(wildData);
        SceneLoader loader = new SceneLoader();
        loader.LoadMainScene();
    }
    public void OnAttack(int move_no)
    {
        if(state!=BATTLE_STATE.PLAYER_TURN)
            return;
        int rndmove = Random.Range(0,3);
        //PokemonMove player_move = player_pokemon.LearnedMoves[rndmove];
        Debug.Log(playerMoves[move_no].BaseMove.Name);
        PokemonMove enemy_move = EnemyPokemon.LearnedMoves[rndmove];
        Debug.Log(enemy_move.BaseMove.Name);
        StartCoroutine(conductCombat(playerMoves[move_no], enemy_move));
    }*/
}