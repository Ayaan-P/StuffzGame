public class Player : Singleton<Player>
{
    private Player()
    {
    }

    public PlayerInventory Inventory { get; } = new PlayerInventory();
    public PlayerParty Party { get; } = new PlayerParty();
}