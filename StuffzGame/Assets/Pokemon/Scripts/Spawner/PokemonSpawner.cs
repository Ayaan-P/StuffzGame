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
            
            randpkmn = Random.Range(1, 649);
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
           
                pkmn = factory.CreatePokemon(randpkmn,randlvl);
                if(randpkmn<10)
                {
                    go.GetComponent<SpriteSwap>().id = "00"+randpkmn;
                }
                else if(randpkmn<100)
                {
                    go.GetComponent<SpriteSwap>().id = "0"+randpkmn;
                }
                else
                    go.GetComponent<SpriteSwap>().id = ""+randpkmn;
                
               // go.GetComponent<AIDestinationSetter>().target = player;
              
           
            
                           
                go.GetComponent<PkmnController>().player = player;
                
                go.GetComponent<PkmnController>().pokemon_name = pkmn.BasePokemon.Name;
                go.GetComponent<PkmnController>().wild_pokemon = pkmn;
                
                
            currmobs++;
        }

    }
}

