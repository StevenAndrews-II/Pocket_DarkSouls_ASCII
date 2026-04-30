
using PocketDarkSouls;

public abstract class Potion : Item, IConsumable
{
    public int modifier;

    public void Consume(Player user, int amount)
    {
        if (amount <= 0)
        {
            Console.WriteLine("Invalid amount.");
            return;
        }

        // ⚠️ NO quantity handling here (inventory does that)

        Hook(user, amount);
    }

    protected abstract void Hook(Player user, int amount);
}