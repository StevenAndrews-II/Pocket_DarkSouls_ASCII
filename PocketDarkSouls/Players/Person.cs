using PocketDarkSouls;

public class Person : Player
{
    public Person(
        string name,
        List<Speak> dialog,
        Inventory I_,
        EntityEvents events,
        Wallet W_,
        HealthSystem H_,
        Room room)
        : base(name, dialog, I_, events, W_, H_, room)
    {
    }

    //----------------------------------------------------------
    // AI variables
    //----------------------------------------------------------
    Random dice = new Random();
    List<string> dir_ = new List<string>()
    {
        "east","west","north","south" // characts stay on their floor and cant traverse (up and down)
    };


    //----------------------------------------------------------
    // Driver 
    //----------------------------------------------------------
    public override void AI()
    {
       Roaming();
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
