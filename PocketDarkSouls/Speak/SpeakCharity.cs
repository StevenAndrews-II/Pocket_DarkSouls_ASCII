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
        p1.messenger.WarningMessage($"Speaking with : [ {p2.name} : {p2.GetType()} ] ", ConsoleColor.Blue);
        p1.messenger.ReciveMessage(p2.name,p2.dialogHandler.GenericSpeach(this), ConsoleColor.Magenta);
        while (true)  
        {   
            Console.WriteLine("Input gold total [ 0 to exit ]: ");
            string input = Console.ReadLine();
            if (int.TryParse(input, out int result))
            {
                if (result <= 0)
                {
                    p1.messenger.ReciveMessage(p2.name,p2.dialogHandler.BadCharity(this,0), ConsoleColor.Magenta);
                    break;
                }
                else
                {
                    if (!p1.wallet.GiveGold(result)) {
                        p1.messenger.ErrorMessage("You shouldnt be making enemies here...", ConsoleColor.Red);
                        p1.messenger.ReciveMessage(p2.name,p2.dialogHandler.BadCharity(this,1), ConsoleColor.Magenta);
                        break;
                    }
                    p2.wallet.AddGold(result);
                    p1.messenger.ReciveMessage(p2.name,p2.dialogHandler.ThankYouSpeach(this), ConsoleColor.Magenta);
                    break;
                }
            }

        }
    }
}
