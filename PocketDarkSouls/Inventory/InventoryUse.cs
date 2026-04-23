
using PocketDarkSouls;
public class InventoryUse : ICs
{
    public string keyword { get; } = "use"; // comand to initate this section

    public void Execute(Player p1, string? key = null)
    {
        // Window loop
        bool used = false;
        while (key != null)
        {
            Console.WriteLine(p1.main_inventory.getItemInfo(key),ConsoleColor.White);
            Console.WriteLine("Input an ammount to use:",ConsoleColor.White);
            string input = Console.ReadLine();
            if (int.TryParse(input, out int result))
            {
                if (result <= 0)
                {
                    Console.WriteLine("Canceled...", ConsoleColor.Red);
                    break;
                }
                else
                {
                    used = p1.main_inventory.useItem(key, result);
                    break;
                }
            }
            Console.Clear();
        }

        if (!used)
        {
            p1.messenger.WarningMessage("Item could not be used..", ConsoleColor.Red);
        }
    }
}
