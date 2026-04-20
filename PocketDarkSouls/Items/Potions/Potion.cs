
public abstract class Potion : Item
{
    public int healing { get; init; }
    public virtual void Hook(HealthSystem HP, int amt)
    {
    }
}