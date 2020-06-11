using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;

public abstract class DataMapper
{
    protected abstract string FileName { get; }
    protected abstract JObject JsonObject { get; }
    protected string FileExtension { get; } = ".json";
    protected string JsonDirectory { get; } = "Assets/DB/JSON/";
    protected string ObjectCountTag { get; } = "count";

    protected JObject LoadJSON()
    {
        JObject jsonFile;
        using (FileStream fileStream = File.Open($"{JsonDirectory}{FileName}{FileExtension}", FileMode.Open))
        using (StreamReader streamReader = new StreamReader(fileStream))
        using (JsonReader jsonReader = new JsonTextReader(streamReader))
        {
            jsonFile = (JObject)JToken.ReadFrom(jsonReader);
        }
        return jsonFile;
    }

    public int GetJSONObjectCount()
    {
        return (int)JsonObject[ObjectCountTag];
    }

    public abstract object GetObjectById(int id);

    // hook to be optionally overriden by subclasses.
    public virtual object GetEvolutionFromSpeciesID(int speciesId, int evolutionChainId)
    {
        throw new InvalidOperationException($"{nameof(DataMapper)}.{nameof(GetEvolutionFromSpeciesID)} is a hook method. This method should be overidden by a subclass and called accordingly");
    }
}