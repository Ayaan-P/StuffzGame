using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PokemonQuery
{
    private MapperFactory mapperFactory;

    public PokemonQuery()
    {
        this.mapperFactory = MapperFactory.GetInstance();
    }

    public List<BasePokemon> GetEvolutionsFor(Pokemon pokemon)
    {
        int speciesId = pokemon.BasePokemon.Species.Id;
        SpeciesMapper speciesMapper = mapperFactory.GetMapper(MapperName.SPECIES_MAPPER) as SpeciesMapper;
        PokemonMapper pokemonMapper = mapperFactory.GetMapper(MapperName.POKEMON_MAPPER) as PokemonMapper;

        List<int> evolutionSpeciesIdList = speciesMapper.GetSpeciesIdForSpeciesThatEvolveFrom(speciesId);
        List<int> pokemonIdList = new List<int>();
        foreach (int evolutionSpeciesId in evolutionSpeciesIdList)
        {
            if(evolutionSpeciesId != -1)
            {
                int pokemonId = pokemonMapper.GetPokemonIdForSpeciesId(evolutionSpeciesId);
                pokemonIdList.Add(pokemonId);
            }
        }

        if(pokemonIdList.Count!= 0)
        {
            List<BasePokemon> toReturn = new List<BasePokemon>();
            BasePokemonFactory basePokemonFactory = new BasePokemonFactory();
            foreach(int pokemonId in pokemonIdList)
            {
                BasePokemon basePokemon = basePokemonFactory.CreateBasePokemon(pokemonId);
                toReturn.Add(basePokemon);
            }

            return toReturn;
        }
        else
        {
            return new List<BasePokemon>();
        }
    }
}
