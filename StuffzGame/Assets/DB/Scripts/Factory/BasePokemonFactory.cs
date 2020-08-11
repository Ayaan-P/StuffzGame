using System.Collections.Generic;

public class BasePokemonFactory
{
    private MapperFactory MapperFactory { get; }
 
    public BasePokemonFactory()
    {
        this.MapperFactory = MapperFactory.GetInstance();
    }

    public BasePokemon CreateBasePokemon(int id)
    {
        DataMapper pokemonMapper = MapperFactory.GetMapper(MapperName.POKEMON_MAPPER);
        BasePokemonTemplate pokemonTemplate = pokemonMapper.GetObjectById<BasePokemonTemplate>(id);
        if(pokemonTemplate!= null)
        {
            return new BasePokemon
            {
                Abilities = ConstructPokemonAbilityList(pokemonTemplate),
                Height = pokemonTemplate.Height,
                BaseExperienceOnDefeat = pokemonTemplate.BaseExperienceOnDefeat,
                Id = pokemonTemplate.Id,
                IsDefault = pokemonTemplate.IsDefault,
                PossibleMoveList = ConstructPokemonMoveList(pokemonTemplate),
                Name = pokemonTemplate.Name,
                Order = pokemonTemplate.Order,
                Species = ConstructPokemonSpecies(pokemonTemplate),
                Stats = ConstructStatList(pokemonTemplate),
                Types = pokemonTemplate.Types,
                Weight = pokemonTemplate.Weight,
                Forms = ConstructPokemonForms(pokemonTemplate)
            };
        }
        UnityEngine.Debug.LogError($"No pokemon found with id: {id}");
        return null;
    }

    private List<PokemonForm> ConstructPokemonForms(BasePokemonTemplate pokemonTemplate)
    {
        if (pokemonTemplate.FormIdList == null || pokemonTemplate.FormIdList.Count == 0)
        {
            return null;
        }
        DataMapper formMapper = MapperFactory.GetMapper(MapperName.FORM_MAPPER);
        List<PokemonForm> pokemonFormList = new List<PokemonForm>();
        foreach (int formId in pokemonTemplate.FormIdList)
        {
            PokemonForm form = formMapper.GetObjectById<PokemonForm>(formId);
            if (form.FormOrder != 1)
            {
                pokemonFormList.Add(form);
            }
        }

        return pokemonFormList.Count == 0 ? null : pokemonFormList;
    }

    private List<PokemonAbility> ConstructPokemonAbilityList(BasePokemonTemplate pokemonTemplate)
    {
        DataMapper abilityMapper = MapperFactory.GetMapper(MapperName.ABILITY_MAPPER);
        List<PokemonAbility> pokemonAbilityList = new List<PokemonAbility>();

        for (int i = 0; i < pokemonTemplate.AbilityIdList.Count; i++)
        {
            int abilityId = pokemonTemplate.AbilityIdList[i];
            bool isHidden = pokemonTemplate.IsAbilityHiddenList[i];
            int slot = pokemonTemplate.AbilitySlotList[i];

            BasePokemonAbility baseAbility = abilityMapper.GetObjectById<BasePokemonAbility>(abilityId);

            PokemonAbility ability = new PokemonAbility
            {
                BaseAbility = baseAbility,
                IsHidden = isHidden,
                Slot = slot
            };

            pokemonAbilityList.Add(ability);
        }
        return pokemonAbilityList;
    }

    private List<PokemonMove> ConstructPokemonMoveList(BasePokemonTemplate pokemonTemplate)
    {
        DataMapper moveMapper = MapperFactory.GetMapper(MapperName.MOVE_MAPPER);
        DataMapper statMapper = MapperFactory.GetMapper(MapperName.STAT_MAPPER);
        List<PokemonMove> pokemonMoveList = new List<PokemonMove>();

        for (int i = 0; i < pokemonTemplate.PossibleMoveIdList.Count; i++)
        {
            int moveId = pokemonTemplate.PossibleMoveIdList[i];
            List<MoveLearnDetails> moveLearnDetails = pokemonTemplate.MoveLearnDetailsList[i];

            BasePokemonMoveTemplate baseMoveTemplate = moveMapper.GetObjectById<BasePokemonMoveTemplate>(moveId);
            Dictionary<BasePokemonStat, int> statChangesDict = new Dictionary<BasePokemonStat, int>();

            foreach (int baseStatId in baseMoveTemplate.StatChangesIdDict.Keys)
            {
                BasePokemonStat baseStat = statMapper.GetObjectById<BasePokemonStat>(baseStatId);
                int baseStatValue = baseMoveTemplate.StatChangesIdDict[baseStatId];

                statChangesDict.Add(baseStat, baseStatValue);
            }

            BasePokemonMove baseMove = new BasePokemonMove
            {
                Accuracy = baseMoveTemplate.Accuracy,
                MoveDamageClass = baseMoveTemplate.MoveDamageClass,
                EffectChance = baseMoveTemplate.EffectChance,
                EffectEntries = baseMoveTemplate.EffectEntries,
                FlavorText = baseMoveTemplate.FlavorText,
                Id = baseMoveTemplate.Id,
                Ailment = baseMoveTemplate.Ailment,
                AilmentChance = baseMoveTemplate.AilmentChance,
                Category = baseMoveTemplate.Category,
                CritRate = baseMoveTemplate.CritRate,
                Drain = baseMoveTemplate.Drain,
                FlinchChance = baseMoveTemplate.FlinchChance,
                Healing = baseMoveTemplate.Healing,
                MaxHits = baseMoveTemplate.MaxHits,
                MinHits = baseMoveTemplate.MinHits,
                MaxTurns = baseMoveTemplate.MaxTurns,
                MinTurns = baseMoveTemplate.MinTurns,
                StatChance = baseMoveTemplate.StatChance,
                Name = baseMoveTemplate.Name,
                Power = baseMoveTemplate.Power,
                PP = baseMoveTemplate.PP,
                Priority = baseMoveTemplate.Priority,
                StatChanges = statChangesDict,
                Target = baseMoveTemplate.Target,
                Type = baseMoveTemplate.Type
            };

            PokemonMove pokemonMove = new PokemonMove
            {
                BaseMove = baseMove,
                CurrentPP = baseMove.PP,
                MoveLearnDetails = moveLearnDetails
            };

            pokemonMoveList.Add(pokemonMove);
        }
        return pokemonMoveList;
    }

