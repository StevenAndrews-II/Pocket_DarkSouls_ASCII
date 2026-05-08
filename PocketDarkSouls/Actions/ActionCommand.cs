using System;

namespace PocketDarkSouls
{

    public class ActionCommand : Command
    {

        public ActionCommand() : base()
        {
            this.Name = "do";
        }

        override
        public bool Execute(Player player)
        {
            if (this.HasSecondWord())
            { 
                if (player.CurrentRoom != null) // guard
                {
                    if (player.CurrentRoom.Actions.ContainsKey(this.SecondWord))
                    {
                        // do the thing man 
                        player.CurrentRoom.Actions[this.SecondWord].Execute();
                        return false;
                    }
                    player.Messenger.WarningMessage($"\nYou shall not do '{this.SecondWord}' in this location... ", ConsoleColor.DarkYellow);
                }
            }
            else
            {
                player.Messenger.WarningMessage("\nDo what?", ConsoleColor.DarkYellow);
            }
            return false;
        }
    }
}
