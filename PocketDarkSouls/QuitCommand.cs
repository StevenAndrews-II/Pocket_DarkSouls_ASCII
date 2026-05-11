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

        
        public override bool Execute(Player player)
        {
            player.Messenger.InfoMessage("Thanks for playing.", ConsoleColor.Green);
            GameManager.Instance.QuitGame();
            return true;
        }
    }
}
