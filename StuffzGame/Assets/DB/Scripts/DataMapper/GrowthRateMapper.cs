using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

public class GrowthRateMapper : DataMapper
{
    protected override string FileName { get => "exp_rates"; }
    protected override JObject JsonObject { get; }
    private List<JObject> ExpGrowthRateList { get; }

    public GrowthRateMapper()
    {
        this.JsonObject = LoadJSON();
        this.ExpGrowthRateList = (JsonObject[FileName] as JArray).Select(obj => obj as JObject).ToList();
    }

    public override T GetObjectById<T>(int id)
    {

        JObject expGrowthRate = ExpGrowthRateList.Where(it => (int)it["id"] == id).SingleOrDefault();
        if (expGrowthRate != null)
        {
            return (T) Convert.ChangeType(new PokemonGrowthRate
            {
                Description = expGrowthRate["description"].Value<string>(),
                Formula = expGrowthRate["formula"].Value<string>(),
                Id = expGrowthRate["id"].Value<int>(),
                LevelExperienceDict = GetLevelExperienceDict(expGrowthRate["levels"].Value<JArray>()),
                Name = expGrowthRate["name"].Value<string>(),
                CurrentExperience = null
            },typeof(T));;
        }
        UnityEngine.Debug.LogWarning($"No EXP Growth Rate found for ID: {id}");
        return default;
    }


    private Dictionary<int, long> GetLevelExperienceDict(JArray levels)
    {
        Dictionary<int, long> levelExpDict = new Dictionary<int, long>();

        foreach (JObject level in levels)
        {
            levelExpDict.Add(
                level["level"].Value<int>(),
                level["experience"].Value<long>());
        }

        return levelExpDict;
    }
}