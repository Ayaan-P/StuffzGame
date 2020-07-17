using System;
using System.Linq;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private GameManager()
    {
    }


    private void Start()
    {
        Player player = Player.Instance;
        PokemonFactory pokemonFactory = new PokemonFactory();
        ItemFactory itemFactory = new ItemFactory();
        System.Random rand = new System.Random();
        int size = rand.Next(30, 200);
   
        Pokemon randPokemon = pokemonFactory.CreatePokemon(rand.Next(1, 151), rand.Next(15, 101));
        Pokemon randPokemon2 = pokemonFactory.CreatePokemon(rand.Next(1, 151), rand.Next(15, 101));
      
        player.Party.Add(randPokemon);
        player.Party.Add(randPokemon2);

        /*
        for (int i = 0; i < size; i++)
        {
            int randLevel = rand.Next(15, 101);
            int randPokemonId = rand.Next(1, 151);
            Pokemon randPokemon = pokemonFactory.CreatePokemon(randPokemonId, randLevel);
            if (i == 25)
            {
                randPokemon.IsShiny = true;
            }
            PokemonStat hpStat = randPokemon.BasePokemon.Stats.Where(it => it.BaseStat.Name == StatName.HP).SingleOrDefault();
            float hpPercent = (float)rand.NextDouble();

            hpStat.CurrentValue = Convert.ToInt32(hpPercent * hpStat.CalculatedValue);
            if (hpStat.CurrentValue <= float.Epsilon)
            {
                randPokemon.IsFainted = true;
            }
            int randItemId = rand.Next(1, 400);
            Item randItem = itemFactory.CreateItem(randItemId);
            bool wasItemGiven = randPokemon.GiveItem(randItem);
            player.Party.Add(randPokemon);
        }
        Item anotherRandItem;
        for (int j = 1; j <= 50; j++)
        {
            int randNum = rand.Next(1, 400);
            anotherRandItem = itemFactory.CreateItem(randNum);
            player.Inventory.Add(anotherRandItem);
        }*/
    }
}
