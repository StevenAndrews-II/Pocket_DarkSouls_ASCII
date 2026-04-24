using System;
using System.Collections.Generic;


public class ItemCreator
{

    Dictionary<string, Item> cache = new Dictionary<string, Item>();

    // WEAPONS

    private List<string> Swords_first_low = new List<string>()
    {
        "Broken","Cracked","Worn","Rusty","Dull",
        "Chipped","Bent","Cheap","Rough","Old",
        "Wooden","Makeshift","Splintered","Blunt",
        "Damaged","Fragile","Plain"
    };

    private List<string> Swords_first_mid = new List<string>()
    {
        "Iron","Steel","Balanced","Sharp","Fine",
        "Tempered","Weighted","Polished","Hardened",
        "Reliable","Soldiers","Mercenary",
        "Knights","Battle","Forged","Sturdy",
        "Reinforced","Etched"
    };

    private List<string> Swords_first_high = new List<string>()
    {
        "Ancient","Runed","Engraved","Masterwork",
        "Legendary","Mythic","Royal","Gilded",
        "Cursed","Blessed","Obsidian","Dragonforged",
        "Soulbound","Shadowed","Radiant","Eternal",
        "Doomed","Exalted","Divine","Void_touched"
    };
    // LAST NAMES
    private List<string> Swords_second = new List<string>()
    {
        "Fallen","Forsaken","Dawn","Dusk","Night","Ember",
        "Storm","Flame","Frost","Void","Abyss","Light",
        "Soul","Spirit","Edge","Blade","Reaver","Bite",
        "Breaker","Cleaver","Piercer","Rend","Whisper",
        "Oath","Honor","Wrath","Fury","Sorrow","Hope",
        "Rising","Last Stand","Echo","Shatter","Bane","Sever"
    };


    // ARMOR NAMES
    private List<string> Armor_first_low = new List<string>()
    {
        "Torn","Worn","Crude","Stitched","Patchwork",
        "Frayed","Weak","Thin","Dirty","Old",
        "Leather","Hide","Makeshift","Loose"
    };

    private List<string> Armor_first_mid = new List<string>()
    {
        "Chain","Scale","Iron","Steel","Reinforced",
        "Layered","Guards","Soldiers","Knights",
        "Battleworn","Sturdy","Fortified","Heavy"
    };

    private List<string> Armor_first_high = new List<string>()
    {
        "Ancient","Runed","Blessed","Cursed",
        "Royal","Gilded","Dragonscale","Mythic",
        "Divine","Eternal","Unyielding","Bulwark",
        "Titanforged","Voidforged"
    };
    // LAST NAMES
    private List<string> Armor_second = new List<string>()
    {
        "of_Protection","of_Fortitude","of_the_Bear","of_the_Wolf",
        "of_the_Iron_Wall","of_Thorns","of_Resolve","of_Stone",
        "of_the_Sentinel","of_Guarding","of_Defiance",
        "of_the_Fallen","of_the_Watcher","of_Endurance",
        "of_the_Bulwark","of_Shelter","of_the_Fortress"
    };

    private Random dice = new Random();



    public Item? Generate(string type,int rarity)
    {
        switch (type)
        {
            case "sword":
                return Weapon_Maker("Sword", rarity);
            case "helmet":
                return Armor_Maker("Helmet", rarity);
            case "chestplate":
                return Armor_Maker("ChestPlate", rarity );
            case "HP":
                return Potions_Maker("HP", rarity);
            default: return null;
        }
    }

