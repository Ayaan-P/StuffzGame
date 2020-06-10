using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class EvolutionMapper : DataMapper
{
    protected override string FileName { get => "evolutions"; }
    protected override JObject JsonObject { get; }
    private List<JObject> EvolutionList { get; }

    public EvolutionMapper()
    {
        this.JsonObject = LoadJSON();
        this.EvolutionList = (JsonObject[FileName] as JArray).Select(obj => obj as JObject).ToList();
    }

    public override object GetObjectById(int id)
    {
        throw new InvalidOperationException($"{nameof(EvolutionMapper)} does not support this method. Instead call {nameof(GetEvolutionFromSpeciesID)}");
    }

    public override object GetEvolutionFromSpeciesID(int speciesId, int evolutionChainId)
    {
        JObject evolution = EvolutionList.Where(it => (int)it["id"] == evolutionChainId).SingleOrDefault();
        if (evolution != null)
        {
            int babyTriggerItemId = JSONParsingUtil.GetIdFromJObject(evolution["baby_trigger_item"]);
            int evolutionId = evolution["id"].Value<int>();
            JObject chain = evolution["chain"].Value<JObject>();
            JObject obj = GetEvolutionHelper(chain, speciesId);

            if (obj != null)
            {
                return new PokemonEvolution
                {
                    BabyTriggerItemId = babyTriggerItemId,
                    Id = evolutionId,
                    IsBabyPokemon = obj["is_baby"].Value<bool>(),
                    PokemonSpeciesId = JSONParsingUtil.GetIdFromJObject(obj["species"]),
                    EvolutionDetails = GetEvolutionDetails(obj["evolution_details"].Value<JArray>())
                };
            }
        }
        UnityEngine.Debug.LogWarning($"No evolution found for species: {speciesId} and evolutionChain: {evolutionChainId}");
        return null;
    }

    private JObject GetEvolutionHelper(JObject chain, int speciesId)
    {
        int currentSpeciesId = JSONParsingUtil.GetIdFromJObject(chain["species"].Value<JObject>());
        if (currentSpeciesId != speciesId)
        {
            JArray evolvesTo = chain["evolves_to"].Value<JArray>();
            if (evolvesTo.Count != 0)
            {
                foreach (JObject evolution in evolvesTo)
                {
                    JObject returnedObject = GetEvolutionHelper(evolution, speciesId);
                    if (returnedObject != null)
                    {
                        return returnedObject;
                    }
                }
            }
            else
            {
                return null;
            }
        }
        else
        {
            return chain;
        }
        return null;
    }

    private List<EvolutionDetail> GetEvolutionDetails(JArray evolutionDetails)
    {
        List<EvolutionDetail> evolutionDetailList = new List<EvolutionDetail>();

        foreach (JObject detail in evolutionDetails)
        {
            evolutionDetailList.Add(
                new EvolutionDetail
                {
                    RequiredGender = (detail["gender"].Value<int?>() != null) ? (Gender)detail["gender"].Value<int>() : Gender.NULL,
                    RequiredHeldItemId = JSONParsingUtil.GetIdFromJObject(detail["held_item"]),
                    EvolutionItemId = JSONParsingUtil.GetIdFromJObject(detail["item"]),
                    KnownMoveId = JSONParsingUtil.GetIdFromJObject(detail["known_move"]),
                    KnownMoveType = (PokemonType)JSONParsingUtil.GetIdFromJObject(detail["known_move_type"]),
                    MinHappiness = detail["min_happiness"]?.Value<int?>(),
                    MinLevel = detail["min_level"]?.Value<int?>(),
                    NeedsOverworldRain = detail["needs_overworld_rain"].Value<bool>(),
                    PartySpeciesId = JSONParsingUtil.GetIdFromJObject(detail["party_species"]),
                    PartyType = (PokemonType)JSONParsingUtil.GetIdFromJObject(detail["party_type"]),
                    RelativePhysicalStats = (detail["relative_physical_stats"].Value<int?>()!=null)? (RelativePhysicalStatDifference) detail["relative_physical_stats"].Value<int>() : RelativePhysicalStatDifference.NULL,
                    RequiredTimeOfDay = (detail["time_of_day"].Value<string>().Length != 0)? ParseTimeOfDay(detail["time_of_day"].Value<string>()) : TimeOfDay.NULL,
                    TradeSpeciesId = JSONParsingUtil.GetIdFromJObject(detail["trade_species"]),
                    Trigger = (EvolutionTrigger)JSONParsingUtil.GetIdFromJObject(detail["trigger"])
                }
                ) ;
        }
        return evolutionDetailList;
    }


    private TimeOfDay ParseTimeOfDay(string str)
    {
        if (Enum.TryParse<TimeOfDay>(str,true, out TimeOfDay result))
        {
            return result;
        }
        else
        {
            return TimeOfDay.NULL;
        }
    }
}