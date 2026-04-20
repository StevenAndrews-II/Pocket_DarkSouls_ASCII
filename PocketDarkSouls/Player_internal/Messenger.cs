using PocketDarkSouls;


public class Messenger
{
    private readonly Player p;

    public Messenger(Player p) {
        this.p = p;
    }



    //List<string> out_ = new List<string>();  // idea   
    private Dictionary<string, ConsoleColor> out_ = new Dictionary<string, ConsoleColor>();
    public Dictionary<string, ConsoleColor> Get()
    {
        return out_;
    }

    public void Clear()
    {
        out_.Clear();
    }

    public void draw()
    {
        foreach (var (k, v) in out_)
        {
            ColoredMessage(k, v);
        }
    }

    public void OutputMessage(string message)
    {
        Console.WriteLine(message);
    }

    public void ColoredMessage(string message, ConsoleColor newColor)
    {
        ConsoleColor oldColor = Console.ForegroundColor;
        Console.ForegroundColor = newColor;
        OutputMessage(message);
        Console.ForegroundColor = oldColor;
    }



    public void ErrorMessage(string message, ConsoleColor select)
    {
        out_.TryAdd("[Error] : " + message, select);
    }

    public void WarningMessage(string message, ConsoleColor select)
    {
        out_.TryAdd("[Warning] : " + message, select);
    }

    public void InfoMessage(string message, ConsoleColor select)
    {
        out_.TryAdd("[Info] : " + message, select);
    }

    public void NormalMessage(string message, ConsoleColor select)
    {
        out_.TryAdd(message, select);
    }

    public void SendMessage(string message, ConsoleColor select)
    {

        out_.TryAdd($"[{p.name}] : " + message, select);

    }

    public void ReciveMessage(string name_,string message, ConsoleColor select)
    {
        out_.TryAdd($"[{name_}] : " +message, select);
    }

    private bool Near_menu(ConsoleColor select)
    {
        string nearby = p.CurrentRoom.GetNearByPlayers(p.name);
        if (nearby != "")
        {
            string _    = "\n - - - - - - - - - - - - - - - - - - - - - - - - - -   " +
                         $"\n{nearby}" +
                          "\n - - - - - - - - - - - - - - - - - - - - - - - - - -   ";
            out_.TryAdd(_, select);
            return true;
        }
        return false;
    }

    private void PlayerStats(ConsoleColor select, bool top = true, bool bottom = true)
    {
        string line = "\n - - - - - - - - - - - - - - - - - - - - - - - - - -   ";
        string _ = "";
        if (top){
            _ = line;
        }
        _ += $"\n{p.main_inventory.getInfo()}" +
            $"\nHP:{p.health.GetHealthStatus()} | Gold:{p.wallet.GetGoldInWallet()}";
        if (bottom)
        {
            _ += line;
        }
        out_.TryAdd(_, select);
    }

    public void display_menu(ConsoleColor near, ConsoleColor stats)
    {
        bool NM_check = p.messenger.Near_menu(near);
        if (NM_check)
        {
            p.messenger.PlayerStats(stats, false);
        }
        else
        {
            p.messenger.PlayerStats(stats);
        }
    }
}
