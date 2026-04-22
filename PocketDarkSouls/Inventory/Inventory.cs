using PocketDarkSouls;
using System;

public class Inventory : IInventory
{

    private Dictionary<string, Item> backpack = new Dictionary<string, Item>();   
    private Dictionary<string, Item> equipment = new Dictionary<string, Item>();
    private Dictionary<string, Item> armor = new Dictionary<string, Item>();
    private Dictionary<string, Item> forSale = new Dictionary<string, Item>();  // get rid of flag in weapon 

    private int Capacity                = 20;      // capacity cap
    private int Capacity_onboard        = 0;


    private double backpack_lbs         = 0.00;
    private double backpack_cap         = 75.00;    // backpack weight limit

 
    private double equipment_lbs        = 0;
    private double equipment_cap        = 25;       // 100 pound carry limit

    private double armor_lbs            = 0;
    private double armor_cap            = 25;

    private double forSale_lbs          = 0;



    private double MAX_backpack_lbs     = 150;
    private double MAX_equipment_lbs    = 75;
    private double MAX_armor_lbs        = 75;




    // ABSOLUTE LIMITS - boosters cap 
    private const int    LIMIT_forSale      = 10;
    private const double LIMIT_backpack     = 300;
    private const double LIMIT_armor        = 70;
    private const double LIMIT_equipment    = 70;


    private readonly Wallet wallet;
    private readonly HealthSystem HP;

    // ake this today
    private struct inject {
        Wallet wallet;
        HealthSystem HP;
    }


    public Inventory(Wallet wallet,HealthSystem HP,double backpack_cap = 75 ,double equipment_cap = 25,double armor_cap = 25)
    {
        this.wallet         = wallet;
        this.HP             = HP;
        this.backpack_cap   = backpack_cap;
        this.equipment_cap  = equipment_cap;
        this.armor_cap      = armor_cap;
    }




    //-------------------------------------------------------------------------------------------------------
    // Settings for weight
    //-------------------------------------------------------------------------------------------------------


    private bool RaiseInventoryWeightCap(int ammount)
    {
        if (backpack_cap + ammount <= LIMIT_backpack)
        {
            backpack_cap += ammount;
            return true;
        }
        return false;
    }

    private bool RaiseEquipmentWeightCap(int ammount)
    {
        if (equipment_cap + ammount <= LIMIT_equipment)
        {
            equipment_cap += ammount;
            return true;
        }
        return false;

    }

    private bool RaiseArmorWeightCap(int ammount)
    {
        if (armor_cap + ammount <= LIMIT_armor)
        {
            armor_cap += ammount;
            return true;
        }
        return false;

    }




    //-------------------------------------------------------------------------------------------------------
    // Equipment attachment
    //-------------------------------------------------------------------------------------------------------





    Dictionary<string, bool> sections = new Dictionary<string, bool>
    {
        ["head"]    = false,
        ["body"]    = false,
    };

    public bool Equip(string id)
    {
        if (backpack.ContainsKey(id))
        {
            if (backpack[id] is Weapon ) // any number of weapons can be equiped / weight based
            {
                if (!equipment.ContainsKey(backpack[id].id))
                { 
                    if (equipment_lbs + backpack[id].mass <= equipment_cap)
                    {
                        equipment_lbs += backpack[id].mass;
                        equipment.Add(id, backpack[id]);
                        DelItem(id, 1);
                        return true;
                    }
                }
            }

            if (backpack[id] is Helmet && sections["head"] == false)     // slot 1
            {
                if (!armor.ContainsKey(backpack[id].id))
                {
                    if (armor_lbs + backpack[id].mass <= armor_cap)
                    {
                        armor_lbs += backpack[id].mass;
                        armor.Add(id, backpack[id]);
                        //HP.UpdateDefenseStats();
                        DelItem(id, 1);
                        sections["head"] = true;
                        return true;
                    }
                }
            }

            if (backpack[id] is ChestPlate && sections["body"] == false)  // slot 2 
            {
                if (!armor.ContainsKey(backpack[id].id))
                {
                    if (armor_lbs + backpack[id].mass <= armor_cap)
                    {
                        armor_lbs += backpack[id].mass;
                        armor.Add(id, backpack[id]);
                        DelItem(id, 1);
                        sections["body"] = true;
                        return true;
                    }
                }
            }
        }
        return false;
    }


    public bool Unequip(string id)
    {
        if (equipment.ContainsKey(id))
        {
            equipment_lbs -= equipment[id].mass;
            Item tmp = equipment[id];
            equipment.Remove(id);
            AddItem(tmp);
            return true;

        }
        if (armor.ContainsKey(id))
        {

            
            if (armor[id] is Helmet && sections["head"] == true)
            {
                sections["head"] = false;
            }
            if (armor[id] is ChestPlate && sections["body"] == true)
            {
                sections["body"] = false;

            }

            armor_lbs -= armor[id].mass;
            Item tmp = armor[id];
            armor.Remove(id);
            AddItem(tmp);
            return true;

        }
        return false;
    }


    //-------------------------------------------------------------------------------------------------------
    // Sell, trade, use items 
    //-------------------------------------------------------------------------------------------------------

    public double getTotalWeight()
    {
        return backpack_lbs + equipment_lbs + armor_lbs + forSale_lbs;
    }

    public List<Item> getAllItemsMarkedForSale()
    {
        List<Item> __ = new List<Item>();
        foreach (var (k, v) in forSale)
        {
           __.Add(v);
        }
       return __;
    }


