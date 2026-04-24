using System;
using System.Collections;
using System.Collections.Generic;

namespace PocketDarkSouls
{

    public class InventoryCommand : Command
    {

        public InventoryCommand() : base()
        {
            this.Name = "inventory"; 
        }


        Dictionary<string, ICs> InventoryCommands = new Dictionary<string, ICs>()
        {
            ["open"]            = new InventoryOpen(),
            ["equip"]           = new InventoryEquip(),
            ["unequip"]         = new InventoryUnequip(),
            ["use"]             = new InventoryUse(),
            //["drop"]          = new InventoryDrop(),
            //["markforsale"]   = new InventoryMarkForSale(),
            //["unmarkforsale"] = new InventoryUnMarkForSale(),
        };


        override
        public bool Execute(Player player)
        {

            if (this.HasSecondWord())
            {
                    if (InventoryCommands.ContainsKey(this.SecondWord))
                    {
                        InventoryCommands[this.SecondWord].Execute(player,this.ThirdWord);
                    }
                    else
                    {
                        player.messenger.WarningMessage("\nCant do that... ", ConsoleColor.Yellow);
                    }
                
                return false;
            }
            else
            {
                player.messenger.WarningMessage("\nWhat was I doing?...", ConsoleColor.Yellow);
            }
            return false;
        }
    }
}
