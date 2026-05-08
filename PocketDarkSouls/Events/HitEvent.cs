using PocketDarkSouls;
public sealed class HitEvent
{
    public int Damage { get; set; }

    public HitEvent(int damage)
    {
        Damage = damage;
    }
}