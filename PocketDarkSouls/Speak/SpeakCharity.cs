using PocketDarkSouls;

public class SpeakCharity : Speak
{

    public Dictionary<string, List<string>> Dialog { get; init; }
    public SpeakCharity(Dictionary<string, List<string>> Dialog)
    {
        this.Dialog = Dialog;
    }

    public string keyword { get; } = "charity"; // comand to initate this section


    public void Execute(Player p1, Player p2)
    {
       p1.CharityMenu(p2)
    }
}
