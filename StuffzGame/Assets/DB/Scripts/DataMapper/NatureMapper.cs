using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

public class NatureMapper : DataMapper
{
    protected override string FileName { get => "natures"; }
    protected override JObject JsonObject { get; }
    private List<JObject> NatureList { get; }

    public NatureMapper()
    {
        this.JsonObject = LoadJSON();
        this.NatureList = (JsonObject[FileName] as JArray).Select(obj => obj as JObject).ToList();
    }

    public override object GetObjectById(int id)
    {
        JObject nature = NatureList.Where(it => (int)it["id"] == id).SingleOrDefault();
        if (nature != null)
        {
            return new PokemonNatureTemplate
            {
               Id = nature["id"].Value<int>(),
               Name= ParseNature(nature["name"].Value<string>()),
               IncreasedStatId = JSONParsingUtil.GetIdFromJObject(nature["increased_stat"]),
               DecreasedStatId = JSONParsingUtil.GetIdFromJObject(nature["decreased_stat"]),
               LikedBerryFlavor = (BerryFlavor)JSONParsingUtil.GetIdFromJObject(nature["likes_flavor"]),
               DislikedBerryFlavor = (BerryFlavor)JSONParsingUtil.GetIdFromJObject(nature["hates_flavor"])
            };
        }

        UnityEngine.Debug.LogWarning($"No nature found for nature ID: {id}");
        return null;

    }

    private Nature ParseNature(string str)
    {
        string stringWithoutDashes = str.Replace('-', '_');
        if (Enum.TryParse<Nature>(stringWithoutDashes, true, out Nature result))
        {
            return result;
        }
        else
        {
            return Nature.NULL;
        }
    }
}