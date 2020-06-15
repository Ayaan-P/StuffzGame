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
            nextspawn = Time.time + rate;
            randX = Random.Range(-5.0f, 5.0f);
            randY = Random.Range(-5.0f, 5.0f);
            spawnpoint = new Vector2(randX, randY);
            GameObject go = Instantiate(WildPokemon, spawnpoint, Quaternion.identity);
            go.GetComponent<PkmnController>().player = player;
            // go.GetComponent<AIDestinationSetter>().target = player.transform;
            if (randpkmn == 0)
            {
                
                go.GetComponent<SpriteSwap>().pokemon_name = "Blaziken";
                go.GetComponent<PkmnController>().pokemon_name = "Blaziken";
               // go.GetComponent<AIDestinationSetter>().target = player;
              
            }
            else
            {
                go.GetComponent<SpriteSwap>().pokemon_name = "Salamence";
                go.GetComponent<PkmnController>().pokemon_name = "Salamence";
            }
            currmobs++;
        }

    }
}

