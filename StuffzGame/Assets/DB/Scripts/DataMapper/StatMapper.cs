using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

public class StatMapper : DataMapper
{
    protected override string FileName { get => "stats"; }
    protected override JObject JsonObject { get; }
    private List<JObject> StatList { get; }
    private readonly bool enableDebug = false;

    public StatMapper()
    {
        this.JsonObject = LoadJSON();
        this.StatList = (JsonObject[FileName] as JArray).Select(obj => obj as JObject).ToList();
    }

    public override T GetObjectById<T>(int id)
    {
        JObject stat = StatList.Where(it => (int)it["id"] == id).SingleOrDefault();
        if (stat != null)
        {
         
            return (T) Convert.ChangeType( new BasePokemonStat
            {
                Id = stat["id"].Value<int>(),
                IsBattleOnly = stat["is_battle_only"].Value<bool>(),
                DamageClass = ((string)stat["move_damage_class"] != null)? ParseDamageClass(stat["move_damage_class"].Value<string>()) : MoveDamageClass.NULL,
                Name = ((string)stat["name"] != null) ? ParseStatName(stat["name"].Value<string>()) : StatName.NULL,
            }, typeof(T));
        }
        if (enableDebug)
        {
            UnityEngine.Debug.LogWarning($"No stat found for stat ID: {id}");
        }
        return default;
    }

    private MoveDamageClass ParseDamageClass(string str)
    {
        string stringWithoutDashes = str.Replace('-', '_');
        if (Enum.TryParse<MoveDamageClass>(stringWithoutDashes, true, out MoveDamageClass result))
        {
            return result;
        }
        else
        {
            return MoveDamageClass.NULL;
        }
    }

    private StatName ParseStatName(string str)
    {
        string stringWithoutDashes = str.Replace('-', '_');
        if (Enum.TryParse<StatName>(stringWithoutDashes, true, out StatName result))
        {
            return result;
        }
        else
        {
            return StatName.NULL;
        }
    }
}