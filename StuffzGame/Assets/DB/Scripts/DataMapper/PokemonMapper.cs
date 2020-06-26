using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

public class PokemonMapper : DataMapper
{
    protected override string FileName { get => "pokemon"; }
    protected override JObject JsonObject { get; }
    private List<JObject> PokemonList { get; }

    public PokemonMapper()
    {
        this.JsonObject = LoadJSON();
        this.PokemonList = (JsonObject[FileName] as JArray).Select(obj => obj as JObject).ToList();
    }

    public override T GetObjectById<T>(int id)
    {
        JObject pokemon = PokemonList.Where(it => (int)it["id"] == id).SingleOrDefault();
        if (pokemon != null)
        {
            List<AbilityWrapper> abilityWrapperList = GetAbilityInfo(pokemon["abilities"].Value<JArray>());
            List<int> abilityIdList = abilityWrapperList.Select(it => it.AbilityId).ToList();
            List<bool> abilityIsHiddenList = abilityWrapperList.Select(it => it.AbilityIsHidden).ToList();
            List<int> abilitySlotList = abilityWrapperList.Select(it => it.AbilitySlot).ToList();

            List<MoveWrapper> moveWrapperList = GetMoveInfo(pokemon["moves"].Value<JArray>());
            List<int> possibleMoveIdList = moveWrapperList.Select(it => it.PossibleMoveId).ToList();
            List<List<MoveLearnDetails>> moveLearnDetailsList = moveWrapperList.Select(it => it.MoveLearnDetails).ToList();

            List<StatWrapper> statWrapperList = GetStatInfo(pokemon["stats"].Value<JArray>());
            List<int> baseStatIdList = statWrapperList.Select(it => it.BaseStatId).ToList();
            List<int> EVGainedOnDefeatList = statWrapperList.Select(it => it.EVGainedOnDefeat).ToList();
            List<int> baseStatValueList = statWrapperList.Select(it => it.StatValue).ToList();


            return (T) Convert.ChangeType( new BasePokemonTemplate
            {
                AbilityIdList = abilityIdList,
                IsAbilityHiddenList = abilityIsHiddenList,
                AbilitySlotList = abilitySlotList,
                Height = pokemon["height"].Value<int>(),
                BaseExperienceOnDefeat = pokemon["base_experience"].Value<int>(),
                Id = pokemon["id"].Value<int>(),
                IsDefault = pokemon["is_default"].Value<bool>(),
                PossibleMoveIdList = possibleMoveIdList,
                MoveLearnDetailsList = moveLearnDetailsList,
                Name = pokemon["name"].Value<string>(),
                Order = pokemon["order"].Value<int>(),
                SpeciesId = JSONParsingUtil.GetIdFromJObject(pokemon["species"]),
                BaseStatIdList = baseStatIdList,
                EVsGainedOnDefeatList = EVGainedOnDefeatList,
                BaseStatValueList = baseStatValueList,
                Types = pokemon["types"].Value<JArray>().Select(it => (PokemonType)JSONParsingUtil.GetIdFromJObject(it)).ToList(),
                Weight = pokemon["weight"].Value<int>()

            }, typeof(T));
          
        }

        UnityEngine.Debug.LogWarning($"No pokemon found for pokemon ID: {id}");
        return default;
    }

   public int GetPokemonIdForSpeciesId(int id)
    {
        JObject pokemon = PokemonList.Where(it => JSONParsingUtil.GetIdFromJObject(it["id"]["species"]) == id).SingleOrDefault();
        if(pokemon!= null)
        {
            return pokemon["id"].Value<int>();
        }
        else
        {
            return -1;
        }

    }
    private List<AbilityWrapper> GetAbilityInfo(JArray abilities)
    {
        if (abilities == null || abilities.Count==0)
        {
            return new List<AbilityWrapper>();
        }
        else
        {
            List<AbilityWrapper> abilityWrapperList = new List<AbilityWrapper>();
            foreach (JObject ability in abilities)
            {
                AbilityWrapper wrapper = new AbilityWrapper 
                {
                    AbilityId = ability["id"].Value<int>(),
                    AbilityIsHidden = ability["is_hidden"].Value<bool>(),
                    AbilitySlot = ability["slot"].Value<int>()
                };
                abilityWrapperList.Add(wrapper);
            }
            return abilityWrapperList;
        }
    }

    private List<MoveWrapper> GetMoveInfo(JArray moves)
    {
        if (moves == null || moves.Count == 0)
        {
            return new List<MoveWrapper>();
        }
        else
        {
            List<MoveWrapper> moveWrapperList = new List<MoveWrapper>();
            foreach (JObject move in moves)
            {
                MoveWrapper wrapper = new MoveWrapper
                {
                    PossibleMoveId = move["id"].Value<int>(),
                    MoveLearnDetails = GetMoveLearnDetails(move["move_learn_details"].Value<JArray>())
                };
                moveWrapperList.Add(wrapper);
            }
            return moveWrapperList;
        }
    }

    private List<MoveLearnDetails> GetMoveLearnDetails(JArray moveDetails)
    {
        if (moveDetails.Count == 0)
        {
            return new List<MoveLearnDetails>();
        }
        else
        {
            List<MoveLearnDetails> moveLearnDetails = new List<MoveLearnDetails>();
            foreach(JObject detail in moveDetails)
            {
                MoveLearnDetails mld = new MoveLearnDetails
                {
                    LevelLearnedAt = detail["level_learned_at"].Value<int>(),
                    MoveLearnMethod = (MoveLearnMethod)detail["move_learn_method"].Value<int>()
                };
                moveLearnDetails.Add(mld);
            }
            return moveLearnDetails;
        }
    }

    private List<StatWrapper> GetStatInfo(JArray stats)
    {
        if (stats == null || stats.Count == 0)
        {
            return new List<StatWrapper>();
        }
        else
        {
            List<StatWrapper> statWrapperList = new List<StatWrapper>();
            foreach (JObject stat in stats)
            {
                StatWrapper wrapper = new StatWrapper
                {
                    BaseStatId = stat["id"].Value<int>(),
                    EVGainedOnDefeat = stat["effort"].Value<int>(),
                    StatValue = stat["base_stat"].Value<int>()
                };
                statWrapperList.Add(wrapper);
            }
            return statWrapperList;
        }
    }

    private class AbilityWrapper
    {
        public int AbilityId { get; set; }
        public bool AbilityIsHidden { get; set; }
        public int AbilitySlot { get; set; }

    }

    private class MoveWrapper
    {
        public int PossibleMoveId { get; set; }
        public List<MoveLearnDetails> MoveLearnDetails { get; set; }
    }

    private class StatWrapper
    {
        public int BaseStatId { get; set; }
        public int EVGainedOnDefeat { get; set; }
        public int StatValue { get; set; }
    }
}