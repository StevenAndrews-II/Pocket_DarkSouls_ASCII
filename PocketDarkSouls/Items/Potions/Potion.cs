
public abstract class Potion : Item , IConsumable
{
    public int modifier { get; init; }

    /// <summary>
    /// Consumes a specified amount from the given entity events.
    /// </summary>
    /// <param name="events">The entity events to consume from.</param>
    /// <param name="amount">The amount to consume.</param>
    public void Consume(EntityEvents events, int amount) {
    }
    /// <summary>
    /// Hooks into the event system to apply the potion's effect. This method is intended to be overridden by derived classes to implement specific potion effects.
    /// The base implementation does nothing, allowing derived classes to define their own behavior when consumed.
    /// </summary>
    /// <param name="events"></param>
    /// <param name="amt"></param>
    public virtual void Hook(EntityEvents events, int amt)
    {
    }
}