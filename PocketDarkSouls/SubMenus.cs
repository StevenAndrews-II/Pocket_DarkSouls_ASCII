using System;
using PocketDarkSouls;
public class SubMenus
{

    public void UseItemMenu(Player p1, string? key = null)
    {
        // Window loop
        bool used = false;
        while (key != null)
        {
            Console.WriteLine(p1.Inventory.getItemInfo(key), ConsoleColor.White);
            Console.WriteLine("Input an ammount to use:", ConsoleColor.White);
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
                    used = p1.Inventory.useItem(key, result);
                    break;
                }
            }
            Console.Clear();
        }

        if (!used)
        {
            p1.Messenger.WarningMessage("Item could not be used..", ConsoleColor.Red);
        }
    }





}
