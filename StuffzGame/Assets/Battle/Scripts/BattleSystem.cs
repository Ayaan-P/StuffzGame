using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Pathfinding;
using System.Linq;

public enum BATTLE_STATE { START, PLAYER_TURN, COMBAT, WIN, LOST}
public class BattleSystem : MonoBehaviour
{
    public BATTLE_STATE state;
    public GameObject playerUnit;
    public GameObject enemyUnit;

    public GameObject wildData;

    public Transform playerPlatform;
    public Transform enemyPlatform;

    public List<Pokemon> PlayerParty;
  
    public Pokemon PlayerPokemon {get; set;}
    public Pokemon EnemyPokemon {get; set;}

    
	public Text dialogueText;
    public Text move1;
    public Text move2;
    public Text move3;
    public Text move4;

	public BattleHUD playerHUD;
	public BattleHUD enemyHUD;

    List<PokemonMove> playerMoves;

    unit player;
    unit enemy;

    // Start is called before the first frame update
    void Start()
    {

        state = BATTLE_STATE.START;
        StartCoroutine(battleSetup());
    }

    IEnumerator battleSetup()
    {
        wildData = GameObject.Find("CurrentEncounter");
        GameObject enemy_fab = Instantiate(enemyUnit, enemyPlatform);
        GameObject player_fab = Instantiate(playerUnit, playerPlatform);

        PlayerParty = wildData.GetComponent<EncounterData>().Party;
        //Pokemon player_pokemon = PlayerParty[0];
        EnemyPokemon = wildData.GetComponent<EncounterData>().CurrentEnemyPokemon;
        enemy_fab.GetComponent<SpriteSwapBattle>().orientation = "Front";
        enemy_fab.GetComponent<SpriteSwapBattle>().id = wildData.GetComponent<EncounterData>().CurrentEnemyPokemon.BasePokemon.Id;
        
        PokemonFactory factory = new PokemonFactory();
        PlayerPokemon = factory.CreatePokemon(257, 60);
        player_fab.GetComponent<SpriteSwapBattle>().orientation = "Back";
        player_fab.GetComponent<SpriteSwapBattle>().id = PlayerPokemon.BasePokemon.Id;
        

        player = player_fab.GetComponent<unit>();
        enemy = enemy_fab.GetComponent<unit>();

        
	    playerHUD.SetHUD(PlayerPokemon);
		enemyHUD.SetHUD(EnemyPokemon);
        
        playerMoves = PlayerPokemon.LearnedMoves;
    
        dialogueText.text = "A wild " + EnemyPokemon.BasePokemon.Name + " approaches...";
        yield return new WaitForSeconds(2f);
         
        state = BATTLE_STATE.PLAYER_TURN;
        Debug.Log(" Battle Setup");
        playerTurn();
    }
    
    void playerTurn()
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
        SceneManager.LoadScene(0);
    }

     public void playerLose()
    {
        Debug.Log(" lose");
        Destroy(wildData);
        SceneManager.LoadScene(0);
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
        
    }
 
}
