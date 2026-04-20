using PocketDarkSouls;

public class SpeakTrade : Speak
{

    public Dictionary<string, List<string>> Dialog { get; init; }
    public SpeakTrade(Dictionary<string, List<string>> Dialog)
    {
        this.Dialog = Dialog;
    }

    public string keyword { get; } = "trade"; // comand to initate this section


    public void Execute(Player p1, Player p2)
    {
        //  p1 = player 
        //  p2 = NPC
        p1.messenger.WarningMessage($"Trading with : [ {p2.name} : {p2.GetType()} ] ", ConsoleColor.Yellow);
        // needs to hook into p2's interactions menu
        
        
        // menu
        
        List <Item> forsale  = p2.main_inventory.getAllItemsMarkedForSale();

        if (forsale.Count > 0)
        {
            p1.messenger.ReciveMessage(p2.name,p2.dialogHandler.TradeSpeach(this),ConsoleColor.Magenta);
            
            

            int option_select = 0;
            while (true)
            {
                Console.WriteLine("--------------------------------[ Trade Menu ]--------------------------------", ConsoleColor.White);
                for (int i = 0; i < forsale.Count; i++)
                {
                    Console.WriteLine($"{i,-3} : {forsale[i].id,-35}  >> ", ConsoleColor.White);
                    Console.WriteLine(forsale[i].ToString(), ConsoleColor.White);                   // use abstract ovveride of ToString 
                   
                }
                Console.WriteLine($"Selected [  number -1 to exit ] : ", ConsoleColor.Yellow);
                string input = Console.ReadLine();
                if (int.TryParse(input, out int result))
                {
                    if (result >= 0 && result <= forsale.Count)
                    {
                        option_select = result;
                        break; 
                    }
                    Console.WriteLine($"Selected:  {result}", ConsoleColor.Yellow);

                    if (result < 0)
                    {
                        p1.messenger.ReciveMessage(p2.name,p2.dialogHandler.QuitTrade(this),ConsoleColor.Magenta);
                        return;
                    }
                }
                else
                {
                    Console.Clear();
                }
            }

            Item purchase = p2.main_inventory.getForSaleItem(forsale[option_select].id);
            // handle money transaction 
            if (p1.wallet.gold < purchase.price)
            {
                p1.messenger.ReciveMessage(p2.name,p2.dialogHandler.NotEnoughToTrade(this), ConsoleColor.Magenta);
                return;
            }
            else
            {
                p1.wallet.GiveGold(purchase.price);
                p2.wallet.AddGold(purchase.price);
            }
            // transfer item 
            p2.main_inventory.DelItem(purchase.id, 1);
            p1.main_inventory.AddItem(purchase);
        }
        else
        {
            p1.messenger.ReciveMessage(p2.name,p2.dialogHandler.NothingToTradeSpeach(this), ConsoleColor.Magenta);
            return;
        }


        p1.messenger.ReciveMessage(p2.name,p2.dialogHandler.ThankYouSpeach(this), ConsoleColor.Magenta);
        
    }
}
