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
    private PokemonFactory factory;
    // Start is called before the first frame update
    void Start()
    {
        factory = new PokemonFactory();
        Debug.Log("Generating trash value for Pokemon to preload JSON.");
        //Single pokemon creation to load the JSON and keep it cached.
        Pokemon trashValue = factory.CreatePokemon(1,1);
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
           
            Pokemon pkmn ;
            // go.GetComponent<AIDestinationSetter>().target = player.transform;
            if (randpkmn == 0)
            {
                pkmn = factory.CreatePokemon(257,randlvl);
             
               // go.GetComponent<AIDestinationSetter>().target = player;
              
            }
            else
            {
                 pkmn = factory.CreatePokemon(373,randlvl);
              
            }
               GameObject go = Instantiate(WildPokemon, spawnpoint, Quaternion.identity);              
                go.GetComponent<PkmnController>().player = player;
                go.GetComponent<SpriteSwap>().pokemon_name = pkmn.BasePokemon.Name;
                go.GetComponent<PkmnController>().pokemon_name = pkmn.BasePokemon.Name;
                go.GetComponent<PkmnController>().wild_pokemon = pkmn;
                
                
            currmobs++;
        }

    }
}

