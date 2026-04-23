

public class HealingPotion : Potion
{

    public HealingPotion(string id, int numberOf, double mass, double price, int modifier)
    {
        base.id = id;
        base.price = price;
        base.mass = mass;
        base.numberOf = numberOf;
        base.modifier = modifier;

    }

    public override void Hook(EntityEvents HealthEvents , int amt)
    {
        HealthEvents.RaiseHeal(amt*modifier);
    }
    

    
    public override string ToString()
    {
        string price = $"{this.id,-30} >> " +
                      $"Wt:{this.mass,7:F2} lbs | ${this.price,8:F2}\n";
        string stats = $"{"",-40}Healing:{this.modifier}\n";

        return price+ stats;
    }
}
