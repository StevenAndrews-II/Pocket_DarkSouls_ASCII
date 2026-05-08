using System;
using System.Collections.Generic;
using PocketDarkSouls;
public abstract class Armor : Item
{
    public int physical_protection { get; init; }
    public int magic_protection { get; init; }
    public int fire_protection { get; init; }

    public override bool Equals(object? obj)
    {
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }
        Armor other = (Armor)obj;

        return
            id == other.id &&
            mass == other.mass &&
            numberOf == other.numberOf &&
            physical_protection == other.physical_protection &&
            magic_protection == other.magic_protection &&
            fire_protection == other.fire_protection;
        }
}