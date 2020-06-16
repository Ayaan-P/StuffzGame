 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PokemonSpawner : MonoBehaviour
{
    public GameObject WildPokemon;
    public GameObject player;
    public Sprite s;
    float randX, randY;
    int randpkmn;
    Vector2 spawnpoint;
    public float rate = 2f;
    float nextspawn = 0.0f;
    public int currmobs = 0;
    public int maxmobs;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time > nextspawn && currmobs <= maxmobs)
        {
            
            randpkmn = Random.Range(0, 2);
            int randlvl = Random.Range(60, 70);
            nextspawn = Time.time + rate;
            randX = Random.Range(-5.0f, 5.0f);
            randY = Random.Range(-5.0f, 5.0f);
            spawnpoint = new Vector2(randX, randY);
            

            PokemonFactory factory = new PokemonFactory();
            Pokemon trash = factory.CreatePokemon(257,randlvl);
            Pokemon pkmn ;
            // go.GetComponent<AIDestinationSetter>().target = player.transform;
            GameObject go = Instantiate(WildPokemon, spawnpoint, Quaternion.identity);  
            if (randpkmn == 0)
            {
                pkmn = factory.CreatePokemon(257,randlvl);
                go.GetComponent<SpriteSwap>().id = "445s";
               // go.GetComponent<AIDestinationSetter>().target = player;
              
            }
            else
            {
                 pkmn = factory.CreatePokemon(373,randlvl);
                 go.GetComponent<SpriteSwap>().id = "715";
              
            }
                           
                go.GetComponent<PkmnController>().player = player;
                
                go.GetComponent<PkmnController>().pokemon_name = pkmn.BasePokemon.Name;
                go.GetComponent<PkmnController>().wild_pokemon = pkmn;
                
                
            currmobs++;
        }

    }
}