    //-------------------------------------------------------------------------------------------------------------
    // potions maker 
    //-------------------------------------------------------------------------------------------------------------
    private Item? Potions_Maker(string type, int rarity)
    {
        if (rarity < 0 || rarity > 3)
        {
            throw new ArgumentException($"Rarity must be between 0 and 3. Provided value: {rarity}  ");
        }

        int special_roll = dice.Next(0, rarity);
        string first            = "";
        string second           = "";
        int first_name_roll     = 0;
        int second_name_roll    = 0;

        int health_roll         = 0;
        int magic_roll          = 0; 
        int fire_roll           = 0;


        double weight               = 0; 
        double price_roll           = 0;

        switch (special_roll)
        {
            case 0:
                first_name_roll         = dice.Next(0, Armor_first_low.Count);
                second_name_roll        = dice.Next(0, Armor_second.Count);
                first                   = Armor_first_low[first_name_roll];
                second                  = Armor_second[second_name_roll];


                health_roll             = dice.Next(5,10);
                weight                  = health_roll;

                price_roll              = dice.Next(25, 30) + dice.NextDouble();
                break;

            case 1:
                first_name_roll         = dice.Next(0, Armor_first_mid.Count);
                second_name_roll        = dice.Next(0, Armor_second.Count);
                first                   = Armor_first_mid[first_name_roll];
                second                  = Armor_second[second_name_roll];

                health_roll             = dice.Next(10, 15);
                weight                  = health_roll;

                price_roll              = dice.Next(30, 40) + dice.NextDouble();
                break;

            case 2:
                first_name_roll         = dice.Next(0, Armor_first_high.Count);
                second_name_roll        = dice.Next(0, Armor_second.Count);
                first                   = Armor_first_high[first_name_roll];
                second                  = Armor_second[second_name_roll];

                health_roll             = dice.Next(15, 35);
                weight                  = health_roll;

                price_roll              = dice.Next(40, 50) + dice.NextDouble();
                break;

        }



        string name = $"{type}_Elixer";
        Item _;
        switch (type)
        {
            case "HP":
                _ = new HealingPotion(name, 1, weight, price_roll, health_roll);
                update_cache(name, _);
                return _;
            default: return null;

        }

    }


    //-------------------------------------------------------------------------------------------------------------
    // weapons maker
    //-------------------------------------------------------------------------------------------------------------

