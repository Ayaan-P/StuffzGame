using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
public class MapperFactory
{
    private Dictionary<MapperName, DataMapper> mapperInstances;

    private volatile static MapperFactory uniqueInstance;  //volatile so you instantiate and synchronize lazily
    private static readonly object padlock = new object();
    private static readonly bool enableDebug = false;

    private MapperFactory()
    {
        mapperInstances = new Dictionary<MapperName, DataMapper>();
    }

    public static MapperFactory GetInstance()
    {
        if (uniqueInstance == null)
        {
            lock (padlock)
            {
                if (uniqueInstance == null) // check again to be thread-safe
                {
                    UnityEngine.Debug.LogWarning($"No {nameof(MapperFactory)} instance found. Creating new instance");
                    uniqueInstance = new MapperFactory();
                }
            }
        }
        if (enableDebug) { UnityEngine.Debug.Log($"{nameof(MapperFactory)} instance found! Returning existing instance"); }
        return uniqueInstance;
    }

    public DataMapper GetMapper(MapperName mapperName)
    {
        if (mapperInstances.ContainsKey(mapperName))
        {
            if (enableDebug) { UnityEngine.Debug.Log($"Returned existing instance of DataMapper: {mapperName}"); }
            return mapperInstances[mapperName];
        }
        else
        {
            switch (mapperName)
            {
                case MapperName.MOVE_MAPPER:
                    mapperInstances.Add(mapperName, new MoveMapper());
                    break;

                case MapperName.POKEMON_MAPPER:
                    mapperInstances.Add(mapperName, new PokemonMapper());
                    break;

                case MapperName.ABILITY_MAPPER:
                    mapperInstances.Add(mapperName, new AbilityMapper());
                    break;

                case MapperName.NATURE_MAPPER:
                    mapperInstances.Add(mapperName, new NatureMapper());
                    break;

                case MapperName.ITEM_MAPPER:
                    mapperInstances.Add(mapperName, new ItemMapper());
                    break;

                case MapperName.STAT_MAPPER:
                    mapperInstances.Add(mapperName, new StatMapper());
                    break;

                case MapperName.GROWTH_RATE_MAPPER:
                    mapperInstances.Add(mapperName, new GrowthRateMapper());
                    break;

                case MapperName.EVOLUTION_MAPPER:
                    mapperInstances.Add(mapperName, new EvolutionMapper());
                    break;

                case MapperName.BERRY_MAPPER:
                    mapperInstances.Add(mapperName, new BerryMapper());
                    break;

                case MapperName.SPECIES_MAPPER:
                    mapperInstances.Add(mapperName, new SpeciesMapper());
                    break;
                case MapperName.MACHINE_MAPPER:
                    mapperInstances.Add(mapperName, new MachineMapper());
                    break;
                case MapperName.FORM_MAPPER:
                    mapperInstances.Add(mapperName, new FormMapper());
                    break;
            }
            if (enableDebug) { UnityEngine.Debug.Log($"DataMapper not found in mapperInstances. Creating new instance of DataMapper: {mapperName}"); }
            return mapperInstances[mapperName];
        }
    }
}

public enum MapperName
{
    MOVE_MAPPER,
    POKEMON_MAPPER,
    ABILITY_MAPPER,
    NATURE_MAPPER,
    ITEM_MAPPER,
    STAT_MAPPER,
    GROWTH_RATE_MAPPER,
    EVOLUTION_MAPPER,
    BERRY_MAPPER,
    SPECIES_MAPPER,
    MACHINE_MAPPER,
    FORM_MAPPER
}