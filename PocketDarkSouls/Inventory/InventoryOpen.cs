using System;
using PocketDarkSouls;
public class InventoryOpen : ICs
{
    public string keyword { get; } = "open"; // comand to initate this section

    public void Execute(Player p1, string key = null)
    {
        p1.messenger.InfoMessage(p1.main_inventory.ReadInventory(),ConsoleColor.White);
    }
}
