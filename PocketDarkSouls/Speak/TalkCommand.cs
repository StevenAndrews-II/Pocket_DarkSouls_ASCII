using System;


namespace PocketDarkSouls
{

    public class TalkCommand : Command
    {

        public TalkCommand() : base()
        {
            this.Name = "speak";
        }


        override
        public bool Execute(Player player)
        {
            
            
            int count_ = player.CurrentRoom.GetOccupancyCount(); 
            if (count_ < 2){
                player.messenger.InfoMessage("Talking to yourself is a sure sign of madness, these caverns are listening...", ConsoleColor.Yellow);
            }

            

            if (this.HasSecondWord())
            {
                if (this.HasThirdWord())
                {
                    Player p2 = player.CurrentRoom.FindPlayerInRoom(this.ThirdWord);
                    if (p2 != null)
                    {
                        if (p2.SpeakCommands.ContainsKey(this.SecondWord))
                        {
                            // execute the speak command for the NPC - hook up the speak command to the NPC and execute it here
                            p2.SpeakCommands[this.SecondWord].Execute(player,p2);
                        }
                        else
                        {
                            player.messenger.WarningMessage("\nI shouldnt bother them.. ", ConsoleColor.Yellow);
                        }
                    }
                }
                return false;
            }
            else
            {
                player.messenger.WarningMessage("\nSpeek to whom? Youself perhaps? ", ConsoleColor.Yellow);
            }
            return false;
        }
    }
}
