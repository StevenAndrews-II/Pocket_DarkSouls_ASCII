using System;
using PocketDarkSouls;
public class InventoryUnequip : ICs
{
    public string keyword { get; } = "unequip"; // comand to initate this section

    public void Execute(Player p1, string key = null)
    {
        if (key != null)
        {
            bool check = p1.main_inventory.Unequip(key);
            if (!check)
            {
                p1.messenger.ErrorMessage("Not Equipped... ", ConsoleColor.Red);
            }
        }
        else
        {
            p1.messenger.ErrorMessage("What should I unequip..",ConsoleColor.Red);
        }
    }
}
