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
        p1.Messenger.WarningMessage($"Speaking with : [ {p2.Name} : {p2.GetType()} ] ", ConsoleColor.Blue);
        p1.Messenger.ReciveMessage(p2.Name,p2.DialogHandler.GenericSpeach(this.Dialog), ConsoleColor.Magenta);
        while (true)  
        {   
            Console.WriteLine("Input gold total [ 0 to exit ]: ");
            string input = Console.ReadLine();
            if (int.TryParse(input, out int result))
            {
                if (result <= 0)
                {
                    p1.Messenger.ReciveMessage(p2.Name,p2.DialogHandler.BadCharity(this.Dialog, 0), ConsoleColor.Magenta);
                    break;
                }
                else
                {
                    if (!p1.Wallet.GiveGold(result)) {
                        p1.Messenger.ErrorMessage("You shouldnt be making enemies here...", ConsoleColor.Red);
                        p1.Messenger.ReciveMessage(p2.Name,p2.DialogHandler.BadCharity(this.Dialog,1), ConsoleColor.Magenta);
                        break;
                    }
                    p2.Wallet.AddGold(result);
                    p1.Messenger.ReciveMessage(p2.Name,p2.DialogHandler.ThankYouSpeach(this.Dialog), ConsoleColor.Magenta);
                    break;
                }
            }

        }
    }
}
