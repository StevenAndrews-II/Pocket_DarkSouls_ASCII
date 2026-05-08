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
                player.Messenger.InfoMessage("Talking to yourself is a sure sign of madness, these caverns are listening...", ConsoleColor.Yellow);
            }

            

            if (this.HasSecondWord())
            {
                if (this.HasThirdWord())
                {
                    Player? p2 = player.CurrentRoom.FindPlayerInRoom(this.ThirdWord);
                    if (p2 != null)
                    {

                        Speak? cmd = p2.LookUpSpeakCommand(this.SecondWord);

                        if (cmd != null) {
                            cmd.Execute(player, p2);
                        }
                        else
                        {
                            player.Messenger.WarningMessage("\nI shouldnt bother them.. ", ConsoleColor.Yellow);
                        }
                    }
                }
                return false;
            }
            else
            {
                player.Messenger.WarningMessage("\nSpeek to whom? Youself perhaps? ", ConsoleColor.Yellow);
            }
            return false;
        }
    }
}
