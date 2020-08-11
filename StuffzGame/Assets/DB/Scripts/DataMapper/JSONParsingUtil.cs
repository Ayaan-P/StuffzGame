using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

public static class JSONParsingUtil
{

    public static List<string> GetEffectEntries(object obj)
    {
        List<string> effectEntries = new List<string>();
        if (obj is JArray array)
        {
            foreach (JObject entry in array)
            {
                effectEntries.Add(entry["effect"].Value<string>());
                effectEntries.Add(entry["short_effect"].Value<string>());

            }
            return effectEntries;
        }
        else
        {
            return new List<string>();
        }
    }

    public static int GetIdFromJObject(object obj)
    {
        return (obj is JObject jObject) ? jObject["id"].Value<int>() : -1;
    }

    public static float? ConvertPercentageIntToFloat(int? num)
    {
        if(num == null)
        {
            return null;
        }
        const float HUNDRED = 100f;
        float? result = num / HUNDRED;
        return result;
    }
}
