
using PocketDarkSouls;

public abstract class Potion : Item, IConsumable
{
    public int modifier;
    /// <summary>
    /// Consumes the potion, applying its effects to the player. The specific effects will depend on the type of potion and the implementation of the Hook method.
    /// </summary>
    /// <param name="user">The player consuming the potion.</param>
    /// <param name="amount">The amount of the potion to consume.</param>
    public void Consume(Player user, int amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Invalid amount.");
            return;
        }

        Hook(user, amount);
    }
    /// <summary>
    /// Hooks into the player's stats and modifies them based on the potion's effect. The specific implementation will depend on the type of potion and the desired effect.
    /// </summary>
    /// <param name="user">The player consuming the potion.</param>
    /// <param name="amount">The amount of the potion to consume.</param>
    protected abstract void Hook(Player user, int amount);
}