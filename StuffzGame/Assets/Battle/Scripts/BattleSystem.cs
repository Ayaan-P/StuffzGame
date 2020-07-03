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

    public Transform playerPokemonPosition;
    public Transform enemyPokemonPosition;

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



    // Start is called before the first frame update
    void Start()
    {

        state = BATTLE_STATE.START;
        StartCoroutine(battleSetup());
    }

    IEnumerator battleSetup()
    {
        wildData = GameObject.Find("CurrentEncounter");
        GameObject enemySprite = Instantiate(enemyUnit, enemyPokemonPosition);
        GameObject playerSprite = Instantiate(playerUnit, playerPokemonPosition);

        PlayerParty = wildData.GetComponent<EncounterData>().Party;
        //Pokemon player_pokemon = PlayerParty[0];
        EnemyPokemon = wildData.GetComponent<EncounterData>().CurrentEnemyPokemon;
        enemySprite.GetComponent<SpriteSwapBattle>().orientation = "Front";
        enemySprite.GetComponent<SpriteSwapBattle>().id = wildData.GetComponent<EncounterData>().CurrentEnemyPokemon.BasePokemon.Id;
        
        PokemonFactory factory = new PokemonFactory();
        PlayerPokemon = factory.CreatePokemon(257, 60);
        playerSprite.GetComponent<SpriteSwapBattle>().orientation = "Back";
        playerSprite.GetComponent<SpriteSwapBattle>().id = PlayerPokemon.BasePokemon.Id;
        
        
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

    IEnumerator conductCombat(PokemonMove playerMove, PokemonMove enemyMove)
    {
        Debug.Log(" conduct combat");
        int playerSpeed = PlayerPokemon.BasePokemon.Stats.Where(it => it.BaseStat.Name == StatName.SPEED).SingleOrDefault().CurrentValue;
        int enemySpeed = EnemyPokemon.BasePokemon.Stats.Where(it => it.BaseStat.Name == StatName.SPEED).SingleOrDefault().CurrentValue;
        if (playerSpeed>=enemySpeed)
        {
            bool enemyDead = EnemyPokemon.TakeDamage(playerMove, PlayerPokemon);
            enemyHUD.SetHP();
            yield return new WaitForSeconds(2f);

            if(enemyDead)
            {
                state = BATTLE_STATE.WIN;
                playerWin();
            }
            else
            {

               bool playerDead = PlayerPokemon.TakeDamage(enemyMove, EnemyPokemon);
               playerHUD.SetHP();

               yield return new WaitForSeconds(2f);

               if(playerDead)
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
            bool playerDead = PlayerPokemon.TakeDamage(enemyMove, EnemyPokemon);
            playerHUD.SetHP();
            yield return new WaitForSeconds(2f);

            if(playerDead)
            {
                state = BATTLE_STATE.LOST;
                playerLose();
            }
            else
            {
               bool enemyDead = EnemyPokemon.TakeDamage(playerMove, PlayerPokemon);
               enemyHUD.SetHP();
               yield return new WaitForSeconds(2f);

               if(enemyDead)
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
    
    public void OnAttack(int moveNumber)
    {
        if(state!=BATTLE_STATE.PLAYER_TURN)
            return;
        
        //PokemonMove playerMove = player_pokemon.LearnedMoves[randomMove];
        Debug.Log(playerMoves[moveNumber].BaseMove.Name);
        
        int randomMove = Random.Range(0,3);
        PokemonMove enemyMove = EnemyPokemon.LearnedMoves[randomMove];
        Debug.Log(enemyMove.BaseMove.Name);
        StartCoroutine(conductCombat(playerMoves[moveNumber], enemyMove));
        
    }
 
}
