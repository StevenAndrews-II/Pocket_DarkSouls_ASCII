using PocketDarkSouls;
public class HitEvent
{
    public int Amount { get; init; }
    public Player Source { get; init; }
    public Player Target { get; init; }
    public string DamageType { get; init; } = "physical"; // optional
    public bool IsCritical { get; init; } = false;
}

