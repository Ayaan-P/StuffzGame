using System.Linq;
using UnityEngine;

public class PokemonSpawner : MonoBehaviour
{
    public GameObject WildPokemon;
    public GameObject player;
    public Sprite s;
    private float randX, randY;
    private int randpkmn;
    private Vector2 spawnpoint;
    public float rate = 2f;
    private float nextspawn = 0.0f;
    public int currmobs = 0;
    public int maxmobs;
    private PokemonFactory factory;

    // Start is called before the first frame update
    private void Start()
    {
        factory = new PokemonFactory();
        Debug.Log("Generating trash value for Pokemon to preload JSON.");
        //Single pokemon creation to load the JSON and keep it cached.
        Pokemon trashValue = factory.CreatePokemon(1, 1);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (Time.time > nextspawn && currmobs <= maxmobs)
        {
            randpkmn = Random.Range(1, 649);
            int randlvl = Random.Range(60, 70);
            nextspawn = Time.time + rate;
            randX = Random.Range(-5.0f, 5.0f);
            randY = Random.Range(-5.0f, 5.0f);
            spawnpoint = new Vector2(randX, randY);

      
            GameObject go = Instantiate(WildPokemon, spawnpoint, Quaternion.identity);

            Pokemon pkmn = factory.CreatePokemon(randpkmn, randlvl);
            go.GetComponent<SpriteSwap>().Id = randpkmn;

            go.GetComponent<PkmnController>().player = player;

            go.GetComponent<PkmnController>().pokemon_name = pkmn.BasePokemon.Name;
            go.GetComponent<PkmnController>().wild_pokemon = pkmn;

            currmobs++;
        }
    }
}