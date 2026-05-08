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
        p1.Messenger.WarningMessage($"Trading with : [ {p2.Name} : {p2.GetType()} ] ", ConsoleColor.Yellow);
        // needs to hook into p2's interactions menu
        
        
        // menu
        
        List <Item> forsale  = p2.Inventory.getAllItemsMarkedForSale();

        if (forsale.Count > 0)
        {
            p1.Messenger.ReciveMessage(p2.Name,p2.DialogHandler.TradeSpeach(this.Dialog),ConsoleColor.Magenta);
            
            

            int option_select = 0;
            while (true)
            {
                Console.WriteLine("------------------------------------[ Trade Menu ]------------------------------------", ConsoleColor.White);
                for (int i = 0; i < forsale.Count; i++)
                {
                    Console.WriteLine($"{i,-3} : {forsale[i].id,-35}  >> ", ConsoleColor.White);
                    Console.WriteLine(forsale[i].ToString(), ConsoleColor.White);                   // use abstract ovveride of ToString 
                   
                }
                Console.WriteLine($"Selected [  number -1 to exit ] : ", ConsoleColor.Yellow);
                string input = Console.ReadLine();
                if (int.TryParse(input, out int result))
                {
                    if (result >= 0 && result < forsale.Count)
                    {
                        option_select = result;
                        break; 
                    }
                    Console.WriteLine($"Selected:  {result}", ConsoleColor.Yellow);

                    if (result < 0)
                    {
                        p1.Messenger.ReciveMessage(p2.Name,p2.DialogHandler.QuitTrade(this.Dialog),ConsoleColor.Magenta);
                        return;
                    }
                }
                else
                {
                    Console.Clear();
                }
            }

            Item? purchase = p2.Inventory.getForSaleItem(forsale[option_select].id);
            // handle case where item is no longer for sale
            if (purchase == null)
            {
                p1.Messenger.ReciveMessage(p2.Name,"Seems there is nothing available for purchase.", ConsoleColor.Magenta);
                return;
            }
            // handle cash transaction 
            
            if (p1.Wallet.gold < purchase.price)
            {
                p1.Messenger.ReciveMessage(p2.Name,p2.DialogHandler.NotEnoughToTrade(this.Dialog), ConsoleColor.Magenta);
                return;
            }
            else
            {
                p1.Wallet.GiveGold(purchase.price);
                p2.Wallet.AddGold(purchase.price);
            }
            // transfer item 
            p2.Inventory.SoldItem(purchase.id, 1);
            p1.Inventory.AddItem(purchase);
        }
        else
        {
            p1.Messenger.ReciveMessage(p2.Name,p2.DialogHandler.NothingToTradeSpeach(this.Dialog), ConsoleColor.Magenta);
            return;
        }


        p1.Messenger.ReciveMessage(p2.Name,p2.DialogHandler.ThankYouSpeach(this.Dialog), ConsoleColor.Magenta);
        
    }
}
