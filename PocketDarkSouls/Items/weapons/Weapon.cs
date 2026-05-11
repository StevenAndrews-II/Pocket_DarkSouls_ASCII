
using PocketDarkSouls;

public abstract class Weapon : Item
{
    public int damage { get; init; }

    public int chance { get; init; }
    public int fire_damage { get; init; }
    public int magic_damage { get; init; }
    public int physical_damage { get; init; }
    
    public int effective_range { get; init; }

    public virtual void Hook(EntityEvents events) // Enemy health system - maybe switch to event based here 
    {

    }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }
        Weapon other = (Weapon)obj;

        return
            id == other.id &&
            mass == other.mass &&
            physical_damage == other.physical_damage &&
            fire_damage == other.fire_damage &&
            magic_damage == other.magic_damage &&
            effective_range == other.effective_range;
    }

}