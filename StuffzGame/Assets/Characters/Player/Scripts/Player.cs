public class Player : Singleton<Player>
{
    private Player()
    {
    }

    public PlayerInventory Inventory { get; } = new PlayerInventory();
    public PlayerParty Party { get; } = new PlayerParty();
    public string Name { get; private set; }
    private bool isNameSet;

    internal bool SetName(string name)
    {
        if (!isNameSet)
        {
            this.Name = name;
            isNameSet = true;
            return true;
        }
        else
        {
            return false;
        }
    }
}