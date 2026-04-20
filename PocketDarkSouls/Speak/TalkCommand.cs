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
            
            
            int count_ = player.CurrentRoom.GetOccupancyCount(); // defently a hack lol
            if (count_ < 2){
                player.messenger.InfoMessage("Talking to yourself is a sure sign of madness, these caverns are listening...", ConsoleColor.Yellow);
            }

            

            if (this.HasSecondWord())
            {
                // push to exicute here and - push third word down 
                // this is the speak sub command ( trade, to, and fight ) 
                if (this.HasThirdWord())
                {
                    Player p2 = player.CurrentRoom.FindPlayerInRoom(this.ThirdWord);
                    if (p2 != null)
                    {
                        if (p2.SpeakCommands.ContainsKey(this.SecondWord))
                        {
                            
                            p2.SpeakCommands[this.SecondWord].Execute(player,p2);
                        }
                        else
                        {
                            player.messenger.WarningMessage("\nI shouldnt bother asking them.. ", ConsoleColor.Yellow);
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
