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





        override
        public bool Execute(Player player)
        {

            if (this.HasSecondWord())
            {
                    if (player.InventoryCommands.ContainsKey(this.SecondWord))
                    {
                        player.InventoryCommands[this.SecondWord].Execute(player,this.ThirdWord);
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
