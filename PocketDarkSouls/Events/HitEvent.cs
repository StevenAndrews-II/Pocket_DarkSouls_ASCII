using PocketDarkSouls;
public sealed class HitEvent
{
    public int Amount { get; set; }

    public HitEvent(int damage)
    {
        Amount  = damage;
    }
}