using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonSpawner : MonoBehaviour
{
    public GameObject WildPokemon;
    float randX,randY;
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
    void Update()
    {
        if(currmobs<=maxmobs)
        {

        
            if(Time.time>nextspawn)
            {
                nextspawn = Time.time + rate;
                randX = Random.Range(-5.0f, 5.0f);
                randY = Random.Range(-5.0f, 5.0f);
                spawnpoint = new Vector2(randX, randY);
                Instantiate(WildPokemon, spawnpoint, Quaternion.identity);
                currmobs++;
            }
        }
    }
}
