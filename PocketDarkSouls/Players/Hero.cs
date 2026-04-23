using PocketDarkSouls;

public class Hero : Player
{
    public Hero(
        string name,
        List<Speak> dialog,
        Inventory I_,
        List<ICs> InventoryCommands,
        EntityEvents events,
        Wallet W_,
        HealthSystem H_,
        Room room)
        : base(name, dialog, I_, InventoryCommands, events, W_, H_, room)
    {
    }
}
