using System;
using System.Collections;
using System.Collections.Generic;

namespace PocketDarkSouls
{

    public class QuitCommand : Command
    {

        public QuitCommand() : base()
        {
            this.Name = "quit";
        }

        override
        public bool Execute(Player player)
        {
            bool answer = true;
            if (this.HasSecondWord())
            {
                player.messenger.ErrorMessage("\nI cannot quit " + this.SecondWord, ConsoleColor.Red);
                answer = false;
            }
            return answer;
        }
    }
}
