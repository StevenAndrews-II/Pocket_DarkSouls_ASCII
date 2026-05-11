using System;

namespace PocketDarkSouls
{
    public class ScavengeCommand : Command
    {
        public ScavengeCommand() : base()
        {
            this.Name = "scavenge";
        }

        public override bool Execute(Player player)
        {
            player.ScavengeCurrentRoom();
            return false;
        }
    }
}