using PocketDarkSouls;
public sealed class HealEvent
{
    public int Amount { get; set; }

    public HealEvent(int amount)
    {
        Amount = amount;
    }
}
