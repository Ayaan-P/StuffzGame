using System;
using System.Collections.Generic;
using System.Linq;

public class PokemonFactory
{
    private int SEED = 69;
    private readonly System.Random random;
    private readonly BasePokemonFactory basePokemonFactory;

    public PokemonFactory()
    {
        this.random = new System.Random(SEED);
        basePokemonFactory = new BasePokemonFactory();
    }

    public Pokemon CreatePokemon(int id, int level)
    {
        BasePokemon basePokemon = basePokemonFactory.CreateBasePokemon(id);
        if (basePokemon != null)
        {
            Pokemon pokemon = new Pokemon(random)
            {
                BasePokemon = basePokemon,
                CurrentLevel = level,
                Nature = ConstructPokemonNature(),
                Gender = ConstructPokemonGender(basePokemon),
                IsShiny = GetShinyDecision(),
                Nickname = null,
                LearnedMoves = GenerateMovesetForLevel(basePokemon, level),
                CurrentAbility = GenerateAbility(basePokemon),
                HeldItem = null,
                IsFainted = false
            };

            pokemon.CalculateStats();

            UnityEngine.Debug.Log($"Created pokemon with id: {id}. Pokemon : \n{pokemon}");
            return pokemon;
        }

        UnityEngine.Debug.LogError($"No pokemon found with id: {id}");
        return null;
    }

    public Pokemon CreateRandomPokemon(int level)
    {   /*TODO: Fix pokemon Id count via json*/
        /*DataMapper pokemonMapper = MapperFactory.GetInstance().GetMapper(MapperName.POKEMON_MAPPER);
        int maxId = pokemonMapper.GetJSONObjectCount();     <-- doesn't work because ID > 807 exist, but are numbered 10001 through 10157 */
        int MAX_ID = 807;
        int randomId = random.Next(1, MAX_ID + 1);
        UnityEngine.Debug.Log($"Creating random pokemon with id: {randomId}");
        return CreatePokemon(randomId, level);
    }

    private PokemonNature ConstructPokemonNature()
    {
        DataMapper natureMapper = MapperFactory.GetInstance().GetMapper(MapperName.NATURE_MAPPER);
        int numNatures = natureMapper.GetJSONObjectCount();
        int natureId = random.Next(1, numNatures);

        PokemonNatureTemplate natureTemplate = natureMapper.GetObjectById<PokemonNatureTemplate>(natureId);
        int increasedStatId = natureTemplate.IncreasedStatId;
        int decreasedStatId = natureTemplate.DecreasedStatId;

        DataMapper statMapper = MapperFactory.GetInstance().GetMapper(MapperName.STAT_MAPPER);

        BasePokemonStat increasedStat = statMapper.GetObjectById<BasePokemonStat>(increasedStatId);
        BasePokemonStat decreasedStat = statMapper.GetObjectById<BasePokemonStat>(decreasedStatId);

        return new PokemonNature
        {
            Id = natureTemplate.Id,
            Name = natureTemplate.Name,
            IncreasedStat = increasedStat,
            DecreasedStat = decreasedStat,
            LikedBerryFlavor = natureTemplate.LikedBerryFlavor,
            DislikedBerryFlavor = natureTemplate.DislikedBerryFlavor
        };
    }

    private Gender ConstructPokemonGender(BasePokemon pokemon)
    {
        int LOWER_LIMIT = 1;
        int UPPER_LIMIT = 252;
        int GENDER_RATE_MAX_VALUE = 8;
        float UPPER_LIMIT_FLOAT = UPPER_LIMIT;

        GenderRate genderRate = pokemon.Species.GenderRate;

        if (genderRate == GenderRate.MALE)
        {
            return Gender.MALE;
        }
        else if (genderRate == GenderRate.FEMALE)
        {
            return Gender.FEMALE;
        }
        else if (genderRate == GenderRate.GENDERLESS)
        {
            return Gender.GENDERLESS;
        }
        else
        {
            //Calculate gender if not a special case:
            int randomNumber = random.Next(LOWER_LIMIT, UPPER_LIMIT + 1);
            int scaledNumber = (int)(randomNumber / UPPER_LIMIT_FLOAT * GENDER_RATE_MAX_VALUE);

            if (scaledNumber >= (int)genderRate)
            {
                return Gender.MALE;
            }
            else
            {
                return Gender.FEMALE;
            }
        }
    }

