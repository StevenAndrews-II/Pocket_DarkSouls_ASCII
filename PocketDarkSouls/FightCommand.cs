using System;
using System.Collections;
using System.Collections.Generic;

namespace PocketDarkSouls
{
    public class FightCommand : Command
    {

        public FightCommand() : base()
        {
            this.Name = "fight";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                Player p2 = player.CurrentRoom.FindPlayerInRoom(this.SecondWord);

                if (p2 != null)
                {


                    // loop
                }
                return false;
            }
            else
            {
                player.messenger.ErrorMessage("\nFight whom?", ConsoleColor.Red);
            }
            return false;
        }
    }
}
