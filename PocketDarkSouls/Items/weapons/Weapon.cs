
public abstract class Weapon : Item
{
    public int damage { get; init; }

    public int chance { get; init; }
    public int fire_damage { get; init; }
    public int magic_damage { get; init; }
    public int physical_damage { get; init; }
    
    public int effective_range { get; init; }

    public virtual void Hook(HealthSystem HP) // Enemy health system - maybe switch to event based here 
    {

    }
}