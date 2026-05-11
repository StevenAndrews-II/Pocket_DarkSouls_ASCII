using System;
using PocketDarkSouls;
public class InventoryUnequip : ICs
{
    public string keyword { get; } = "unequip"; // comand to initate this section

    public void Execute(Player p1, string key = null)
    {
        if (key != null)
        {
            p1.UnequipItem(key);
        }
        else
        {
            p1.Messenger.ErrorMessage("What should I unequip..",ConsoleColor.Red);
        }
    }
}
