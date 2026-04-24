using PocketDarkSouls;

public class Hero : Player
{
    public Hero(
        string name,
        List<Speak> dialog,
        Inventory I_,
        EntityEvents events,
        Wallet W_,
        HealthSystem H_,
        Room room)
        : base(name, dialog, I_, events, W_, H_, room)
    {
    }
}
