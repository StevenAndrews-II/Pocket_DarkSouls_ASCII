using PocketDarkSouls;

public class DialogHandler
{

	private  Random rand = new Random();

    //---------------------------------------------------------------------------------------------------------
    // Combat interactions 
    //---------------------------------------------------------------------------------------------------------

    public string CombatEngageSpeach(Combat cmd)
    {
        if (cmd.Dialog.ContainsKey("engage"))
        {
            int roll = rand.Next(0, cmd.Dialog["engage"].Count);
            return $"{cmd.Dialog["engage"][roll]}";

        }
        return "...";

    }

    public string CombatAttackSpeach(Combat cmd)
    {
        if (cmd.Dialog.ContainsKey("attack"))
        {
            int roll = rand.Next(0, cmd.Dialog["attack"].Count);
            return $"{cmd.Dialog["attack"][roll]}";

        }
        return "...";

    }

    public string CombatTauntSpeach(Combat cmd)
    {
        if (cmd.Dialog.ContainsKey("taunt"))
        {
            int roll = rand.Next(0, cmd.Dialog["taunt"].Count);
            return $"{cmd.Dialog["taunt"][roll]}";

        }
        return "...";

    }

    public string CombatLowHPSpeach(Combat cmd)
    {
        if (cmd.Dialog.ContainsKey("low_health"))
        {
            int roll = rand.Next(0, cmd.Dialog["low_health"].Count);
            return $"{cmd.Dialog["low_health"][roll]}";

        }
        return "...";

    }

    public string CombatDeathSpeach(Combat cmd)
    {
        if (cmd.Dialog.ContainsKey("death"))
        {
            int roll = rand.Next(0, cmd.Dialog["death"].Count);
            return $"{cmd.Dialog["death"][roll]}";

        }
        return "...";

    }

    //---------------------------------------------------------------------------------------------------------
    // Generic speak non combat interactions
    //---------------------------------------------------------------------------------------------------------


    public string GenericSpeach(Dictionary<string, List<string>> cmd )
	{
		if (cmd.ContainsKey("generic"))
		{
			int roll = rand.Next(0, cmd["generic"].Count);
            return $"{cmd["generic"][roll]}";
        }
        return "...";

	}

    public string TradeSpeach(Dictionary<string, List<string>> cmd)
    {
        if (cmd.ContainsKey("trade"))
        {
            int roll = rand.Next(0, cmd["trade"].Count);
            return $"{cmd["trade"][roll]}";

        }
        return "...";
    }


    public string ThankYouSpeach(Dictionary<string, List<string>> cmd)
    {
        if (cmd.ContainsKey("thanks"))
        {
            int roll = rand.Next(0, cmd["thanks"].Count);
            return $"{cmd["thanks"][roll]}";

        }
        return "...";
    }

    public string NothingToTradeSpeach(Dictionary<string, List<string>> cmd)
    {
        if (cmd.ContainsKey("notrade"))
        {
            int roll = rand.Next(0, cmd["notrade"].Count);
            return $"{cmd["notrade"][roll]}";

        }
        return "...";
    }


    public string NotEnoughToTrade(Dictionary<string, List<string>> cmd)
    {
        if (cmd.ContainsKey("notenough"))
        {
            int roll = rand.Next(0, cmd["notenough"].Count);
            return $"{cmd["notenough"][roll]}";

        }
        return "...";
    }

    public string BadCharity(Dictionary<string, List<string>> cmd, int level)
    {
        string select = "badcharity_neutral";
        if (level == 1) {
            select = "badcharity_hostile";
        }
        if (cmd.ContainsKey(select))
        {
            int roll = rand.Next(0, cmd[select].Count);
            return $"{cmd[select][roll]}";

        }
        return "...";
    }

    public string QuitTrade(Dictionary<string, List<string>> cmd)
    {
        if (cmd.ContainsKey("quittrade"))
        {
            int roll = rand.Next(0, cmd["quittrade"].Count);
            return $"{cmd["quittrade"][roll]}";

        }
        return "...";
    }



}// EOF
