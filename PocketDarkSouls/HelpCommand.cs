using System;
using System.Collections;
using System.Collections.Generic;

namespace PocketDarkSouls
{
    public class HelpCommand : Command
    {
        private CommandWords _words;

        public HelpCommand() : this(new CommandWords()){}

        // Designated Constructor
        public HelpCommand(CommandWords commands) : base()
        {
            _words = commands;
            this.Name = "help";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            {
                player.messenger.WarningMessage("\nI cannot help you with " + this.SecondWord, ConsoleColor.Yellow);
            }
            else
            {
                player.messenger.InfoMessage("\nYou are lost... You are alone... You seak penenace with the Gods.. " + _words.Description(), ConsoleColor.Blue);
            }
            return false;
        }
    }
}
