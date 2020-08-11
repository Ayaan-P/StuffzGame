using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PokemonGrowthRate 
{

    public string Description { get; set; }
    public string Formula { get; set; }
    public int Id { get; set; }
    public Dictionary<int, long> LevelExperienceDict { get; set; }
    public string Name { get; set; }
    public long? CurrentExperience { get; set; }

    internal void SetEXP(int level)
    {
        if(level >= 1 && level <= Pokemon.MAX_LEVEL)
        {
            CurrentExperience = LevelExperienceDict[level];
        }
        else
        {
            Debug.LogError($"level {level} is invalid. Cannot set experience value for this level");
        }
    }

    internal long GetEXPToNextLevel(int currentLevel)
    {
        if(currentLevel == Pokemon.MAX_LEVEL)
        {
            return 0;
        }
        else if(CurrentExperience == null)
        {
            Debug.LogError($"Current experience is null. Cant get experience to next level");
            return long.MaxValue;
        }
        else
        {
            return LevelExperienceDict[currentLevel + 1] - (long)CurrentExperience;
        }
    }

    internal void GainExperience(long gainedExp)
    {
        CurrentExperience += gainedExp;
    }
}
