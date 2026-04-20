using System.Collections;
using System.Collections.Generic;
using System;

namespace PocketDarkSouls
{
    /*
     * Spring 2026
     * This is a simple parser. It can only accept 
     * commands with upmost 1 parameter. You may modify
     * it along with the Command Abstract class.
     */
    public class Parser
    {
        private CommandWords _commands;

        public Parser() : this(new CommandWords()){}

        // Designated Constructor
        public Parser(CommandWords newCommands)
        {
            _commands = newCommands;
        }

        public Command ParseCommand(string commandString)
        {
            Command command = null;
            string[] words = commandString.Split(' ');
            if (words.Length > 0)
            {
                command = _commands.Get(words[0]);
                if (command != null)
                {
                    

                    if (words.Length > 1 )
                    {
                        command.SecondWord = words[1];
                    }
                    else
                    {
                        command.SecondWord = null;
                    }

                    if (words.Length >2 )
                    {
                        command.ThirdWord = words[2];
                    }
                    else
                    {
                        command.ThirdWord = null;
                    }

                    if (words.Length > 3)
                    {
                        command.ForthWord = words[3];
                    }
                    else
                    {
                        command.ForthWord = null;
                    }

                }
                else
                {
                    // This is debug line of code, should remove for regular execution
                    //Console.WriteLine(">>>Did not find the command " + words[0]);
                }
            }
            else
            {
                // This is a debug line of code
                //Console.WriteLine("No words parsed!");
            }
            return command;
        }

        public string Description()
        {
            return _commands.Description();
        }
    }
}
