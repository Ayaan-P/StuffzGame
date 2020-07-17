using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Rendering;

public class Modifiers
{
    private const float EFFECTIVE = 1;
    private const float SUPER = 2;
    private const float NOTVERY = 0.5f;
    private const float NOEFFECT = 0;

    private readonly float[,] typeTable ={ 
                            /*NORMAL     FIGHTING   FLYING     POISON     GROUND     ROCK       BUG        GHOST      STEEL      FIRE       WATER      GRASS      ELECTRIC   PSYCHIC    ICE        DRAGON     DARK       FAIRY
        /*NORMAL*/           {EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, NOTVERY,   EFFECTIVE, NOEFFECT,  NOTVERY,   EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE},
        /*FIGHTING*/         {SUPER,     EFFECTIVE, NOTVERY,   NOTVERY,   EFFECTIVE, SUPER,     NOTVERY,   NOEFFECT,  SUPER,     EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, NOTVERY,   SUPER,     EFFECTIVE, SUPER,     NOTVERY},
        /*FLYING*/           {EFFECTIVE, SUPER,     EFFECTIVE, EFFECTIVE, EFFECTIVE, NOTVERY,   SUPER,     EFFECTIVE, NOTVERY,   EFFECTIVE, EFFECTIVE, SUPER,     NOTVERY,   EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE},
        /*POISON*/           {EFFECTIVE, EFFECTIVE, EFFECTIVE, NOTVERY,   NOTVERY,   NOTVERY,   EFFECTIVE, NOTVERY,   NOEFFECT,  EFFECTIVE, EFFECTIVE, SUPER,     EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, SUPER},
        /*GROUND*/           {EFFECTIVE, EFFECTIVE, NOEFFECT,  SUPER,     EFFECTIVE, SUPER,     NOTVERY,   EFFECTIVE, SUPER,     SUPER,     EFFECTIVE, NOTVERY,   SUPER,     EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE},
        /*ROCK*/             {EFFECTIVE, NOTVERY,   SUPER,     EFFECTIVE, NOTVERY,   EFFECTIVE, SUPER,     EFFECTIVE, NOTVERY,   SUPER,     EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, SUPER,     EFFECTIVE, EFFECTIVE, EFFECTIVE},
        /*BUG*/              {EFFECTIVE, NOTVERY,   NOTVERY,   NOTVERY,   EFFECTIVE, EFFECTIVE, EFFECTIVE, NOTVERY,   NOTVERY,   NOTVERY,   EFFECTIVE, SUPER,     EFFECTIVE, SUPER,     EFFECTIVE, EFFECTIVE, SUPER,     NOTVERY},
        /*GHOST*/            {NOEFFECT,  EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, SUPER,     EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, SUPER,     EFFECTIVE, EFFECTIVE, NOTVERY,   EFFECTIVE},
        /*STEEL*/            {EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, SUPER,     EFFECTIVE, EFFECTIVE, NOTVERY,   NOTVERY,   NOTVERY,   EFFECTIVE, NOTVERY,   EFFECTIVE, SUPER ,    EFFECTIVE, EFFECTIVE, SUPER},
        /*FIRE*/             {EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, NOTVERY,   SUPER,     EFFECTIVE, SUPER,     NOTVERY,   NOTVERY,   SUPER,     EFFECTIVE, EFFECTIVE, SUPER,     NOTVERY,   EFFECTIVE, EFFECTIVE},
        /*WATER*/            {EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, SUPER,     SUPER,     EFFECTIVE, EFFECTIVE, EFFECTIVE, SUPER,     NOTVERY,   NOTVERY,   EFFECTIVE, EFFECTIVE, EFFECTIVE, NOTVERY,   EFFECTIVE, EFFECTIVE},
        /*GRASS*/            {EFFECTIVE, EFFECTIVE, NOTVERY,   NOTVERY,   SUPER,     SUPER,     NOTVERY,   EFFECTIVE, NOTVERY,   NOTVERY,   SUPER,     NOTVERY,   EFFECTIVE, EFFECTIVE, EFFECTIVE, NOTVERY,   EFFECTIVE, EFFECTIVE},
        /*ELECTRIC*/         {EFFECTIVE, EFFECTIVE, SUPER,     EFFECTIVE, NOEFFECT,  EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, SUPER,     NOTVERY,   NOTVERY,   EFFECTIVE, EFFECTIVE, NOTVERY,   EFFECTIVE, EFFECTIVE},
        /*PSYCHIC*/          {EFFECTIVE, SUPER,     EFFECTIVE, SUPER,     EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, NOTVERY,   EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, NOTVERY,   EFFECTIVE, EFFECTIVE, NOEFFECT,  EFFECTIVE},
        /*ICE*/              {EFFECTIVE, EFFECTIVE, SUPER,     EFFECTIVE, SUPER,     EFFECTIVE, EFFECTIVE, EFFECTIVE, NOTVERY,   NOTVERY,   NOTVERY,   SUPER,     EFFECTIVE, EFFECTIVE, NOTVERY,   SUPER,     EFFECTIVE, EFFECTIVE},
        /*DRAGON*/           {EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, NOTVERY,   EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, SUPER,     EFFECTIVE, NOEFFECT},
        /*DARK*/             {EFFECTIVE, NOTVERY,   EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, SUPER,     EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, SUPER,     EFFECTIVE, EFFECTIVE, NOTVERY,   NOTVERY},
        /*FAIRY*/            {EFFECTIVE, SUPER,     EFFECTIVE, NOTVERY,   EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, NOTVERY,   NOTVERY,   EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, EFFECTIVE, SUPER,     SUPER,     EFFECTIVE},
                                       };

