using System;
using PocketDarkSouls;
public class InventoryEquip : ICs
{
    public string keyword { get; } = "equip"; // comand to initate this section

    public void Execute(Player p1, string key = null)
    {
        if (key != null)
        {
            p1.EquipItem(key);
        }
        else
        {
            p1.Messenger.ErrorMessage("What should I equip..", ConsoleColor.Red);
        }
    }
}
