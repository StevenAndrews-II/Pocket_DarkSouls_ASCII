
public abstract class Potion : Item , IConsumable
{
    public int modifier { get; init; }

    public void Consume(EntityEvents events, int amount) {
    }

    public virtual void Hook(EntityEvents HealthEvent, int amt)
    {
    }
}