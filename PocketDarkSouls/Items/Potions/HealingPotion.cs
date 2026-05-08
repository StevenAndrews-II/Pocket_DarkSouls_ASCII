using PocketDarkSouls;

public class HealingPotion : Potion
{
    public HealingPotion(string id, int numberOf, double mass, double price, int modifier)
    {
        this.id         = id;
        this.price      = price;
        this.mass       = mass;
        this.numberOf   = numberOf;
        this.modifier   = modifier;
    }
    /// <summary>
    /// Hooks into the player's event manager and raises a HealEvent with the amount of healing determined by the modifier.
    /// The modifier is multiplied by the base amount (amt) to calculate the total healing effect of the potion.
    /// </summary>
    /// <param name="user">The player consuming the potion.</param>
    /// <param name="amt">The base amount of healing to apply.</param>
    protected override void Hook(Player user, int amt)
    {
        user.Events.Raise(new HealEvent(amt * modifier));
    }
    /// <summary>
    /// ToString provides a string representation of the HealingPotion, including its id, weight, price, healing modifier, and quantity.
    /// </summary>
    /// <returns>A string representation of the HealingPotion.</returns>
    public override string ToString()
    {
        return $"{id,-30} >> Wt:{mass,7:F2} lbs | ${price,8:F2}\n" +
               $"{"",-40}Healing:{modifier} | Qty:{numberOf}\n";
    }
}