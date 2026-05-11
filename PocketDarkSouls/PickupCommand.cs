using System;

namespace PocketDarkSouls
{
    public class PickupCommand : Command
    {
        public PickupCommand() : base()
        {
            this.Name = "pickup";
        }

        public override bool Execute(Player player)
        {
            if (!this.HasSecondWord()) 
            {
                player.Messenger.ErrorMessage("Pickup what?", ConsoleColor.Red);
                return false; 
            }

            player.PickupItem(this.SecondWord); 
            return false;
        }
    }
}