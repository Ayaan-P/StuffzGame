using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;


public class 
    
    MoveMapper : DataMapper
{
    protected override string FileName { get => "moves"; }
    protected override JObject JsonObject { get; }
    private List<JObject> MoveList { get; }

    public MoveMapper()
    {
        this.JsonObject = LoadJSON();
        this.MoveList = (JsonObject[FileName] as JArray).Select(obj => obj as JObject).ToList();
    }

    public override T GetObjectById<T>(int id)
    {
        JObject move = MoveList.Where(it => (int)it["id"] == id).SingleOrDefault();
        
        if (move != null)
        {
            JObject meta = move["meta"].Value<JObject>();
            return (T) Convert.ChangeType( new BasePokemonMoveTemplate
            {
                Accuracy = move["accuracy"]?.Value<int?>(),
                MoveDamageClass = ParseDamageClass(move["damage_class"].Value<string>()),
                EffectChance = JSONParsingUtil.ConvertPercentageIntToFloat(move["effect_chance"]?.Value<int?>()),
                EffectEntries = JSONParsingUtil.GetEffectEntries(move["effect_entries"]),
                FlavorText = move["flavor_text"].Value<string>(),
                Id = move["id"].Value<int>(),
                Ailment = (MoveAilment)JSONParsingUtil.GetIdFromJObject(meta["ailment"]),
                AilmentChance = JSONParsingUtil.ConvertPercentageIntToFloat(meta["ailment_chance"]?.Value<int?>()) ?? -1,
                Category = (MoveCategory)JSONParsingUtil.GetIdFromJObject(meta["category"]),
                CritRate = meta["crit_rate"].Value<int>(),
                Drain = meta["drain"].Value<int>(),
                FlinchChance = JSONParsingUtil.ConvertPercentageIntToFloat(meta["flinch_chance"]?.Value<int?>()) ?? -1,
                Healing = meta["healing"].Value<int>(),
                MaxHits = meta["max_hits"]?.Value<int?>(),
                MinHits = meta["min_hits"]?.Value<int?>(),
                MaxTurns = meta["max_turns"]?.Value<int?>(),
                MinTurns = meta["min_turns"]?.Value<int?>(),
                StatChance = JSONParsingUtil.ConvertPercentageIntToFloat(meta["stat_chance"]?.Value<int?>()) ?? -1,
                Name = move["name"].Value<string>(),
                Power = move["power"]?.Value<int?>(),
                PP = move["pp"].Value<int>(),
                Priority = (MovePriority)move["priority"].Value<int>(),
                StatChangesIdDict = GetStatChangesIdDict(move["stat_changes"].Value<JArray>()),
                Target = ParseTarget(move["target"].Value<string>()),
                Type = (PokemonType)JSONParsingUtil.GetIdFromJObject(move["type"])
            }, typeof(T));
        }

        UnityEngine.Debug.LogWarning($"No move found for move ID: {id}");
        return default;

    }

    private Dictionary<int, int> GetStatChangesIdDict(JArray statArray)
    {
        Dictionary<int, int> statChangesIdDict = new Dictionary<int, int>();
        if(statArray.Count == 0)
        {
            return new Dictionary<int, int>();
        }
        else
        {
            foreach (JObject stat in statArray)
            {
                int statId = JSONParsingUtil.GetIdFromJObject(stat["stat"]);
                int change = stat["change"].Value<int>();
                statChangesIdDict.Add(statId, change);
            }
            return statChangesIdDict;
        }
    }

    private MoveDamageClass ParseDamageClass(string str)
    {
        string stringWithoutDashes = str.Replace('-','_');
        if (Enum.TryParse<MoveDamageClass>(stringWithoutDashes, true, out MoveDamageClass result))
        {
            return result;
        }
        else
        {
            return MoveDamageClass.NULL;
        }
    }

    private PokemonTarget ParseTarget(string str)
    {
        string stringWithoutDashes = str.Replace('-', '_');
        if (Enum.TryParse<PokemonTarget>(stringWithoutDashes, true, out PokemonTarget result))
        {
            return result;
        }
        else
        {
            return PokemonTarget.NULL;
        }
    } 
}