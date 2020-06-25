using System.Collections.Generic;

public class ItemFactory
{
    private int SEED = 69;
    private readonly System.Random random;
    private MapperFactory MapperFactory { get; }

    private static Dictionary<ItemPocket, List<ItemCategory>> pocketCategoryDict = new Dictionary<ItemPocket, List<ItemCategory>>
        {
            { ItemPocket.BATTLE, new List<ItemCategory>{ ItemCategory.STAT_BOOSTS,ItemCategory.FLUTES,ItemCategory.MIRACLE_SHOOTER} },
            { ItemPocket.POKEBALLS, new List<ItemCategory>{ ItemCategory.SPECIAL_BALLS,ItemCategory.STANDARD_BALLS, ItemCategory.APRICORN_BALLS} },
            { ItemPocket.MAIL, new List<ItemCategory>{ItemCategory.ALL_MAIL} },
            { ItemPocket.MEDICINE, new List<ItemCategory>{ItemCategory.VITAMINS,ItemCategory.HEALING,ItemCategory.PP_RECOVERY,ItemCategory.REVIVAL,ItemCategory.STATUS_CURES} },
            { ItemPocket.MACHINES, new List<ItemCategory>{ItemCategory.ALL_MACHINES } },
            { ItemPocket.BERRIES, new List<ItemCategory>{ ItemCategory.EFFORT_DROP,ItemCategory.MEDICINE,ItemCategory.OTHER,ItemCategory.IN_A_PINCH,ItemCategory.PICKY_HEALING,ItemCategory.TYPE_PROTECTION,ItemCategory.BAKING_ONLY} },
            { ItemPocket.KEY, new List<ItemCategory>{ ItemCategory.EVENT_ITEMS,ItemCategory.GAMEPLAY,ItemCategory.PLOT_ADVANCEMENT,ItemCategory.UNUSED,ItemCategory.APRICORN_BOX,ItemCategory.DATA_CARDS,ItemCategory.Z_CRYSTALS} },
            { ItemPocket.MISC, new List<ItemCategory>{ ItemCategory.COLLECTIBLES,ItemCategory.EVOLUTION,ItemCategory.SPELUNKING,ItemCategory.HELD_ITEMS,ItemCategory.CHOICE,ItemCategory.EFFORT_TRAINING,ItemCategory.BAD_HELD_ITEMS,ItemCategory.TRAINING,ItemCategory.PLATES,ItemCategory.SPECIES_SPECIFIC,ItemCategory.TYPE_ENHANCEMENT,ItemCategory.LOOT,ItemCategory.MULCH,ItemCategory.DEX_COMPLETION,ItemCategory.SCARVES,ItemCategory.JEWELS,ItemCategory.MEGA_STONES,ItemCategory.MEMORIES} }
        };

    public ItemFactory()
    {
        this.random = new System.Random(SEED);
        this.MapperFactory = MapperFactory.GetInstance();
    }

    public Item CreateItem(int id)
    {
        DataMapper itemMapper = MapperFactory.GetMapper(MapperName.ITEM_MAPPER);
        if (itemMapper.GetObjectById<Item>(id) is Item item)
        {
            item.Count = item.Attributes.Contains(ItemAttribute.COUNTABLE) ? 1 : (int?)null;
            item.Pocket = GetPocketForItemCategory(item.Category);
            if (item.Pocket == ItemPocket.MACHINES)
            {
                item.IsBerry = false;
                item.IsMachine = true;
                return CreateTM(item);
            }
            else if (item.Pocket == ItemPocket.BERRIES)
            {
                item.IsBerry = true;
                item.IsMachine = false;
                return CreateBerry(item);
            }
            else
            {
                item.IsBerry = false;
                item.IsMachine = false;
                return item;
            }
        }
        UnityEngine.Debug.LogError($"No item found with id: {id}");
        return null;
    }

    private Berry CreateBerry(Item item)
    {
        int id = item.Id;
        DataMapper berryMapper = MapperFactory.GetMapper(MapperName.BERRY_MAPPER);
        if (berryMapper.GetObjectById<Berry>(id) is Berry berry)
        {
            berry.Attributes = item.Attributes;
            berry.BabyTriggerForEvolutionId = item.BabyTriggerForEvolutionId;
            berry.Category = item.Category;
            berry.Cost = item.Cost;
            berry.EffectEntries = item.EffectEntries;
            berry.FlavorText = item.FlavorText;
            berry.FlingEffect = item.FlingEffect;
            berry.FlingPower = item.FlingPower;
            berry.Id = item.Id;
            berry.Name = item.Name;
            berry.Count = item.Count;
            berry.Pocket = item.Pocket;
            berry.IsBerry = item.IsBerry;
            berry.IsMachine = item.IsMachine;
            return berry;
        }
        UnityEngine.Debug.LogError($"No berry found with id: {id}");
        return null;
    }

    private Machine CreateTM(Item item)
    {
        int id = item.Id;
        DataMapper machineMapper = MapperFactory.GetMapper(MapperName.MACHINE_MAPPER);
        DataMapper moveMapper = MapperFactory.GetMapper(MapperName.MOVE_MAPPER);

        if (machineMapper.GetObjectById<Machine>(id) is Machine tm)
        {
            PokemonType tmType = moveMapper.GetObjectById<BasePokemonMoveTemplate>(tm.MoveId).Type;
            tm.TMType = tmType;
            tm.Attributes = item.Attributes;
            tm.BabyTriggerForEvolutionId = item.BabyTriggerForEvolutionId;
            tm.Category = item.Category;
            tm.Cost = item.Cost;
            tm.EffectEntries = item.EffectEntries;
            tm.FlavorText = item.FlavorText;
            tm.FlingEffect = item.FlingEffect;
            tm.FlingPower = item.FlingPower;
            tm.Id = item.Id;
            tm.Name = item.Name;
            tm.Count = item.Count;
            tm.Pocket = item.Pocket;
            tm.IsBerry = item.IsBerry;
            tm.IsMachine = item.IsMachine;
            return tm;
        }
        UnityEngine.Debug.LogError($"No Machine (TM) found with id: {id}");
        return null;
    }

    public Item CreateRandomItem()
    {
        DataMapper itemMapper = MapperFactory.GetMapper(MapperName.ITEM_MAPPER);

        int maxId = itemMapper.GetJSONObjectCount();
        int randomId = random.Next(1, maxId + 1);
        return CreateItem(randomId);
    }

    private ItemPocket GetPocketForItemCategory(ItemCategory category)
    {
        foreach (ItemPocket pocket in pocketCategoryDict.Keys)
        {
            if (pocketCategoryDict[pocket].Contains(category))
            {
                return pocket;
            }
        }
        // else return MISC, but this shouln't get called.
        UnityEngine.Debug.LogError($"Could not find ItemPocket for ItemCategory: {category}");
        return ItemPocket.MISC;
    }
}