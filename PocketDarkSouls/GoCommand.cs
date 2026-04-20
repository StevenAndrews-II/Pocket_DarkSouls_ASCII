using System;
using System.Collections;
using System.Collections.Generic;

namespace PocketDarkSouls
{
    public class GoCommand : Command
    {

        public GoCommand() : base()
        {
            this.Name = "go";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.goTo(this.SecondWord);
            }
            else
            {
                player.messenger.ErrorMessage("\nGo Where?", ConsoleColor.Red);
            }
            return false;
        }
    }
}
