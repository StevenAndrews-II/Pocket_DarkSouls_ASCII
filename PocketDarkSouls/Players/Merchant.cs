using PocketDarkSouls;


public class Merchant : Player
{
    public Merchant(
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
        "east","west","north","south"
    };


    //----------------------------------------------------------
    // Driver 
    //----------------------------------------------------------
    public override void AI()
    {
        InventoryFlipping();
        Roaming();
    }


    //----------------------------------------------------------
    // plugin functions 
    //----------------------------------------------------------

    private void InventoryFlipping()
    {
        // get 5 items ( if we have em, and list them for sale 
        Dictionary<string, Item> forsale = Inventory.getAllItemsMarkedForSale();

        if (forsale.Count == 0)
        {
            Inventory.FindAndMarkItemsToSell(2,dice.Next(5,10));
        }
    }

    private void Roaming()
    {
        // need to add a check to make sure youre not in a conversation / dialog - add time out of like 5 loops 
        // move around at random - random wait
        int roll        = dice.Next(0, dir_.Count);
        int motion_roll = dice.Next(0, 5);
        if (motion_roll == 1)
        {
            GoTo(dir_[roll]);
        }
    }


}
