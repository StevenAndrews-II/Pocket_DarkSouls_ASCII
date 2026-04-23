
public abstract class Potion : Item
{
    public int modifier { get; init; }
    public virtual void Hook(EntityEvents HealthEvent, int amt)
    {
    }
}