    private PokemonSpecies ConstructPokemonSpecies(BasePokemonTemplate pokemonTemplate)
    {
        DataMapper speciesMapper = MapperFactory.GetMapper(MapperName.SPECIES_MAPPER);
        DataMapper evolutionMapper = MapperFactory.GetMapper(MapperName.EVOLUTION_MAPPER);
        DataMapper growthRateMapper = MapperFactory.GetMapper(MapperName.GROWTH_RATE_MAPPER);

        int pokemonSpeciesId = pokemonTemplate.SpeciesId;

        PokemonSpeciesTemplate speciesTemplate = speciesMapper.GetObjectById<PokemonSpeciesTemplate>(pokemonSpeciesId);
        int evolvesFromSpeciesId = speciesTemplate.EvolvesFromSpeciesId;
        int evolutionId = speciesTemplate.EvolvesFromChainId;
        int growthRateId = speciesTemplate.GrowthRateId;

        PokemonEvolution evolvesFrom = evolutionMapper.GetEvolutionFromSpeciesID<PokemonEvolution>(evolvesFromSpeciesId, evolutionId);
        PokemonGrowthRate growthRate = growthRateMapper.GetObjectById<PokemonGrowthRate>(growthRateId);

        PokemonSpecies pokemonSpecies = new PokemonSpecies
        {
            BaseHappiness = speciesTemplate.BaseHappiness,
            CaptureRate = speciesTemplate.CaptureRate,
            EggGroups = speciesTemplate.EggGroups,
            EvolvesFrom = evolvesFrom,
            EvolvesFromSpeciesId = evolvesFromSpeciesId,
            FlavorText = speciesTemplate.FlavorText,
            FormsSwitchable = speciesTemplate.FormsSwitchable,
            GenderRate = speciesTemplate.GenderRate,
            Genus = speciesTemplate.Genus,
            GrowthRate = growthRate,
            HasGenderDifferences = speciesTemplate.HasGenderDifferences,
            HatchCounter = speciesTemplate.HatchCounter,
            Id = speciesTemplate.Id,
            IsBaby = speciesTemplate.IsBaby,
            Name = speciesTemplate.Name,
            Order = speciesTemplate.Order,
            PokemonVarieties = speciesTemplate.PokemonVarieties
        };

        return pokemonSpecies;
    }

    private List<PokemonStat> ConstructStatList(BasePokemonTemplate pokemonTemplate)
    {
        DataMapper statMapper = MapperFactory.GetMapper(MapperName.STAT_MAPPER);
        List<PokemonStat> statList = new List<PokemonStat>();

        for (int i = 0; i < pokemonTemplate.BaseStatIdList.Count; i++)
        {
            int baseStatId = pokemonTemplate.BaseStatIdList[i];
            int EVsGainedOnDefeat = pokemonTemplate.EVsGainedOnDefeatList[i];
            int baseStatValue = pokemonTemplate.BaseStatValueList[i];

            BasePokemonStat basePokemonStat = statMapper.GetObjectById<BasePokemonStat>(baseStatId);

            PokemonStat pokemonStat = new PokemonStat
            {
                BaseStat = basePokemonStat,
                EVsGainedOnDefeat = EVsGainedOnDefeat,
                BaseValue = baseStatValue,
                CalculatedValue = baseStatValue,
                CurrentValue = baseStatValue,
                IV = null,
                EV = null
            };
            statList.Add(pokemonStat);
        }
        return statList;
    }
}