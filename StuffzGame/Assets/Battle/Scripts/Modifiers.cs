using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    

public class Modifiers 
{
    const double EFFECT = 1.0;
    const double SUPER = 2.0;
    const double NOTVERY = 0.5;
    const double NOEFFECT = 0;

    public double[,] type_table ={ {EFFECT, EFFECT, EFFECT, EFFECT, EFFECT, NOTVERY, EFFECT, NOEFFECT, NOTVERY, EFFECT, EFFECT, EFFECT, EFFECT, EFFECT, EFFECT, EFFECT, EFFECT, EFFECT},
                                  {SUPER, EFFECT, NOTVERY, NOTVERY, EFFECT, SUPER, NOTVERY, NOEFFECT, SUPER, EFFECT, EFFECT, EFFECT, EFFECT, NOTVERY, SUPER, EFFECT, SUPER, NOTVERY},
                                  {EFFECT, SUPER, EFFECT, EFFECT, EFFECT, NOTVERY, SUPER, EFFECT, NOTVERY, EFFECT, EFFECT, SUPER, NOTVERY, EFFECT, EFFECT, EFFECT, EFFECT, EFFECT},
                                  {EFFECT, EFFECT, EFFECT, NOTVERY, NOTVERY, NOTVERY, EFFECT, NOTVERY, NOEFFECT, EFFECT, EFFECT, SUPER, EFFECT, EFFECT, EFFECT, EFFECT, EFFECT, SUPER},
                                  {EFFECT, EFFECT, NOEFFECT, SUPER, EFFECT, SUPER, NOTVERY, EFFECT, SUPER, SUPER, EFFECT, NOTVERY, SUPER, EFFECT, EFFECT, EFFECT, EFFECT, EFFECT},
                                  {EFFECT, NOTVERY, SUPER, EFFECT, NOTVERY, EFFECT, SUPER, EFFECT, NOTVERY, SUPER, EFFECT, EFFECT, EFFECT, EFFECT, SUPER, EFFECT, EFFECT, EFFECT},
                                  {EFFECT, NOTVERY, NOTVERY, NOTVERY, EFFECT, EFFECT, EFFECT, NOTVERY, NOTVERY, NOTVERY, EFFECT, SUPER, EFFECT, SUPER, EFFECT, EFFECT , SUPER, NOTVERY},
                                  {NOEFFECT, EFFECT, EFFECT, EFFECT, EFFECT, EFFECT, EFFECT, SUPER, EFFECT, EFFECT, EFFECT, EFFECT, EFFECT, SUPER, EFFECT, EFFECT, NOTVERY, EFFECT},
                                  {EFFECT, EFFECT, EFFECT, EFFECT, EFFECT, SUPER, EFFECT, EFFECT, NOTVERY, NOTVERY, NOTVERY, EFFECT, NOTVERY, EFFECT, SUPER , EFFECT, EFFECT, SUPER},
                                  {EFFECT, EFFECT, EFFECT, EFFECT, EFFECT, NOTVERY, SUPER, EFFECT, SUPER, NOTVERY, NOTVERY, SUPER, EFFECT, EFFECT, SUPER, NOTVERY, EFFECT, EFFECT},
                                  {EFFECT, EFFECT, EFFECT, EFFECT, SUPER, SUPER, EFFECT, EFFECT, EFFECT, SUPER, NOTVERY, NOTVERY, EFFECT, EFFECT, EFFECT, NOTVERY, EFFECT, EFFECT},
                                  {EFFECT, EFFECT, NOTVERY, NOTVERY, SUPER, SUPER, NOTVERY, EFFECT, NOTVERY, NOTVERY, SUPER, NOTVERY, EFFECT, EFFECT, EFFECT, NOTVERY, EFFECT, EFFECT},
                                  {EFFECT, EFFECT, SUPER, EFFECT, NOEFFECT, EFFECT, EFFECT, EFFECT, EFFECT, EFFECT, SUPER, NOTVERY, NOTVERY, EFFECT, EFFECT, NOTVERY, EFFECT, EFFECT},
                                  {EFFECT, SUPER, EFFECT, SUPER, EFFECT, EFFECT, EFFECT, EFFECT, NOTVERY, EFFECT, EFFECT, EFFECT, EFFECT, NOTVERY, EFFECT, EFFECT, NOEFFECT, EFFECT},
                                  {EFFECT, EFFECT, SUPER, EFFECT, SUPER, EFFECT, EFFECT, EFFECT, NOTVERY, NOTVERY, NOTVERY, SUPER, EFFECT, EFFECT, NOTVERY, SUPER, EFFECT, EFFECT},
                                  {EFFECT, EFFECT, EFFECT, EFFECT, EFFECT, EFFECT, EFFECT, EFFECT, NOTVERY, EFFECT, EFFECT, EFFECT, EFFECT, EFFECT, EFFECT, SUPER, EFFECT, NOEFFECT},
                                  {EFFECT, NOTVERY, EFFECT, EFFECT, EFFECT, EFFECT, EFFECT, SUPER, EFFECT, EFFECT, EFFECT, EFFECT, EFFECT, SUPER, EFFECT, EFFECT, NOTVERY, NOTVERY},
                                  {EFFECT, SUPER, EFFECT, NOTVERY, EFFECT, EFFECT, EFFECT, EFFECT, NOTVERY, NOTVERY, EFFECT, EFFECT, EFFECT, EFFECT, EFFECT, SUPER, SUPER, EFFECT},
                                       };
  
    public double CalculateModifier(Pokemon attacking, Pokemon defending, PokemonMove move)
    {
        
        double STAB = 1;
        List<PokemonType> defending_type = defending.BasePokemon.Types;
        List<PokemonType> attacking_type = attacking.BasePokemon.Types;
        int move_type = (int) move.BaseMove.Type;
        foreach (PokemonType x in attacking_type)
        {
            if(move_type == (int) x)
            {
                Debug.Log("you get stab");
                STAB = 1.5;
                break;
            }    

        }
        double type_modifier=1;
        foreach (PokemonType x in defending_type)
        {
            type_modifier *= type_table[move_type-1, (int)(x)-1];
        }

        Debug.Log(type_modifier);
        double rand_modifier = Random.Range(0.85f, 1.0f); 

        double final_modifier = STAB* type_modifier*rand_modifier;
        Debug.Log(final_modifier+"="+STAB+"x"+type_modifier+"x"+rand_modifier);
        return final_modifier;
  }


}

