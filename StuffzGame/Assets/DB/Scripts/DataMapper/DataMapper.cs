using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using UnityEngine.AddressableAssets;

public abstract class DataMapper
{
    protected abstract string FileName { get; }
    protected abstract JObject JsonObject { get; }
    protected string FileExtension { get; } = ".json";
    protected string JsonDirectory { get; } = "Assets/DB/JSON/";
    protected string ObjectCountTag { get; } = "count";

    protected JObject LoadJSON()
    {
       return new JsonLoader().LoadJSON($"{JsonDirectory}{FileName}{FileExtension}");
    }

    public int GetJSONObjectCount()
    {
        return (int)JsonObject[ObjectCountTag];
    }

    public abstract T GetObjectById<T>(int id);

    // hook to be optionally overriden by subclasses.
    public virtual T GetEvolutionFromSpeciesID<T>(int speciesId, int evolutionChainId)
    {
        throw new InvalidOperationException($"{nameof(DataMapper)}.{nameof(GetEvolutionFromSpeciesID)} is a hook method. This method should be overidden by a subclass and called accordingly");
    }
}