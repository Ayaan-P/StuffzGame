using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Modifiers
{
    private const double EFFECTIVE = 1.0;
    private const double SUPER = 2.0;
    private const double NOTVERY = 0.5;
    private const double NOEFFECT = 0;

    private readonly double[,] typeTable ={ 
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

    public double CalculateModifier(Pokemon attacking, Pokemon defending, PokemonMove move)
    {
        double STAB = 1;
        List<PokemonType> defending_type = defending.BasePokemon.Types;
        List<PokemonType> attacking_type = attacking.BasePokemon.Types;
        int move_type = (int)move.BaseMove.Type;
        foreach (PokemonType x in attacking_type)
        {
            if (move_type == (int)x)
            {
                Debug.Log("you get stab");
                STAB = 1.5;
                break;
            }
        }
        double type_modifier = 1;
        foreach (PokemonType x in defending_type)
        {
            type_modifier *= typeTable[move_type - 1, (int)(x) - 1];
        }

        Debug.Log(type_modifier);
        double rand_modifier = UnityEngine.Random.Range(0.85f, 1.0f);

        double final_modifier = STAB * type_modifier * rand_modifier;
        Debug.Log(final_modifier + "=" + STAB + "x" + type_modifier + "x" + rand_modifier);
        return final_modifier;
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
}