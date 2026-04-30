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

    protected override void Hook(Player user, int amt)
    {
        user.EventManager.RaiseHeal(new HealEvent(amt * modifier));
    }

    public override string ToString()
    {
        return $"{id,-30} >> Wt:{mass,7:F2} lbs | ${price,8:F2}\n" +
               $"{"",-40}Healing:{modifier} | Qty:{numberOf}\n";
    }
}