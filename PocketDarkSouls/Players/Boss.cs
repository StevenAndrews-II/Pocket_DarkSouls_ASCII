using PocketDarkSouls;

public class Boss : Player
{
    public Boss(
        string name,
        List<Speak> dialog,
        EntityEvents events,
        Room room)
        : base(name, dialog, events, room)
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
        Combat();
    }
    private void Combat()
    {
        // to do brp - add a check to make sure youre not in a conversation / dialog 
    }
}
