
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

public class FormMapper : DataMapper
{
    protected override string FileName { get => "forms"; }
    protected override JObject JsonObject { get; }
    private List<JObject> FormList { get; }

    public FormMapper()
    {
        this.JsonObject = LoadJSON();
        this.FormList = (JsonObject[FileName] as JArray).Select(obj => obj as JObject).ToList();
    }

    public override T GetObjectById<T>(int id)
    {
        JObject form = FormList.SingleOrDefault(it => (int) it["id"] == id);
        if (form != null)
        {
            return (T) Convert.ChangeType(new PokemonForm
            {
                Description = form["form_description"]?.Value<string>(),
                FormName =  form["form_name"]?.Value<string>(),
                FormOrder = form["form_order"].Value<int>(),
                FormFullName = form["full_name"]?.Value<string>(),
                Id = form["id"].Value<int>(),
                IsBattleOnly = form["is_battle_only"].Value<bool>(),
                IsDefault = form["is_default"].Value<bool>(),
                IsMega = form["is_mega"].Value<bool>(),
                Name = form["name"].Value<string>(),
                Order = form["order"].Value<int>(),
                PokemonId = JSONParsingUtil.GetIdFromJObject(form["pokemon"])
            }, typeof(T));
        }

        UnityEngine.Debug.LogWarning($"No form found for form ID: {id}");
        return default;
    }
}
