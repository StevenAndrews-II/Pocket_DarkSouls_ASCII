using PocketDarkSouls;

public class DialogHandler
{

	private readonly Player player_ref;
	private  Random rand = new Random();
    public DialogHandler(Player player)
	{
		player_ref = player;
    }


	public string GenericSpeach( Speak cmd )
	{
		if (cmd.Dialog.ContainsKey("generic"))
		{
			int roll = rand.Next(0, cmd.Dialog["generic"].Count);
            //player_ref.messenger.ReplyMessage($"{cmd.Dialog["generic"][roll]}");
            return $"{cmd.Dialog["generic"][roll]}";

        }
        return "...";

	}


    public string TradeSpeach(Speak cmd)
    {
        if (cmd.Dialog.ContainsKey("trade"))
        {
            int roll = rand.Next(0, cmd.Dialog["trade"].Count);
            //player_ref.messenger.ReplyMessage($"{cmd.Dialog["trade"][roll]}");
            return $"{cmd.Dialog["trade"][roll]}";

        }
        return "...";
    }


    public string ThankYouSpeach(Speak cmd)
    {
        if (cmd.Dialog.ContainsKey("thanks"))
        {
            int roll = rand.Next(0, cmd.Dialog["thanks"].Count);
            //player_ref.messenger.ReplyMessage($"{cmd.Dialog["thanks"][roll]}");
            return $"{cmd.Dialog["thanks"][roll]}";

        }
        return "...";
    }

    public string NothingToTradeSpeach(Speak cmd)
    {
        if (cmd.Dialog.ContainsKey("notrade"))
        {
            int roll = rand.Next(0, cmd.Dialog["notrade"].Count);
            //player_ref.messenger.ReplyMessage($"{cmd.Dialog["notrade"][roll]}");

            return $"{cmd.Dialog["notrade"][roll]}";

        }
        return "...";
    }


    public string NotEnoughToTrade(Speak cmd)
    {
        if (cmd.Dialog.ContainsKey("notenough"))
        {
            int roll = rand.Next(0, cmd.Dialog["notenough"].Count);
            //player_ref.messenger.ReplyMessage($"{cmd.Dialog["notenough"][roll]}");
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
            //player_ref.messenger.ReplyMessage($"{cmd.Dialog[select][roll]}");
            return $"{cmd.Dialog[select][roll]}";

        }
        return "...";
    }



    public string QuitTrade(Speak cmd)
    {
        if (cmd.Dialog.ContainsKey("quittrade"))
        {
            int roll = rand.Next(0, cmd.Dialog["quittrade"].Count);
            //player_ref.messenger.ReplyMessage($"{cmd.Dialog["quittrade"][roll]}");
            return $"{cmd.Dialog["quittrade"][roll]}";

        }
        return "...";
    }



}
