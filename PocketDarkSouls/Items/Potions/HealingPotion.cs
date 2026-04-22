

public class HealingPotion : Potion
{

    public HealingPotion(string id, int numberOf, double mass, double price, int healing)
    {
        base.id         = id;
        base.price      = price;
        base.mass       = mass;
        base.numberOf   = numberOf;
        base.healing    = healing;
    }

    public override void Hook(HealthSystem HP, int amt)
    {
        if (HP.isAlive()){
            HP.reginerate(amt*this.healing);
        }
    }
    public override string ToString()
    {
        string price = $"{this.id,-30} >> " +
                      $"Wt:{this.mass,7:F2} lbs | ${this.price,8:F2}\n";
        string stats = $"{"",-40}Healing:{this.healing}\n";

        return price+ stats;
    }
}
