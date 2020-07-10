using UnityEngine;

public class PokemonSpawner : MonoBehaviour
{
    public GameObject wildPokemonPrefab;
    public GameObject playerGameObject;
    public GameObject encounterData;
    public float rate = 2f;
    public int maxMobs;
    private float nextSpawnTime = 0f;
    private int currentMobs;
    private PokemonFactory factory;

    // Start is called before the first frame update
    private void Start()
    {
        currentMobs = 0;
        factory = new PokemonFactory();
        Debug.Log("Generating trash value for Pokemon to preload JSON.");
        //Single pokemon creation to load the JSON and keep it cached.
        Pokemon trashValue = factory.CreatePokemon(1, 1);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        int randomPokemonID, randomLevel;
        float randX, randY;

        if (Time.time > nextSpawnTime && currentMobs < maxMobs)
        {
            randomPokemonID = Random.Range(1, 650);
            randomLevel = Random.Range(1, 101);
            nextSpawnTime = Time.time + rate;
            randX = Random.Range(-5.0f, 5.0f);
            randY = Random.Range(-5.0f, 5.0f);
            Vector2 spawnPoint = new Vector2(randX, randY);
            Pokemon wildPokemon = factory.CreatePokemon(randomPokemonID, randomLevel);

            GameObject spawnedWildPokemon = Instantiate(wildPokemonPrefab, spawnPoint, Quaternion.identity);
            spawnedWildPokemon.GetComponent<SpriteSwap>().Id = randomPokemonID;
            PokemonController pokemonController = spawnedWildPokemon.GetComponent<PokemonController>();
            pokemonController.InitWildPokemonData(playerGameObject, wildPokemon, encounterData);

            currentMobs++;
        }
    }
}