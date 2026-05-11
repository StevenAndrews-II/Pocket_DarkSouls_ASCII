using System.Collections;
using System.Collections.Generic;
using System;
using System.Diagnostics;

namespace PocketDarkSouls
{
    public class BackCommand : Command
    {
        public BackCommand()
        {
            Name = "back";
        }

        public override bool Execute(Player player)
        {
            player.GoBack();
            return false;
        }
    }
}