
public class Bow : Weapon
{


    public Bow(string id, int numberOf, double mass, double price, int range, int physical, int fire, int magic)
    {
        base.id                 = id;
        base.price              = price;
        base.mass               = mass;
        base.numberOf           = numberOf;

        base.physical_damage    = physical;
        base.fire_damage        = fire;
        base.magic_damage       = magic;
        base.effective_range    = range;

    }


    public override void Hook(HealthSystem HP)
    {
        // add effects 
        // hook into HP
        // maybe switch to event manager here tho
        // use enemy event manager then send it a hit packet 
    }


    public override string ToString()
    {
        string price = $"{this.id,-30} >> " +
                      $"Wt:{this.mass,7:F2} lbs | ${this.price,8:F2}\n";
        string stats = $"{"",-40}RNG:{this.effective_range,5} | PHY:{this.physical_damage,5} | MAG:{this.magic_damage,5} | FIR:{this.fire_damage}\n";

        return price + stats;
    }
}