    private bool GetShinyDecision()
    {
        int SHINY_THRESHOLD = 32;
        int LOWER_LIMIT = 0;
        int UPPER_LIMIT = 65535;
        int randomNumber = random.Next(LOWER_LIMIT, UPPER_LIMIT + 1);

        return randomNumber < SHINY_THRESHOLD;
    }

    private List<PokemonMove> GenerateMovesetForLevel(BasePokemon pokemon, int level)
    {
        int MAX_MOVE_SLOTS = 4;

        List<PokemonMove> allPossibleMoves = pokemon.PossibleMoveList;
        List<PokemonMove> movesPossibleAtCurrentLevel = new List<PokemonMove>();

        foreach (PokemonMove move in allPossibleMoves)
        {
            List<MoveLearnDetails> moveLearnDetailsList = move.MoveLearnDetails.Where(moveLearnDetail => moveLearnDetail.LevelLearnedAt <= level && moveLearnDetail.MoveLearnMethod == MoveLearnMethod.LEVEL_UP).ToList();
            if (moveLearnDetailsList.Count > 0)
            {
                movesPossibleAtCurrentLevel.Add(move);
            }
        }

        int numMovesPossibleAtCurrentLevel = movesPossibleAtCurrentLevel.Count;

        if (numMovesPossibleAtCurrentLevel == 0)
        {
            UnityEngine.Debug.LogError($"{pokemon.Name} with ID: {pokemon.Id} was unable to be constructed. No moves can be learned at level: {level}");
            throw new Exception($"{pokemon.Name} with ID: {pokemon.Id} was unable to be constructed. No moves can be learned at level: {level}");
        }
        else if (numMovesPossibleAtCurrentLevel <= MAX_MOVE_SLOTS)
        {
            // if less than or equal to maxMoveSlots, return all those moves as a moveset.
            return movesPossibleAtCurrentLevel;
        }
        else
        {
            // Fisher-Yates shuffle to get random, unique set of moves from possible moveset pool

            for (int i = numMovesPossibleAtCurrentLevel - 1; i >= numMovesPossibleAtCurrentLevel - MAX_MOVE_SLOTS; i--)
            {
                int randomIndex = random.Next(i + 1);
                PokemonMove temp = movesPossibleAtCurrentLevel[i];
                movesPossibleAtCurrentLevel[i] = movesPossibleAtCurrentLevel[randomIndex];
                movesPossibleAtCurrentLevel[randomIndex] = temp;
            }
            return movesPossibleAtCurrentLevel.Skip(numMovesPossibleAtCurrentLevel - MAX_MOVE_SLOTS).Take(MAX_MOVE_SLOTS).ToList();
        }
    }

    private PokemonAbility GenerateAbility(BasePokemon pokemon)
    {
        // approx. 30% of hidden ability:
        int HIDDEN_ABILITY_THRESHOLD = 21845;
        int LOWER_LIMIT = 0;
        int UPPER_LIMIT = 65535;

        int randomNumber = random.Next(LOWER_LIMIT, UPPER_LIMIT + 1);
        PokemonAbility hiddenAbility = pokemon.Abilities.Where(it => it.IsHidden == true).SingleOrDefault();

        int subtractHiddenAbility = 0;
        if (hiddenAbility != null)
        {
            if (randomNumber < HIDDEN_ABILITY_THRESHOLD)
            {
                return hiddenAbility;
            }
            else
            {
                subtractHiddenAbility--;
            }
        }

        int remainingAbilityCount = pokemon.Abilities.Count + subtractHiddenAbility;
        int randomIndex = random.Next(remainingAbilityCount);

        return pokemon.Abilities[randomIndex];
    }
}