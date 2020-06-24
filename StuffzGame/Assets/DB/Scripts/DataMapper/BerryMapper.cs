using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

public class BerryMapper : DataMapper
{
    protected override string FileName { get => "berries"; }
    protected override JObject JsonObject { get; }
    private List<JObject> BerryList { get; }

    public BerryMapper()
    {
        this.JsonObject = LoadJSON();
        this.BerryList = (JsonObject[FileName] as JArray).Select(obj => obj as JObject).ToList();
    }

    public override T GetObjectById<T>(int id)
    {
        JObject berry = BerryList.Where(it => (int)it["id"] == id).SingleOrDefault();
        if (berry != null)
        {
            return (T) Convert.ChangeType(new Berry
            {
                Firmness = ParseBerryFirmness(berry["firmness"].Value<string>()),
                Flavors = berry["flavors"].Select(flavor => ParseBerryFlavor(flavor.Value<string>())).ToList(),
                GrowthTime = berry["growth_time"].Value<int>(),
                ItemId = berry["id"].Value<int>(),
                MaxHarvest = berry["max_harvest"].Value<int>(),
                BerryName = berry["name"].Value<string>(),
                NaturalGiftPower = berry["natural_gift_power"].Value<int>(),
                NaturalGiftTypeId = berry["natural_gift_type_id"].Value<int>(),
                Size = berry["size"].Value<int>(),
                Smoothness = berry["smoothness"].Value<int>(),
                Soil_dryness = berry["soil_dryness"].Value<int>()
            },typeof(T));
        }

        UnityEngine.Debug.LogWarning($"No berry found for berry ID: {id}");
        return default;
    }

    private BerryFlavor ParseBerryFlavor(string str)
    {
        if (Enum.TryParse<BerryFlavor>(str, true, out BerryFlavor result))
        {
            return result;
        }
        else
        {
            return BerryFlavor.NULL;
        }
    }

    private BerryFirmness ParseBerryFirmness(string str)
    {
        if (Enum.TryParse<BerryFirmness>(str, true, out BerryFirmness result))
        {
            return result;
        }
        else
        {
            return BerryFirmness.NULL;
        }
    }
}