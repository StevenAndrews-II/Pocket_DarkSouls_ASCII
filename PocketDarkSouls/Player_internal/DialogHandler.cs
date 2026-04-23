using PocketDarkSouls;

public class DialogHandler
{

	private  Random rand = new Random();

    //---------------------------------------------------------------------------------------------------------
    // Combat interactions 
    //---------------------------------------------------------------------------------------------------------

    public string CombatEngageSpeach(Speak cmd)
    {
        if (cmd.Dialog.ContainsKey("engage"))
        {
            int roll = rand.Next(0, cmd.Dialog["engage"].Count);
            return $"{cmd.Dialog["engage"][roll]}";

        }
        return "...";

    }

    public string CombatAttackSpeach(Speak cmd)
    {
        if (cmd.Dialog.ContainsKey("attack"))
        {
            int roll = rand.Next(0, cmd.Dialog["attack"].Count);
            return $"{cmd.Dialog["attack"][roll]}";

        }
        return "...";

    }

    public string CombatTauntSpeach(Speak cmd)
    {
        if (cmd.Dialog.ContainsKey("taunt"))
        {
            int roll = rand.Next(0, cmd.Dialog["taunt"].Count);
            return $"{cmd.Dialog["taunt"][roll]}";

        }
        return "...";

    }

    public string CombatLowHPSpeach(Speak cmd)
    {
        if (cmd.Dialog.ContainsKey("low_health"))
        {
            int roll = rand.Next(0, cmd.Dialog["low_health"].Count);
            return $"{cmd.Dialog["low_health"][roll]}";

        }
        return "...";

    }

    public string CombatDeathSpeach(Speak cmd)
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


    public string GenericSpeach( Speak cmd )
	{
		if (cmd.Dialog.ContainsKey("generic"))
		{
			int roll = rand.Next(0, cmd.Dialog["generic"].Count);
            return $"{cmd.Dialog["generic"][roll]}";

        }
        return "...";

	}

    public string TradeSpeach(Speak cmd)
    {
        if (cmd.Dialog.ContainsKey("trade"))
        {
            int roll = rand.Next(0, cmd.Dialog["trade"].Count);
            return $"{cmd.Dialog["trade"][roll]}";

        }
        return "...";
    }


    public string ThankYouSpeach(Speak cmd)
    {
        if (cmd.Dialog.ContainsKey("thanks"))
        {
            int roll = rand.Next(0, cmd.Dialog["thanks"].Count);
            return $"{cmd.Dialog["thanks"][roll]}";

        }
        return "...";
    }

    public string NothingToTradeSpeach(Speak cmd)
    {
        if (cmd.Dialog.ContainsKey("notrade"))
        {
            int roll = rand.Next(0, cmd.Dialog["notrade"].Count);
            return $"{cmd.Dialog["notrade"][roll]}";

        }
        return "...";
    }


    public string NotEnoughToTrade(Speak cmd)
    {
        if (cmd.Dialog.ContainsKey("notenough"))
        {
            int roll = rand.Next(0, cmd.Dialog["notenough"].Count);
            return $"{cmd.Dialog["notenough"][roll]}";

        }
        return "...";
    }

    public string BadCharity(Speak cmd, int level)
    {
        string select = "badcharity_neutral";
        if (level == 1) {
            select = "badcharity_hostile";
        }
        if (cmd.Dialog.ContainsKey(select))
        {
            int roll = rand.Next(0, cmd.Dialog[select].Count);
            return $"{cmd.Dialog[select][roll]}";

        }
        return "...";
    }

    public string QuitTrade(Speak cmd)
    {
        if (cmd.Dialog.ContainsKey("quittrade"))
        {
            int roll = rand.Next(0, cmd.Dialog["quittrade"].Count);
            return $"{cmd.Dialog["quittrade"][roll]}";

        }
        return "...";
    }



}// EOF
