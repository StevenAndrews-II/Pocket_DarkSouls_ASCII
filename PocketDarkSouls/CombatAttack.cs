using PocketDarkSouls;
public class CombatAttack : Combat
{

    public Dictionary<string, List<string>> Dialog { get; init; }
    public CombatAttack(Dictionary<string, List<string>> Dialog)
    {
        this.Dialog = Dialog;
    }

    public string keyword { get; } = "attack"; // comand to initate this section


    public void Execute(Player p1, Player p2)
    {
        p1.messenger.WarningMessage($"In combat with : [ {p2.name} : {p2.GetType()} ] ", ConsoleColor.DarkRed);
       

        
    }
}