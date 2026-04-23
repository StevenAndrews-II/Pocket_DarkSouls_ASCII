public class HealingPotion : Potion, IConsumable
{
    public HealingPotion(string id, int numberOf, double mass, double price, int modifier)
    {
        this.id = id;
        this.price = price;
        this.mass = mass;
        this.numberOf = numberOf;
        this.modifier = modifier;
    }

 
    public new void Consume(EntityEvents events, int amount)
    {
        this.Hook(events, amount);
    }

    public override void Hook(EntityEvents events, int amt)
    {
        events.RaiseHeal(amt * modifier);
    }

    public override string ToString()
    {
        return $"{id,-30} >> Wt:{mass,7:F2} lbs | ${price,8:F2}\n" +
               $"{"",-40}Healing:{modifier} | Qty:{numberOf}\n";
    }
}
