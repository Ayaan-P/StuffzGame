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

        
        for (int i = 0; i < size; i++)
        {
            int randLevel = rand.Next(15, 101);
            int randPokemonId = rand.Next(1, 807);
            Pokemon randPokemon = pokemonFactory.CreatePokemon(randPokemonId, randLevel);
            if (i % 25 == 0)
            {
                randPokemon.IsShiny = true;
            }

            PokemonStat hpStat = randPokemon.GetStat(StatName.HP);
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

        for (int j = 1; j <= size/4; j++)
        {
            int randNum = rand.Next(1, 400);
            var anotherRandItem = itemFactory.CreateItem(randNum);
            player.Inventory.Add(anotherRandItem);
        }
    }
}
