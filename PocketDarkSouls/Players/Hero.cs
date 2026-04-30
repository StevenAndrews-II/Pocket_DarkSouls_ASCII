using PocketDarkSouls;

public class Hero : Player
{
    public Hero(
        string name,
        List<Speak> dialog,
        EntityEvents events,
        Room room)
        : base(name, dialog, events, room)
    {
    }
}