    private Item Weapon_Maker(string type, int rarity)
    {
        if (rarity < 0 || rarity > 3)
        {
            throw new ArgumentException($"Rarity must be between 0 and 3. Provided value: {rarity}  ");
        }

        int special_roll = dice.Next(0, rarity);

        string first            = "";
        string second           = "";
        int first_name_roll     = 0;
        int second_name_roll    = 0;

        int physical_roll       = 0;
        int fire_roll           = 0;
        int magic_roll          = 0;
        int range_roll          = 0;

        double weight_roll      = 0;
        double price_roll       = 0;
        switch (special_roll)
        {
            case 0:
                first_name_roll         = dice.Next(0, Swords_first_low.Count);
                second_name_roll        = dice.Next(0, Swords_second.Count);
                first                   = Swords_first_low[first_name_roll];
                second                  = Swords_second[second_name_roll];

                range_roll              = dice.Next(5, 30);
                physical_roll           = dice.Next(5, 30);
                fire_roll               = dice.Next(5, 30);
                magic_roll              = dice.Next(5, 30);
                weight_roll             = dice.Next(8, 12) + dice.NextDouble();
                price_roll              = dice.Next(50, 199) + dice.NextDouble();
                break;

            case 1:
                first_name_roll         = dice.Next(0, Swords_first_mid.Count);
                second_name_roll        = dice.Next(0, Swords_second.Count);
                first                   = Swords_first_mid[first_name_roll];
                second                  = Swords_second[second_name_roll];

                range_roll              = dice.Next(5, 30);
                physical_roll           = dice.Next(30, 60);
                fire_roll               = dice.Next(30, 60);
                magic_roll              = dice.Next(30, 60);
                
                weight_roll             = dice.Next(30, 60) + dice.NextDouble();
                price_roll              = dice.Next(200, 600) + dice.NextDouble();
                break;

            case 2:
                first_name_roll         = dice.Next(0, Swords_first_high.Count);
                second_name_roll        = dice.Next(0, Swords_second.Count);
                first                   = Swords_first_high[first_name_roll];
                second                  = Swords_second[second_name_roll];

                range_roll              = dice.Next(5, 30);
                physical_roll           = dice.Next(60, 250);
                fire_roll               = dice.Next(60, 250);
                magic_roll              = dice.Next(60, 250);
                
                weight_roll             = dice.Next(10, 25) + dice.NextDouble();
                price_roll              = dice.Next(700, 2000) + dice.NextDouble();
                break;
        }

        string name = $"{first}_{second}_{type}";

        Item _;
        switch (type)
        {
            case "Sword":
                _ = new Sword(name, 1, weight_roll, price_roll, range_roll, physical_roll, fire_roll, magic_roll);
                update_cache(name, _);
                return _;
            case "Axe":
                _ = new Axe(name, 1, weight_roll, price_roll, range_roll, physical_roll, fire_roll , magic_roll);
                update_cache(name, _);
                return _;
            default: return null;

        }
    }
    //-------------------------------------------------------------------------------------------------------------
    // Armor maker
    //-------------------------------------------------------------------------------------------------------------
    private Item? Armor_Maker(string middle, int rarity)
    {
        if (rarity < 0 || rarity > 3)
        {
            throw new ArgumentException($"Rarity must be between 0 and 3. Provided value: {rarity}");
        }

        int special_roll = dice.Next(0, rarity);

        string first                = "";
        string second               = "";
        int first_name_roll         = 0;
        int second_name_roll        = 0;

        int fire_roll               = 0;
        int magic_roll              = 0;
        int physical_roll           = 0;

        double weight_roll          = 0;
        double price_roll           = 0;
        switch (special_roll)
        {
            case 0:
                first_name_roll         = dice.Next(0, Armor_first_low.Count);
                second_name_roll        = dice.Next(0, Armor_second.Count);
                first                   = Armor_first_low[first_name_roll];
                second                  = Armor_second[second_name_roll];


                fire_roll               = dice.Next(0, 200);
                magic_roll              = dice.Next(0, 200);
                physical_roll           = dice.Next(0, 200); // 0 to 999 on these stats 

                weight_roll             = dice.Next(2, 5) + dice.NextDouble();
                price_roll              = dice.Next(50, 199) + dice.NextDouble();
                break;

            case 1:
                first_name_roll         = dice.Next(0, Armor_first_mid.Count);
                second_name_roll        = dice.Next(0, Armor_second.Count);
                first                   = Armor_first_mid[first_name_roll];
                second                  = Armor_second[second_name_roll];

                fire_roll               = dice.Next(200, 500);
                magic_roll              = dice.Next(200, 500);
                physical_roll           = dice.Next(200, 500); // 0 to 999 on these stats 

                weight_roll             = dice.Next(5, 7) + dice.NextDouble();
                price_roll              = dice.Next(200, 499) + dice.NextDouble();
                break;

            case 2:
                first_name_roll         = dice.Next(0, Armor_first_high.Count);
                second_name_roll        = dice.Next(0, Armor_second.Count);
                first                   = Armor_first_high[first_name_roll];
                second                  = Armor_second[second_name_roll];


                fire_roll               = dice.Next(500, 999);
                magic_roll              = dice.Next(500, 999);
                physical_roll           = dice.Next(500, 999); // 0 to 999 on these stats 

                weight_roll             = dice.Next(7, 8) + dice.NextDouble();
                price_roll              = dice.Next(499, 1000) + dice.NextDouble();
                break;
        }

        string name = $"{first}_{middle}_{second}";
        Item _;
        switch (middle)
        {
            case "Helmet":
                _ = new Helmet(name, 1, weight_roll, price_roll, physical_roll, fire_roll, magic_roll);
                update_cache(name, _);
                return _;
            case "ChestPlate":
                _ = new ChestPlate(name, 1, weight_roll, price_roll, physical_roll, fire_roll, magic_roll);
                update_cache(name, _);
                return _;
            default: return null;

        }

    }


    private void update_cache (string name, Item _){
        if (!cache.ContainsKey(name))
        {
            cache.TryAdd(name, _);
        }
        else
        {
            cache[name].numberOf += 1;
        }
    }

}
