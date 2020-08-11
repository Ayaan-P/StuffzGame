using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
public class Pokemon
{
    public BasePokemon BasePokemon { get; set; }
    public int CurrentLevel { get; set; }
    public PokemonNature Nature { get; set; }
    public Gender Gender { get; set; }
    public bool IsShiny { get; set; }
    public string Nickname { get; set; }
    public List<PokemonMove> LearnedMoves { get; set; }
    public PokemonAbility CurrentAbility { get; set; }
    public Item HeldItem { get; set; }
    public bool IsFainted { get; set; }
    public MoveAilment Ailment { get; set; }
    private readonly System.Random random;

    public const int MAX_LEVEL = 100;
    public const int MAX_POKEMON_TYPES = 2;

    public delegate void UpdatePokemonUI(Pokemon pokemon);
    public delegate void LearnMove(Pokemon pokemon, PokemonMove move);
    public event UpdatePokemonUI OnLevelUp;
    public event UpdatePokemonUI OnEvolve;
    public event LearnMove OnNewMoveAvailable;
    public Pokemon(System.Random random)
    {
        this.random = random;
    }
    internal void CalculateStats()
    {
        int increasedStatId = Nature.IncreasedStat?.Id ?? -1;
        int decreasedStatId = Nature.DecreasedStat?.Id ?? -1;

        List<PokemonStat> persistentStats = BasePokemon.Stats.Where( it => it.BaseStat.IsBattleOnly == false).ToList();
        foreach (PokemonStat stat in persistentStats)
        {
            if (stat.IV == null)
            {
                //UnityEngine.Debug.LogWarning($"Pokemon {BasePokemon.Name} ({BasePokemon.Id}) has no {stat.BaseStat.Name} IV. Generating a new IV value!");
                stat.IV = GenerateIV();
            }

            stat.CalculateStat(increasedStatId, decreasedStatId, CurrentLevel);
        }

        // set experience for level
        this.BasePokemon.Species.GrowthRate.SetEXP(CurrentLevel);
    }

    private int GenerateIV()
    {
        const int minIV = 1;
        const int maxIV = 31;
        return random.Next(minIV, maxIV + 1);   // Random.Next(a,b) 'b' is exclusive
    }

