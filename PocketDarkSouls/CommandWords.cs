using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace PocketDarkSouls
{
    /* 
     * Spring 2026
     * This class holds all the commands that can be used
     * with a parser. You may create different instances
     * with different commands according to your needs.
     */
    public class CommandWords
    {
        private Dictionary<string, Command> _commands;
        private static Command[] _commandArray = { new GoCommand(), new QuitCommand() , new ActionCommand(), new InventoryCommand() , new TalkCommand(), new FightCommand()};

        public CommandWords() : this(_commandArray) {}



        // Designated Constructor
        public CommandWords(Command[] commandList)
        {
            _commands = new Dictionary<string, Command>();
            foreach (Command command in commandList)
            {
                _commands[command.Name] = command;
            }
            Command help = new HelpCommand(this);
            _commands[help.Name] = help;
        }



        public Command Get(string word)
        {
            Command command = null;
            _commands.TryGetValue(word, out command);
            return command;
        }



        public string Description()
        {
            string commandNames = "";
            Dictionary<string, Command>.KeyCollection keys = _commands.Keys;
            foreach (string commandName in keys)
            {
                commandNames += " " + commandName;
            }
            return commandNames;
        }
    }
}
