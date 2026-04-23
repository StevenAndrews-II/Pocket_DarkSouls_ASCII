using PocketDarkSouls;


public class SpeakTo : Speak
{

	public Dictionary<string, List<string>> Dialog { get; init; }
    public SpeakTo(Dictionary<string, List<string>> Dialog )
	{
		this.Dialog = Dialog;
	}

	public string keyword { get; } = "to"; // comand to initate this section

    // P1 = the one who is speaking
	// P2 = the one who is being spoken to
    public void Execute(Player p1,Player p2)
	{
		p1.messenger.WarningMessage($"Speaking with : [ {p2.name} : {p2.GetType()} ] ", ConsoleColor.Yellow);
        // hooks p2s dialog handler to p1s messenger and sends random message in the dialog list
        p1.messenger.ReciveMessage(p2.name,p2.dialogHandler.GenericSpeach(this),ConsoleColor.Magenta);
	}
}
