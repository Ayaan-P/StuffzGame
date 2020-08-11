using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;

public class JsonLoader
{
    public JObject LoadJSON(string path)
    {
        JObject jsonFile;
        using (FileStream fileStream = File.Open(path, FileMode.Open))
        using (StreamReader streamReader = new StreamReader(fileStream))
        using (JsonReader jsonReader = new JsonTextReader(streamReader))
        {
            jsonFile = (JObject)JToken.ReadFrom(jsonReader);
        }
        return jsonFile;
    }
}
