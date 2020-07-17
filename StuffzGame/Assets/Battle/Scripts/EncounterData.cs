using System.Collections.Generic;
using UnityEngine;

public class EncounterData : Singleton<EncounterData>
{

    private EncounterData() { }

    private bool? isWildEncounter = null;
    private Pokemon currentWildPokemon;
    private List<Pokemon> currentTrainerParty;
    public KeyValuePair<BattleTerrain,TimeOfDay> TerrainAndTime { get; set; }

    private void ClearPreviousEncounters()
    {
        TerrainAndTime = new KeyValuePair<BattleTerrain,TimeOfDay>(BattleTerrain.MEADOWS, TimeOfDay.DAY);
        isWildEncounter = null;
        currentWildPokemon = null;
        currentTrainerParty = null;
    }
    public void SetWildPokemonEncounter(Pokemon wildPokemon)
    {
        ClearPreviousEncounters();
        isWildEncounter = true;
        currentWildPokemon = wildPokemon;
    }

    public void SetTrainerEncounter(List<Pokemon> trainerParty)
    {
        ClearPreviousEncounters();
        isWildEncounter = false;
        currentTrainerParty = trainerParty;

    }

    public List<Pokemon> GetCurrentEncounterData()
    {
        if(isWildEncounter == null)
        {
            Debug.LogError("No encounter data set!");
            return null;
        }
        else if ((bool)isWildEncounter)
        {
            return new List<Pokemon> { currentWildPokemon };
        }
        else
        {
            return new List<Pokemon>(currentTrainerParty);
        }
    }

}