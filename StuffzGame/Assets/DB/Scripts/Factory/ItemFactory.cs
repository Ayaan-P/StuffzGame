public class ItemFactory
{
    private int SEED = 69;
    private readonly System.Random random;
    private MapperFactory MapperFactory { get; }

    public ItemFactory()
    {
        this.random = new System.Random(SEED);
        this.MapperFactory = MapperFactory.GetInstance();
    }

    public Item CreateItem(int id)
    {
        DataMapper itemMapper = MapperFactory.GetMapper(MapperName.ITEM_MAPPER);
        if (itemMapper.GetObjectById(id) is Item item)
        {
            return item;
        }
        UnityEngine.Debug.LogError($"No item found with id: {id}");
        return null;
    }

    public Berry CreateBerry(int id)
    {
        DataMapper berryMapper = MapperFactory.GetMapper(MapperName.BERRY_MAPPER);
        if (berryMapper.GetObjectById(id) is Berry berry)
        {
            return berry;
        }
        UnityEngine.Debug.LogError($"No berry found with id: {id}");
        return null;
    }

    public Machine CreateTM(int id)
    {
        DataMapper machineMapper = MapperFactory.GetMapper(MapperName.MACHINE_MAPPER);
        if (machineMapper.GetObjectById(id) is Machine tm)
        {
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
}