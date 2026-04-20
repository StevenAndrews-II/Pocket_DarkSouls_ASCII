
public class Helmet : Armor
{

    public int stat { get; init; } = 25;


    public Helmet(string id, int numberOf, double mass, double price, int psyical ,int fire , int magic )
    {
        base.id                     = id;
        base.price                  = price;
        base.mass                   = mass;
        base.numberOf               = numberOf;
       
        base.magic_protection       = magic;
        base.fire_protection        = fire;
        base.physical_protection    = psyical;
    }

    public override string ToString()
    {
        string price = $"{this.id,-30} >> " +
                      $"Wt:{this.mass,7:F2} lbs | ${this.price,8:F2}\n";
        string stats = $"   {"",-40}    PHY:{this.physical_protection,5} | MAG:{this.magic_protection,5} | FIR:{this.fire_protection}\n";

        return price+ stats;
    }
}