    public void AddItemToForSale(string id, int amt)
    {
        int amt_ = Math.Abs(amt); // user guard
        if (backpack.ContainsKey(id))
        {
            // in the bag
            if (forSale.ContainsKey(id))
            {
                // still has some left in inventory
                if (backpack[id].numberOf > amt_)
                {
                    backpack[id].numberOf   -= amt_;    // remove from inventory 
                    forSale[id].numberOf    += amt_;    // add to forsale bag 

                    forSale_lbs             += forSale[id].numberOf * forSale[id].mass; // update mass
                }
                // remove the rest from inventory + add the rest to bag
                else
                {
                    forSale[id].numberOf    += backpack[id].numberOf;
                    forSale_lbs             += forSale[id].numberOf * forSale[id].mass; // update mass
                    backpack.Remove(id);
                }
            }
            // not in athe bag 
            else
            {
                // add to the bag and set the inital amount for the bag 
                if (backpack[id].numberOf > amt_)
                {
                    forSale.Add(id, backpack[id]);
                    forSale[id].numberOf    = amt;
                    forSale_lbs             = forSale[id].numberOf * forSale[id].mass;// update mass
                }
                else // remove entirely and dirently plug it into the bag 
                {
                    forSale.Add(id, backpack[id]);
                    backpack.Remove(id);
                    forSale_lbs             = forSale[id].numberOf * forSale[id].mass; // update mass
                }
            }
        }
    }

    //-------------------------------------------------------------------------------------------------------
    // Locate
    //-------------------------------------------------------------------------------------------------------

    public void FindAndMarkItemsToSell(int amt, int numberOf) 
    {
        int count = 0;
        foreach ( var (k,v) in backpack) 
        {
            if (count < amt)
            {
                AddItemToForSale(k, numberOf);
            }
            else
            {
                break;
            }
        }
    }

    public Item? getForSaleItem(string id)
    {

        if (forSale.ContainsKey(id))
        {
            return forSale[id];
        }
        return null;
    }


    public Item? getItem(string id)
    {
        if (backpack.ContainsKey(id))
        {
            return backpack[id];
        }
        return null;
    }

    public string getItemInfo(string id)
    {
        if (backpack.ContainsKey(id))
        {
            return backpack[id].ToString();
        }
        return null;
    }


    public Potion? GetHealthPotion(string id)
    {
        if (backpack.ContainsKey(id))
        {
            if (backpack[id] is Potion)
            {
                return (Potion)backpack[id];
            }
        }
        return null;
    }


    public bool useItem( string id , int amt)
    {
        if (amt <= 0) return false;
        Potion healthPotion = GetHealthPotion(id);
        if (healthPotion!=null)
        {
            healthPotion.Hook(HP,amt);
            int amt_ = DelItem(id, amt);
            return true;
        }
        return false;
    }


    //-------------------------------------------------------------------------------------------------------
    // Inventory managment
    //-------------------------------------------------------------------------------------------------------


    public bool AddItem(Item item)
    {
        double item_mass = (item.numberOf * item.mass);

        if ((item_mass + getTotalWeight()) <= backpack_cap)
        {
            if (Capacity_onboard + item.numberOf <= Capacity)
            {
                if (backpack.ContainsKey(item.id))
                {
                    Capacity_onboard            += item.numberOf;    // capacity
                    backpack_lbs                += item_mass;        // add up mass
                    backpack[item.id].numberOf  += item.numberOf;    // add
                    return true;
                }
                else
                {
                    backpack_lbs += item_mass;      // add up mass
                    backpack.Add(item.id, item);
                    return true;
                }
            }
        }
        return false;
    }



    public int DelItem(string id, int ammount)
    {
        if (!backpack.ContainsKey(id))
        {
            return 0;
        }

        if (ammount < backpack[id].numberOf)
        {
            Capacity_onboard -= ammount;
            backpack_lbs -= (ammount * backpack[id].mass);
            backpack[id].numberOf -= ammount;
            return ammount;
        }
        else
        {
            int temp_actualAmmount = 0;
            temp_actualAmmount = backpack[id].numberOf;
            Capacity_onboard -= backpack[id].numberOf;
            backpack_lbs -= (backpack[id].numberOf) * backpack[id].mass;
            backpack.Remove(id);
            return temp_actualAmmount;
        }
    }


    public string getInfo()
    {
        return $"Inventory total : [{getTotalWeight():F2}]lbs ||  Equipment : [{equipment_lbs:F2}]lbs || Armor : [{armor_lbs:F2}]lbs";
    }

    public string ReadInventory()
    {
        if (backpack.Count != 0)
        {
            string tmp = "\n";
            if (armor.Count > 0)
            {
                tmp += "[ Armor ]---------------------------------------------------------------------\n";
                foreach (Armor item in armor.Values)
                {
                    tmp +=
                            $"{item.id,-45} >>>   #: {item.numberOf,3}  | Weight: {(item.mass * item.numberOf),8:F2} lbs\n" +
                            $"{"",-45}PHY: {item.physical_protection,5} | FIR: {item.fire_protection,5} | MGK: {item.magic_protection,5}\n";
                }
            }
            if (equipment.Count > 0)
            {
                tmp += "[ Equioment ]------------------------------------------------------------------\n";
                foreach (Weapon item in equipment.Values)
                {

                    tmp +=
                        $"{item.id,-45} >>>   #: {item.numberOf,3}  | Weight: {(item.mass * item.numberOf),8:F2} lbs\n" +
                        $"{"",-45}PHY: {item.physical_damage,5} | FIR: {item.fire_damage,5} | MGK: {item.magic_damage,5}\n";

                }
            }
            if (backpack.Count > 0)
            {
                tmp += "[ Inventory ]------------------------------------------------------------------\n";

                foreach (var item in backpack.Values)
                {
                    tmp += $"{item.id,-45} >>> #: {item.numberOf,3} [Weight: {(item.mass * item.numberOf),8:F2}] lbs\n";
                }
            }
            return tmp;
        }
        return "Your inventory is empty...";
    }


}
