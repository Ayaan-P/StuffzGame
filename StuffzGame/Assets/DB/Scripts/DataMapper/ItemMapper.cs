using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
public class ItemMapper : DataMapper
{
    protected override string FileName { get => "items"; }
    protected override JObject JsonObject { get; }
    private List<JObject> ItemList { get; }

    public ItemMapper()
    {
        this.JsonObject = LoadJSON();
        this.ItemList = (JsonObject[FileName] as JArray).Select(obj => obj as JObject).ToList();
    }

    public override object GetObjectById(int id)
    {
        JObject item = ItemList.Where(it => (int)it["id"] == id).SingleOrDefault();
        if (item != null)
        {
            return new Item
            {
                Attributes = (item["attributes"].Value<JArray>().Count!=0)? item["attributes"].Value<JArray>().Select(it => (ItemAttribute)JSONParsingUtil.GetIdFromJObject(it)).ToList() : new List<ItemAttribute>(),
                BabyTriggerForEvolutionId = JSONParsingUtil.GetIdFromJObject(item["baby_trigger_for"]),
                Category = (ItemCategory)JSONParsingUtil.GetIdFromJObject(item["category"]),
                Cost = item["cost"].Value<long>(),
                EffectEntries = JSONParsingUtil.GetEffectEntries(item["effect_entries"]),
                FlavorText = item["flavor_text"]?.Value<string>(),
                FlingEffect = (ItemFlingEffect)JSONParsingUtil.GetIdFromJObject(item["fling_effect"]),
                FlingPower = item["fling_power"]?.Value<int?>(),
                Id = item["id"].Value<int>(),
                Name = item["name"].Value<string>()
            };
        }

        UnityEngine.Debug.LogWarning($"No item found for item ID: {id}");
        return null;
    }

}