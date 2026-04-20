using System;
using PocketDarkSouls;
public class InventoryEquip : ICs
{
    public string keyword { get; } = "equip"; // comand to initate this section

    public void Execute(Player p1, string key = null)
    {
        if (key != null)
        {
            bool check = p1.main_inventory.Equip(key);
            if (!check) {
                p1.messenger.ErrorMessage("Not equipable...", ConsoleColor.Red);
            }
        }
        else
        {
            p1.messenger.ErrorMessage("What should I equip..", ConsoleColor.Red);
        }
    }
}
