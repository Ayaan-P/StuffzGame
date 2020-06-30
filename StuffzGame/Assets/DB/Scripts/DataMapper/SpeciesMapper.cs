using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

public class SpeciesMapper : DataMapper
{
    protected override string FileName { get => "species"; }
    protected override JObject JsonObject { get; }
    private List<JObject> SpeciesList { get; }

    public SpeciesMapper()
    {
        this.JsonObject = LoadJSON();
        this.SpeciesList = (JsonObject[FileName] as JArray).Select(obj => obj as JObject).ToList();
    }

    public override T GetObjectById<T>(int id)
    {
        JObject species = SpeciesList.Where(it => (int)it["id"] == id).SingleOrDefault();
        if (species != null)
        {
            return (T)Convert.ChangeType(new PokemonSpeciesTemplate
            {
                BaseHappiness = species["base_happiness"].Value<int>(),
                CaptureRate = species["capture_rate"].Value<int>(),
                EggGroups = species["egg_groups"].Value<JArray>().Select(eggGroup => (EggGroup)JSONParsingUtil.GetIdFromJObject(eggGroup)).ToList(),
                EvolvesFromChainId = JSONParsingUtil.GetIdFromJObject(species["evolution_chain"]),
                EvolvesFromSpeciesId = JSONParsingUtil.GetIdFromJObject(species["evolves_from_species"]),
                FlavorText = species["flavor_text"].Value<string>(),
                FormsSwitchable = species["forms_switchable"].Value<bool>(),
                GenderRate = (GenderRate)species["gender_rate"].Value<int>(),
                Genus = species["genus"].Value<string>(),
                GrowthRateId = JSONParsingUtil.GetIdFromJObject(species["growth_rate"]),
                HasGenderDifferences = species["has_gender_differences"].Value<bool>(),
                HatchCounter = species["hatch_counter"].Value<int>(),
                Id = species["id"].Value<int>(),
                IsBaby = species["is_baby"].Value<bool>(),
                Name = species["name"].Value<string>(),
                Order = species["order"].Value<int>(),
                PokemonVarieties = GetPokemonVarietiesDict(species["varieties"].Value<JArray>())
            }, typeof(T));
        }

        UnityEngine.Debug.LogWarning($"No species found for species ID: {id}");
        return default;
    }

    public List<int> GetSpeciesIdForSpeciesThatEvolveFrom(int id)
    {
        List<int> speciesId = new List<int>();
        foreach (JObject speciesJObject in SpeciesList)
        {
            if (speciesJObject != null)
            {
                int evolvesFromId = JSONParsingUtil.GetIdFromJObject(speciesJObject["evolves_from_species"]);
                if (evolvesFromId != -1 && evolvesFromId == id)
                {
                    speciesId.Add(speciesJObject["id"].Value<int>());
                }
            }
        }
        return speciesId;
    }

    private Dictionary<int, bool> GetPokemonVarietiesDict(JArray varieties)
    {
        Dictionary<int, bool> varietiesDict = new Dictionary<int, bool>();
        foreach (JObject variety in varieties)
        {
            varietiesDict.Add(
            JSONParsingUtil.GetIdFromJObject(variety["pokemon"]),
            variety["is_default"].Value<bool>()
                );
        }
        return varietiesDict;
    }
}