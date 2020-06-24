using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Pathfinding;
using System.Linq;

public enum battle_state { start, playerturn, combat, win, lose}
public class BattleSystem : MonoBehaviour
{
    public battle_state state;
    public GameObject player_unit;
    public GameObject enemy_unit;

    public GameObject wild_data;

    public Transform player_platform;
    public Transform enemy_platform;

    public List<Pokemon> PlayerParty;
  
    public Pokemon player_pokemon {get; set;}
    public Pokemon enemy_pokemon {get; set;}

    
	public Text dialogueText;
    public Text move1;
    public Text move2;
    public Text move3;
    public Text move4;

	public BattleHUD playerHUD;
	public BattleHUD enemyHUD;

    List<PokemonMove> player_moves;

    unit player;
    unit enemy;

    // Start is called before the first frame update
    void Start()
    {

        state = battle_state.start;
        StartCoroutine(battleSetup());
    }

    IEnumerator battleSetup()
    {
        wild_data = GameObject.Find("CurrentEncounter");
        GameObject enemy_fab = Instantiate(enemy_unit, enemy_platform);
        GameObject player_fab = Instantiate(player_unit, player_platform);

        PlayerParty = wild_data.GetComponent<EncounterData>().Party;
        //Pokemon player_pokemon = PlayerParty[0];
        enemy_pokemon = wild_data.GetComponent<EncounterData>().current_enemy;
        enemy_fab.GetComponent<SpriteSwapBattle>().orientation = "Front";
        enemy_fab.GetComponent<SpriteSwapBattle>().id = wild_data.GetComponent<EncounterData>().current_enemy.BasePokemon.Id;
        
        PokemonFactory factory = new PokemonFactory();
        player_pokemon = factory.CreatePokemon(257, 60);
        player_fab.GetComponent<SpriteSwapBattle>().orientation = "Back";
        player_fab.GetComponent<SpriteSwapBattle>().id = player_pokemon.BasePokemon.Id;
        

        player = player_fab.GetComponent<unit>();
        enemy = enemy_fab.GetComponent<unit>();

        
	    playerHUD.SetHUD(player_pokemon);
		enemyHUD.SetHUD(enemy_pokemon);
        
        player_moves = player_pokemon.LearnedMoves;
    
        dialogueText.text = "A wild " + enemy_pokemon.BasePokemon.Name + " approaches...";
        yield return new WaitForSeconds(2f);
         
        state = battle_state.playerturn;
        Debug.Log(" Battle Setup");
        playerTurn();
    }
    
    void playerTurn()
    {
        Debug.Log(" player turn");
        move1.text=player_moves[0].BaseMove.Name;
        move2.text=player_moves[1].BaseMove.Name;
        move3.text=player_moves[2].BaseMove.Name;
        move4.text=player_moves[3].BaseMove.Name;
        // StartCoroutine(conductCombat());
    }

    IEnumerator conductCombat(PokemonMove player_move, PokemonMove enemy_move)
    {
        Debug.Log(" conduct combat");
        int player_speed = player_pokemon.BasePokemon.Stats.Where(it => it.BaseStat.Name == StatName.SPEED).SingleOrDefault().CurrentValue;
        int enemy_speed = enemy_pokemon.BasePokemon.Stats.Where(it => it.BaseStat.Name == StatName.SPEED).SingleOrDefault().CurrentValue;
        if (player_speed>=enemy_speed)
        {
            bool enemy_dead = enemy_pokemon.TakeDamage(player_move, player_pokemon);
            enemyHUD.SetHP();
            yield return new WaitForSeconds(2f);

            if(enemy_dead)
            {
                state = battle_state.win;
                playerWin();
            }
            else
            {

               bool player_dead = player_pokemon.TakeDamage(enemy_move, enemy_pokemon);
               playerHUD.SetHP();

               yield return new WaitForSeconds(2f);

               if(player_dead)
               {
                   state = battle_state.lose;
                   playerLose();
               }
               else
               {
                   state = battle_state.playerturn;
                   playerTurn();
               }
            }

        }
        else
        {
            bool player_dead = player_pokemon.TakeDamage(enemy_move, enemy_pokemon);
            playerHUD.SetHP();
            yield return new WaitForSeconds(2f);

            if(player_dead)
            {
                state = battle_state.lose;
                playerLose();
            }
            else
            {
               bool enemy_dead = enemy_pokemon.TakeDamage(player_move, player_pokemon);
               enemyHUD.SetHP();
               yield return new WaitForSeconds(2f);

               if(enemy_dead)
               {
                   state = battle_state.win;
                   playerWin();
               }
               else
               {
                   state = battle_state.playerturn;
                   playerTurn();
               }
            }
        }
    }

    public void playerWin()
    {
        Debug.Log(" win");
        Destroy(wild_data);
        SceneManager.LoadScene(0);
    }

     public void playerLose()
    {
        Debug.Log(" lose");
        Destroy(wild_data);
        SceneManager.LoadScene(0);
    }
    public void OnAttack(int move_no)
    {
        if(state!=battle_state.playerturn)
            return;
        int rndmove = Random.Range(0,3);
        //PokemonMove player_move = player_pokemon.LearnedMoves[rndmove];
        Debug.Log(player_moves[move_no].BaseMove.Name);
        PokemonMove enemy_move = enemy_pokemon.LearnedMoves[rndmove];
        Debug.Log(enemy_move.BaseMove.Name);
        StartCoroutine(conductCombat(player_moves[move_no], enemy_move));
        
    }
 
}