    public bool GiveItem(Item item)
    {
        if (item.Attributes.Contains(ItemAttribute.HOLDABLE) ||
             item.Attributes.Contains(ItemAttribute.HOLDABLE_ACTIVE) ||
             item.Attributes.Contains(ItemAttribute.HOLDABLE_PASSIVE))
        {
            this.HeldItem = item;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void GainExperience(long gainedExp)
    {
        if (CurrentLevel == MAX_LEVEL){ return;}
        var growthRate = this.BasePokemon.Species.GrowthRate;
        long expToNextLevel = growthRate.GetEXPToNextLevel(CurrentLevel);

        if (gainedExp > expToNextLevel)
        {
            long difference = gainedExp - expToNextLevel;
            this.LevelUp();
            this.GainExperience(difference);
        }
        else
        {
            growthRate.GainExperience(gainedExp);
        }
    }

    private void LevelUp()
    {
        if (CurrentLevel == MAX_LEVEL){ return;}
        CurrentLevel++;
        CalculateStats();
        OnLevelUp(this);
        CheckForNewMovesAtLevel();
        if (ShouldEvolve())
        {
            OnEvolve(this);
            Evolve();
        }
    }

    private bool ShouldEvolve()
    {
        PokemonQuery query = new PokemonQuery();
        return query.CanPokemonEvolveAtLevel(CurrentLevel, this);
    }

    private void Evolve()
    {
    }

    private void CheckForNewMovesAtLevel()
    {
        foreach(PokemonMove move in this.BasePokemon.PossibleMoveList)
        {
            foreach(var moveDetail in move.MoveLearnDetails)
            {
                if(moveDetail.MoveLearnMethod == MoveLearnMethod.LEVEL_UP && moveDetail.LevelLearnedAt <= CurrentLevel)
                {
                    OnNewMoveAvailable(this, move);
                    Debug.Log($"{this.BasePokemon.Name} can learn a new move: {move.BaseMove.Name}");
                }
            }
        }
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine($"{BasePokemon.Name} ({Gender}) @ {HeldItem?.Name}");
        builder.AppendLine($"Lv. {CurrentLevel}");
        builder.AppendLine($"Ability: {CurrentAbility.BaseAbility.Name}");
        foreach (PokemonStat stat in BasePokemon.Stats)
        {
            if (stat.BaseStat.Name == StatName.HP)
            {
                builder.AppendLine($"{stat.BaseStat.Name}: {stat.CurrentValue}/{stat.CalculatedValue} ({stat.BaseValue})");
            }
            else
            {
                builder.Append($"{stat.BaseStat.Name}: {stat.CalculatedValue}({stat.BaseValue}) ");
            }
        }
        builder.Append("\nTypes: ");
        foreach (PokemonType type in BasePokemon.Types)
        {
            builder.Append($"{type}  ");
        }
        builder.AppendLine($"\nNature: {Nature.Name}");
        foreach (PokemonMove move in LearnedMoves)
        {
            builder.AppendLine($"   - {move.BaseMove.Name}  ({move.CurrentPP}/{move.BaseMove.PP})");
        }

        builder.AppendLine($"Meta: {{ \nNickname: {Nickname}");
        builder.AppendLine($"is Shiny?: {IsShiny}");
        builder.AppendLine($"Evolves from?: {BasePokemon?.Species?.EvolvesFrom?.PokemonSpeciesId.ToString() ?? "N/A"} }}");
        return builder.ToString();
    }

    public bool AttackedWith(PokemonMove move, Pokemon enemy)
    {
        float baseDamage = 0;
        return false;
       /* 
        MoveDamageClass damageClass = move.BaseMove.MoveDamageClass;
        if(move.BaseMove.Power!=null)
        {
           if(damageClass==1)
           {
               PokemonStat spatkStat = enemy.BasePokemon.Stats.Where( it=> it.BaseStat.Name == StatName.SPECIAL_ATTACK).SingleOrDefault();
               PokemonStat spdefStat = BasePokemon.Stats.Where( it=> it.BaseStat.Name == StatName.SPECIAL_DEFENSE).SingleOrDefault();
               baseDamage = (((2*CurrentLevel/5 +2)*(int)move.BaseMove.Power*spatkStat.CurrentValue/spdefStat.CurrentValue)/50 +2);
               Debug.Log("specialmove");
           }
           else if(damageClass==2)
           {
               PokemonStat atkStat = enemy.BasePokemon.Stats.Where( it=> it.BaseStat.Name == StatName.ATTACK).SingleOrDefault();
               PokemonStat defStat = BasePokemon.Stats.Where( it=> it.BaseStat.Name == StatName.DEFENSE).SingleOrDefault();
               baseDamage = (((2*CurrentLevel/5 +2)*(int)move.BaseMove.Power*atkStat.CurrentValue/defStat.CurrentValue)/50 +2);    
               Debug.Log("physicalmove");
           }

        }
        int currentHp = this.BasePokemon.Stats.Where( it=> it.BaseStat.Name == StatName.HP).SingleOrDefault().CurrentValue;
        Modifiers modifier = new Modifiers();
        Debug.Log(currentHp + ","+this.BasePokemon.Name);
        currentHp -= (int)( baseDamage*modifier.CalculateModifier(enemy, this, move ));
        Debug.Log(currentHp + ","+this.BasePokemon.Name);
        
        if(currentHp <= 0)
        {      
            this.BasePokemon.Stats.Where( it=> it.BaseStat.Name == StatName.HP).SingleOrDefault().CurrentValue = 0;
            return true; 
        }
        else
        {
            this.BasePokemon.Stats.Where( it=> it.BaseStat.Name == StatName.HP).SingleOrDefault().CurrentValue = currentHp; 
            return false;         
        }*/
    }

    internal bool? CanUseItem(Item item)
    {
        if (item.IsMachine)
        {
            Machine machine = item as Machine;
            List<int> possibleMoveIdList = this.BasePokemon.PossibleMoveList.Select(it => it.BaseMove.Id).ToList();
            return possibleMoveIdList.Contains(machine.MoveId);
        }
        else if (item.Category == ItemCategory.EVOLUTION)
        {
            PokemonQuery pokemonQuery = new PokemonQuery();
            List<BasePokemon> basePokemonList = pokemonQuery.GetEvolutionsFor(this);
            foreach(BasePokemon basePokemon in basePokemonList)
            {
                foreach( EvolutionDetail evolutionDetail in basePokemon.Species.EvolvesFrom.EvolutionDetails)
                {
                    if(evolutionDetail.EvolutionItemId == item.Id)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        else
        {
            return null;
        }
    }

    internal PokemonStat GetStat(StatName stat)
    {
        return this.BasePokemon.Stats.SingleOrDefault(it => it.BaseStat.Name == stat);
    }
}