    /*
     * Formula for damage calculation taken from Bulbapedia:
     * https://bulbapedia.bulbagarden.net/wiki/Damage#Damage_calculation
     * 
     */
    public double CalculateModifier(Pokemon attackingPokemon, Pokemon defendingPokemon, PokemonMove move)
    {
        StringBuilder stringBuilder = new StringBuilder();
        List<PokemonType> defendingTypes = defendingPokemon.BasePokemon.Types;
        List<PokemonType> attackingTypes = attackingPokemon.BasePokemon.Types;
        PokemonType moveType = move.BaseMove.Type;

        float targetModifier = GetTargetModifier(move.BaseMove.Target);

        float criticalModifier = GetCriticalHitModifier(attackingPokemon, move);

        float randomModifier = UnityEngine.Random.Range(0.85f, 1.0f);

        float STAB = 1;
        foreach (PokemonType attackingType in attackingTypes)
        {
            if (moveType == attackingType)
            {
                STAB = 1.5f;
                stringBuilder.Append($"{attackingPokemon.BasePokemon.Name} attacked {defendingPokemon.BasePokemon.Name} with {move.BaseMove.Name} (STAB: {STAB})\n");
                break;
            }
        }

        float typeModifier = 1;
        foreach (PokemonType defendingType in defendingTypes)
        {

            typeModifier *= GetTypeEffectiveness(moveType, defendingType);
        }


        stringBuilder.Append($"{move.BaseMove.Name} is {typeModifier} x effective against {defendingPokemon.BasePokemon.Name}\n");

        float burnModifier = (attackingPokemon.Ailment == MoveAilment.BURN && move.BaseMove.MoveDamageClass == MoveDamageClass.PHYSICAL) ? 0.5f : 1;

        float finalModifier = targetModifier * criticalModifier * randomModifier * STAB * typeModifier * burnModifier;
        stringBuilder.Append($"Final modifier = {targetModifier} x {criticalModifier} x {randomModifier} x {STAB} x {typeModifier} x {burnModifier}");
        Debug.Log(stringBuilder.ToString());
        return finalModifier;
    }

    private float GetTargetModifier(PokemonTarget target)
    {
        switch (target)
        {
            case PokemonTarget.ALL_OPPONENTS:
            case PokemonTarget.ALL_OTHER_POKEMON:
            case PokemonTarget.ALL_POKEMON:
            case PokemonTarget.ENTIRE_FIELD:
            case PokemonTarget.OPPONENTS_FIELD:
            case PokemonTarget.USERS_FIELD:
            case PokemonTarget.USER_AND_ALLIES:
                return 0.75f;
            case PokemonTarget.SPECIFIC_MOVE:
            case PokemonTarget.SELECTED_POKEMON_ME_FIRST:
            case PokemonTarget.ALLY:
            case PokemonTarget.USER_OR_ALLY:
            case PokemonTarget.USER:
            case PokemonTarget.RANDOM_OPPONENT:
            case PokemonTarget.SELECTED_POKEMON:
            case PokemonTarget.NULL:
                return 1;
            default:
                Debug.LogError($"{target} modifier not available, setting to 1");
                return 1;
        }
    }
    private float GetCriticalHitModifier(Pokemon attackingPokemon, PokemonMove move)
    {

        int critRateBonus = move.BaseMove.CritRate;
        return 0;
    }

    public List<PokemonType> GetTypesStrongAgainst(PokemonType type)
    {
        List<PokemonType> strongAgainstTypes = new List<PokemonType>();
        int typeIndex = (int)type - 1;
        for(int i=0;i< typeTable.GetLength(0); i++)
        {
            if(typeTable[i,typeIndex] == SUPER)
            {
                strongAgainstTypes.Add((PokemonType)(i+1)); // i+1 because the enum is 1 indexed
            }
        }
        return strongAgainstTypes;
    }

    public List<PokemonType> GetTypesWeakAgainst(PokemonType type)
    {
        List<PokemonType> weakAgainstTypes = new List<PokemonType>();
        int typeIndex = (int)type - 1;
        for (int i = 0; i < typeTable.GetLength(0); i++)
        {
            if (typeTable[i,typeIndex] == NOTVERY)
            {
                weakAgainstTypes.Add((PokemonType)(i+1));   // i+1 because the enum is 1 indexed
            }
        }
        return weakAgainstTypes;
    }

  public List<PokemonType> GetTypesWeakTo(PokemonType type)
    {
        List<PokemonType> weakToTypes = new List<PokemonType>();
        int typeIndex = (int)type - 1;
        for (int i = 0; i < typeTable.GetLength(0); i++)
        {
            if (typeTable[typeIndex,i] == SUPER)
            {
                weakToTypes.Add((PokemonType)(i + 1)); // i+1 because the enum is 1 indexed
            }
        }
        return weakToTypes;
    }

    public List<PokemonType> GetTypesResistantTo(PokemonType type)
    {
        List<PokemonType> resistantToTypes = new List<PokemonType>();
        int typeIndex = (int)type - 1;
        for (int i = 0; i < typeTable.GetLength(0); i++)
        {
            if (typeTable[typeIndex, i] == NOTVERY)
            {
                resistantToTypes.Add((PokemonType)(i + 1)); // i+1 because the enum is 1 indexed
            }
        }
        return resistantToTypes;
    }

    public float GetTypeEffectiveness(PokemonType attackingType, PokemonType defendingType)
    {
        int attackingTypeIndex = (int)attackingType - 1;
        int defendingTypeIndex = (int)defendingType - 1;
        return typeTable[attackingTypeIndex, defendingTypeIndex];
    }
}