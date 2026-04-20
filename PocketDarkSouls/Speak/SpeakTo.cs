using PocketDarkSouls;


public class SpeakTo : Speak
{

	public Dictionary<string, List<string>> Dialog { get; init; }
    public SpeakTo(Dictionary<string, List<string>> Dialog )
	{
		this.Dialog = Dialog;
	}

	public string keyword { get; } = "to"; // comand to initate this section


	public void Execute(Player p1,Player p2)
	{
		//  p1 = player 
		//  p2 = NPC
		p1.messenger.WarningMessage($"Speaking with : [ {p2.name} : {p2.GetType()} ] ", ConsoleColor.Yellow);
        // needs to hook into p2's handler - maybt his agros NPCS if you ask a lot lol 
        p1.messenger.ReciveMessage(p2.name,p2.dialogHandler.GenericSpeach(this),ConsoleColor.Magenta);
	}
}
