using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
public class MachineMapper : DataMapper
{
    protected override string FileName { get => "machines"; }
    protected override JObject JsonObject { get; }
    private List<JObject> MachineList { get; }

    public MachineMapper()
    {
        this.JsonObject = LoadJSON();
        this.MachineList = (JsonObject[FileName] as JArray).Select(obj => obj as JObject).ToList();
    }

    public override object GetObjectById(int id)
    {
        JObject machine = MachineList.Where(it => (int)it["id"] == id).SingleOrDefault();
        if (machine != null)
        {
            return new Machine
            {
                ItemId = machine["id"].Value<int>(),
                MoveId = JSONParsingUtil.GetIdFromJObject(machine["move"]),
                MachineName = machine["item"]["name"].Value<string>(),
                MoveName = machine["move"]["name"].Value<string>(),
            };
        }

        UnityEngine.Debug.LogWarning($"No machine found for machine ID: {id}");
        return null;
    }

}
