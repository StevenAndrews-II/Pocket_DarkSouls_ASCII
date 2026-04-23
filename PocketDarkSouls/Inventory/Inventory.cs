using PocketDarkSouls;
using System;

public class Inventory 
{

    private Dictionary<string, Item> backpack   = new Dictionary<string, Item>();    // full inventory  
    private Dictionary<string, Item> equipment  = new Dictionary<string, Item>();    // equipment slots - weight based
    private Dictionary<string, Item> armor      = new Dictionary<string, Item>();    // armor slots - slot based
    private Dictionary<string, Item> forSale    = new Dictionary<string, Item>();    // items marked for sale - weight based

    Dictionary<string, bool> sections = new Dictionary<string, bool>                 // section slots for armor
    {
        ["head"] = false,
        ["body"] = false,
    };

    private int Capacity                = 20;       // capacity cap
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


    private readonly Wallet         wallet;
    private readonly HealthSystem   HP;



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


    /// <summary>
    /// Randomly equips items from the backpack to the appropriate equipment or armor slots based on their types.
    /// </summary>
    public void RandomEquip()
    {
        foreach (var (k, v) in backpack)
        {
            if (v is Weapon || v is Helmet || v is ChestPlate)
            {
                Equip(k);
            }
        }
    }
    /// <summary>
    /// Equips an item from the backpack to the appropriate equipment or armor slot based on the specified item ID.
    /// </summary>
    /// <param name="id">The ID of the item to equip.</param>
    /// <returns>True if the item was successfully equipped; otherwise, false.</returns>
    public bool Equip(string id)
    {
        if (backpack.ContainsKey(id))
        {
            if (backpack[id] is Weapon ) // any number of weapons can be equiped / weight based - select when fighting 
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

    /// <summary>
    /// Unequips an item from the equipment or armor slots based on the specified item ID.
    /// This method checks if the specified item ID exists in either the equipment or armor dictionaries.
    /// </summary>
    /// <param name="id">The ID of the item to unequip.</param>
    /// <returns>True if the item was successfully unequipped; otherwise, false.</returns>
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
    /// <summary>
    /// Gets the total weight of all items in the inventory, including the backpack, equipment, armor, and items marked for sale.
    /// </summary>
    /// <returns>The total weight of all items in the inventory.</returns>
    public double getTotalWeight()
    {
        return backpack_lbs + equipment_lbs + armor_lbs + forSale_lbs;
    }

    /// <summary>
    /// Gets a list of all items currently marked for sale in the inventory.
    /// This method iterates through the forSale dictionary and collects all the items into a list, which is then returned to the caller.
    /// </summary>
    /// <returns>A list of items currently marked for sale.</returns>
    public List<Item> getAllItemsMarkedForSale()
    {
        List<Item> __ = new List<Item>();
        foreach (var (k, v) in forSale)
        {
           __.Add(v);
        }
       return __;
    }

    /// <summary>
    /// Adds a specified amount of an item to the for sale bag based on the item ID and the amount to mark for sale.
    /// This method checks if the specified item ID exists in the backpack
    /// </summary>
    /// <param name="id">The ID of the item to add to the for sale bag.</param>
    /// <param name="amt">The amount of the item to add to the for sale bag.</param>
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
            // not in the bag 
            else
            {
                // add item to the bag and set the inital amount for the bag 
                if (backpack[id].numberOf > amt_)
                {
                    forSale.Add(id, backpack[id]);
                    forSale[id].numberOf    = amt;
                    forSale_lbs             = forSale[id].numberOf * forSale[id].mass;// update mass
                }
                else // remove entirely and directly plug it into the bag 
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
    /// <summary>
    /// finds and marks a specified amount of items to sell based on the specified item ID and the number of items to mark for sale.
    /// </summary>
    /// <param name="amt"></param>
    /// <param name="numberOf"></param>
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

    /// <summary>
    /// gets an item from the for sale bag based on the specified item ID.
    /// This method checks if the specified item ID exists in the for sale bag
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Item? getForSaleItem(string id)
    {

        if (forSale.ContainsKey(id))
        {
            return forSale[id];
        }
        return null;
    }

    /// <summary>
    /// gets an item from the inventory based on the specified item ID. This method checks if the specified item ID exists in the backpack
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Item? getItem(string id)
    {
        if (backpack.ContainsKey(id))
        {
            return backpack[id];
        }
        return null;
    }
    /// <summary>
    /// gets a string representation of an item from the inventory based on the specified item ID.
    /// This method checks if the specified item ID exists in the backpack
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public string getItemInfo(string id)
    {
        if (backpack.ContainsKey(id))
        {
            return backpack[id].ToString();
        }
        return "";
    }

    /// <summary>
    /// gets a health potion from the inventory based on the specified item ID.
    /// This method checks if the specified item ID exists in the backpack
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Gets any consumable item from the inventory, such as health potions, mana potions, 
    /// or other items that can be consumed for various effects. This method checks if the specified item ID exists in the backpack
    /// and if it is of type IConsumable. If both conditions are met, it returns the consumable item; otherwise, it returns null.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public IConsumable? GetConsumables(string id)
    {
        if (backpack.ContainsKey(id))
        {
            if (backpack[id] is IConsumable)
            {
                return (IConsumable)backpack[id];
            }
        }
        return null;
    }


    /// <summary>
    /// Uses a specified amount of a consumable item from the inventory. This method checks if the specified item ID exists in the backpack
    /// and if it is of type IConsumable. If both conditions are met, it then checks if the quantity of the item in the backpack is
    /// sufficient to consume the specified amount. If so, it calls the Consume method on the consumable item, passing in the HealthEvents
    /// and the amount to consume, and then removes the consumed amount from the inventory.
    /// The method returns true if the item was successfully consumed, and false otherwise.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="amt"></param>
    /// <returns></returns>
    public bool useItem( string id , int amt)
    {
        if (amt <= 0) return false;
        IConsumable? consumable = GetHealthPotion(id);
        if (consumable != null)
        {
            if (backpack[id].numberOf >= amt ) {
                consumable.Consume(HP.HealthEvents, amt);
                DelItem(id, amt);
                return true;
            }
        }
        return false;
    }


    //-------------------------------------------------------------------------------------------------------
    // Inventory managment
    //-------------------------------------------------------------------------------------------------------

    /// <summary>
    /// Adds an item to the inventory.
    /// This method checks if adding the specified item would exceed the backpack's weight limit and capacity limit.
    /// </summary>
    /// <param name="item">The item to add to the inventory.</param>
    /// <returns>True if the item was successfully added; otherwise, false.</returns>
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


    /// <summary>
    /// Deletes a specified amount of an item from the inventory based on the item ID.
    /// This method checks if the specified item ID exists in the backpack
    /// and if the quantity of the item in the backpack is sufficient to delete the specified amount.
    /// If so, it removes the specified amount from the inventory.
    /// The method returns the actual amount of items deleted.
    /// </summary>
    /// <param name="id">The ID of the item to delete.</param>
    /// <param name="ammount">The amount of the item to delete.</param>
    /// <returns>The actual amount of items deleted.</returns>
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

    /// <summary>
    /// Gets a string representation of the inventory's current weight status, including the total weight of all items in the inventory,
    /// equipment, and armor.
    /// </summary>
    /// <returns>A string representing the current weight status of the inventory.</returns>
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
                            $"{"",-46}PHY: {item.physical_protection,5} | FIR: {item.fire_protection,5} | MGK: {item.magic_protection,5}\n";
                }
            }
            if (equipment.Count > 0)
            {
                tmp += "[ Equipment ]------------------------------------------------------------------\n";
                foreach (Weapon item in equipment.Values)
                {

                    tmp +=
                        $"{item.id,-45} >>>   #: {item.numberOf,3}  | Weight: {(item.mass * item.numberOf),8:F2} lbs\n" +
                        $"{"",-46}PHY: {item.physical_damage,5} | FIR: {item.fire_damage,5} | MGK: {item.magic_damage,5}\n";

                }
            }
            if (backpack.Count > 0)
            {
                tmp += "[ Inventory ]------------------------------------------------------------------\n";

                foreach (var item in backpack.Values)
                {
                    tmp += $"{item.id,-46} >>> #: {item.numberOf,3} [Weight: {(item.mass * item.numberOf),8:F2}] lbs\n";
                }
            }
            return tmp;
        }
        return "Your inventory is empty...";
    }


}
