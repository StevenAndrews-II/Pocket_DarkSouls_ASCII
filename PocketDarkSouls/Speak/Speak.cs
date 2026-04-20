using PocketDarkSouls;


public interface Speak
{

    public Dictionary<string, List<string>> Dialog { get; init; }
    public string keyword {get;}
    public void Execute(Player p1, Player p2);
}
