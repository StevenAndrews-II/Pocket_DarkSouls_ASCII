using PocketDarkSouls;

public class Beggar : Player
{
    public Beggar(
        string name,
        List<Speak> dialog,
        Inventory I_,
        List<ICs> InventoryCommands,
        Wallet W_,
        HealthSystem H_,
        Room room)
        : base(name, dialog, I_, InventoryCommands, W_, H_, room)
    {
    }


    //----------------------------------------------------------
    // AI variables
    //----------------------------------------------------------
    Random dice = new Random();
    List<string> dir_ = new List<string>()
    {
        "east","west","north","south"
    };


    //----------------------------------------------------------
    // Driver 
    //----------------------------------------------------------
    public override void AI()
    {
        if (health.isAlive())
        {
            Roaming();
        }
    }
    private void Roaming()
    {
        // need to add a check to make sure youre not in a conversation / dialog - add time out of like 5 loops 
        // move around at random - random wait
        int roll = dice.Next(0, dir_.Count);
        int motion_roll = dice.Next(0, 5);
        if (motion_roll == 1)
        {
            goTo(dir_[roll]);
        }
    }
}
