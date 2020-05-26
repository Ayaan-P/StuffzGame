using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;
public enum battle_state { start, playerturn, combat, win, lose}
public class BattleSystem : MonoBehaviour
{
    public battle_state state;
    public GameObject player_unit;
    public GameObject enemy_unit;
    public GameObject wild_data;
    public Transform player_platform;
    public Transform enemy_platform;

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
        wild_data = GameObject.Find("CurrentEncounter(Clone)");
        GameObject enemy_fab = Instantiate(enemy_unit, enemy_platform);
        GameObject player_fab = Instantiate(player_unit, player_platform);
        enemy_fab.GetComponent<SpriteSwapBattle>().pokemon_name = wild_data.GetComponent<EncounterData>().pokemon_name;
        Debug.Log(wild_data.GetComponent<EncounterData>().pokemon_name +"x" );
        player = player_fab.GetComponent<unit>();
        enemy = enemy_fab.GetComponent<unit>();
    
        yield return new WaitForSeconds(2f);
         
        state = battle_state.playerturn;
        Debug.Log(" Battle Setup");
        playerTurn();
    }
    
    void playerTurn()
    {
        Debug.Log(" player turn");
        StartCoroutine(conductCombat());
    }

    IEnumerator conductCombat()
    {
        Debug.Log(" conduct combat");
        if (player.spd>=enemy.spd)
        {
            bool enemy_dead = enemy.takeDamage(player.atk);

            yield return new WaitForSeconds(2f);

            if(enemy_dead)
            {
                state = battle_state.win;
                playerWin();
            }
            else
            {
               bool player_dead = player.takeDamage(enemy.atk);

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
            bool player_dead = player.takeDamage(enemy.atk);

            yield return new WaitForSeconds(2f);

            if(player_dead)
            {
                state = battle_state.lose;
                playerLose();
            }
            else
            {
               bool enemy_dead = enemy.takeDamage(player.atk);

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

 
}
