using PocketDarkSouls;
using System;
using System.Collections.Generic;

public class CharacterCreator
{ 

    List<string> maleMinerNames = new List<string>
{
    "Thorin", "Doran", "Thrain", "Oin", "Bofur", "Dain", "Koth", "Sindri", "Fafnir", "Mimir",
    "Nordri", "Grendel", "Sigurd", "Odin", "Forseti", "Fenris", "Magne", "Moin", "Skuld", "Urd",
    "Bael", "Hagen", "Balin", "Ori", "Bombur", "Nain", "Magni", "Alvis", "Otur", "Galar",
    "Sudri", "Beowulf", "Hagred", "Vidar", "Njord", "Surtr", "Svafnir", "Graf", "Verd", "Mani",
    "Krag", "Vorn", "Gloin", "Dori", "Fili", "Modi", "Onar", "Hreid", "Fjalar", "Gandalf",
    "Vali", "Freyr", "Ofnir", "Grabak", "Vid", "Ur", "Ver", "Mani", "Gorm", "Durin",
    "Nori", "Kili", "Ryser", "Brokk", "Ivaldi", "Austri", "Gram", "Unferth", "Hermod", "Ymir",
    "Ulf", "Bram", "Fundin", "Bifur", "Dwalin", "Dax", "Eitri", "Regin", "Alberic", "Vestri",
    "Thror", "Hrothgar", "Tyr", "Hodur", "Baldur", "Thrud", "Goin", "Sol", "Kael", "Regin","Hagard"
};

    List<string> femaleMinerNames = new List<string>
{
    "Thora", "Lyra", "Freya", "Gerda", "Fulla", "Lofn", "Beyla", "Day", "Bestla", "Signy",
    "Hild", "Rota", "Skogul", "Mist", "Atla", "Jarn", "Fenja", "Heid", "Laufey", "Nal",
    "Sigrid", "Petra", "Idunn", "Ran", "Saga", "Var", "Bil", "Earth", "Borghild", "Sigrun",
    "Gondul", "Radgrid", "Herja", "Angeyja", "Greip", "Menja", "Rind", "Farbauti", "Vesper", "Etta",
    "Sif", "Hel", "Eir", "Vor", "Sol", "Jord", "Brynhild", "Svava", "Skogul", "Goll",
    "Eistla", "Gjalp", "Hyndla", "Skadi", "Baugi", "Helga", "Juno", "Frigg", "Nanna", "Hlin",
    "Syn", "Mani", "Rind", "Gudrun", "Olrun", "Hlokk", "Urd", "Alvitr", "Eyrgjafa", "Gerd",
    "Mara", "Zora", "Skadi", "Gefjun", "Gna", "Snotra", "Night", "Gunnlod", "Kriemhild", "Hervor",
    "Mist", "Verdandi", "Kara", "Grimhild", "Imd", "Bestla", "Gullveig", "Grid", "Hrim", "Suttung"

};

    List<string> minerLastNames = new List<string>
{
    "Aethros", "Vornhal", "Draeven", "Kaelith", "Morvain", "Tharros", "Velkyn", "Zereth", "Orvane", "Nyxar",
    "Helior", "Vaelun", "Rathen", "Solvyr", "Mythren", "Auron", "Valeth", "Dravyn", "Karneth", "Zorvain",
    "Eldros", "Thyrian", "Varkul", "Orren", "Maleth", "Caedryn", "Vorun", "Nerath", "Zareth", "Ulthar",
    "Baelros", "Virel", "Thalor", "Morren", "Avarn", "Kelvyr", "Dorneth", "Yorath", "Vaelor", "Orthex",
    "Zevran", "Karthos", "Velor", "Nyreth", "Arveth", "Droven", "Malrith", "Thorne", "Voss", "Graven",
    "Harrow", "Duskryn", "Veylor", "Ashryn", "Morveth", "Krevos", "Zalthor", "Orvain", "Velmorn", "Threx",
    "Drathen", "Vaul", "Korveth", "Nyros", "Zerath", "Aldren", "Vorath", "Kelros", "Thyros", "Moros",
    "Varn", "Eryndor", "Ulric", "Draeth", "Kaelor", "Vireth", "Zorin", "Malvor", "Ordrin", "Thalryn",
    "Vaelros", "Krynn", "Zerros", "Morvain", "Aethryn", "Velros", "Draemor", "Nyvar", "Korren", "Zalthyn"

};


    private int idcount                             = 0;
    private Dictionary<string, Player> players      = new Dictionary<string, Player>();
    private Random rand                             = new Random();
    private DialogCreator dialogCreator             = new DialogCreator();
    private readonly ItemCreator itemCreator;
    public CharacterCreator(ItemCreator itemCreator)
    {
        this.itemCreator = itemCreator;
    }





    /// <summary>
    /// Updates all players in the game. This should be called once per game loop iteration to ensure that all player states are updated correctly.
    /// Removes any non-hero players that have died from the players dictionary.
    /// This allows for dynamic changes to the game world as players interact with it and face challenges.
    /// </summary>
    public void update()
    {
        foreach (var (k,v) in players)
        {
            if (v.health.isAlive())
            {
                v.update();
            }
            else
            {
                if (v is not Hero)
                {
                    RemovePlayer(k);
                }
            }
        }
    }


    /// <summary>
    /// Removes a player from the players dictionary based on their name. This can be used to remove NPCs that have died or are no longer needed in the game world.
    /// </summary>
    /// <param name="name"></param>
    public void RemovePlayer(string name)
    {
        if (players.ContainsKey(name))
        {
            players.Remove(name);
        }
    }

    /// <summary>
    /// Clears the players dictionary and resets the id count to 0. This can be used to reset the game state when starting a new game or when the player dies and needs to respawn.
    /// </summary>
    public void RemoveAllplayers()
    {
        idcount = 0;
        players.Clear();
    }



    /// <summary>
    /// Creates a random player character with a unique name, randomly generated inventory, and appropriate command sets based on the specified type.
    /// The character is then added to the players dictionary and returned.
    /// This method can be used to create both user-controlled characters and AI-controlled NPCs, allowing for a diverse range of characters in the game world.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="type"></param>
    /// <returns></returns>
    public Player createRandomPerson(string? name = null, string? type = "user") // this is about 9,801(x2) per gender /  19,602 total combinations  99x99
    {

        // name generator - loops until met
        int counter = 0;
        while (name == null)
        {
            counter++; // timeout counter
            idcount++; // index of all players 
            List<string> selected_gender;
            int gender = rand.Next(0, 1);
            int name_roll;
            int lastName_roll;

            if (gender == 0)
            {
                selected_gender = maleMinerNames;
                name_roll = rand.Next(0, maleMinerNames.Count);
            }
            else
            {
                selected_gender = femaleMinerNames;
                name_roll = rand.Next(0, femaleMinerNames.Count);
            }
            lastName_roll = rand.Next(0, minerLastNames.Count);
            string temp_name = $"{selected_gender[name_roll]}_{minerLastNames[lastName_roll]}";
            if (!players.ContainsKey(temp_name))
            {
                name = temp_name;
                continue;
            }
            else
            {
                if (counter > 5) // time out for generator
                {
                    name = $"{temp_name}_{idcount}";
                    break;
                }
            }
        }


        // DI inject health system,and other systems here 

        EntityEvents Events             = new EntityEvents();
        Wallet wallet                   = new Wallet(600, 100000); // make random for final presentation
        HealthSystem health             = new HealthSystem(Events);

        Inventory main_inventory        = new Inventory(wallet, health); // pass wallet and health - potions / stims / loot packs add buffs to sub systems after use...


        // bind user only commands
        List<Speak>  dialogCommands     = dialogCreator.MakeDialogSet(type);
        //List<Combat> dialogCombat       = dialogCreator.MakeDialogSet_combat(type);


        Player character;
        switch (type)
        {
            // only one of these per fame ( is the user )
            // this allows for some advanced character to character interactiosn
            case "hero":

                character = new Hero(
                                                        name,               // Character name 
                                                        dialogCommands,     // custom dialog prompt hooking 
                                                        main_inventory,     // internal system 
                                                        Events,
                                                        wallet,             // internal system 
                                                        health,             // internal system

                                                        null                // current room / spawn room ( null at first - assigned by SpawnWarp() ) 
                                                     );
                Loot(character,1);
                break;
                

            case "merchant":

               character = new Merchant(
                                                        name,               // Character name 
                                                        dialogCommands,     // custom dialog prompt hooking 
                                                        main_inventory,     // internal system 
                                                        Events,
                                                        wallet,             // internal system 
                                                        health,             // internal system

                                                        null                // current room / spawn room ( null at first - assigned by SpawnWarp() ) 
                                                     );
                Loot(character,3);
                break;
            case "beggar":
                character = new Beggar(
                                                        name,               // Character name 
                                                        dialogCommands,     // custom dialog prompt hooking 
                                                        main_inventory,     // internal system 
                                                        Events,
                                                        wallet,             // internal system 
                                                        health,             // internal system

                                                        null                // current room / spawn room ( null at first - assigned by SpawnWarp() ) 
                                                     );
                break;
            case "drunk":
                character = new Drunk(
                                                        name,               // Character name 
                                                        dialogCommands,     // custom dialog prompt hooking 
                                                        main_inventory,     // internal system 
                                                        Events,
                                                        wallet,             // internal system 
                                                        health,             // internal system

                                                        null                // current room / spawn room ( null at first - assigned by SpawnWarp() ) 
                                                     );
                break;
            case "person":
                character = new Person(
                                                        name,               // Character name 
                                                        dialogCommands,     // custom dialog prompt hooking 
                                                        main_inventory,     // internal system 
                                                        Events,
                                                        wallet,             // internal system 
                                                        health,             // internal system

                                                        null                // current room / spawn room ( null at first - assigned by SpawnWarp() ) 
                                                     );
                Loot(character,1);
                break;
            case "goblin":
                character = new Goblin(
                                                        name,               // Character name 
                                                        dialogCommands,     // custom dialog prompt hooking 
                                                        main_inventory,     // internal system 
                                                        Events,
                                                        wallet,             // internal system 
                                                        health,             // internal system

                                                        null                // current room / spawn room ( null at first - assigned by SpawnWarp() ) 
                                                     );
                Loot(character,2);
                break;
            default:
                character = new Person(
                                                       name,               // Character name
                                                       dialogCommands,     // custom dialog prompt hooking 
                                                       main_inventory,     // internal system 
                                                       Events,
                                                       wallet,             // internal system 
                                                       health,             // internal system

                                                       null                // current room / spawn room ( null at first - assigned by SpawnWarp() ) 
                                                    );
                break;
        }

       
        
        players.Add(name, character); // spawn after creation 
        return character;
    }

    /// <summary>
    /// Creates a random number of loot items and adds them to the players inventory, then randomly equips some of them. 
    /// This is called on character creation for certain types, but could be used for any character at any time ( including merchants ) to add some randomization to the game.
    /// Could also be used for random loot drops from enemies 
    /// </summary>
    /// <param name="p">The player to receive the loot.</param>
    private void Loot(Player p,int rarity = -1)
    {
        int numberOf_roll   = rand.Next(0, 6);
        int type_roll       = 0;
        Item? _;

        for (int i = 0; i < numberOf_roll; i++)
        {
            type_roll = rand.Next(0, 4);
            string type = "";
            switch (type_roll)
            {
                case 0:
                    type = "sword";
                    
                    break;
                case 1:
                    type = "helmet";
                    break;
                case 2:
                    type = "chestplate";
                    break;
                case 3:
                    type = "HP";
                    break;
                default: 
                    type = "sword";
                    break;
            }

            if (rarity < 0  || rarity > 3)
            {
                rarity = rand.Next(0, 4);
            }   


            _ = itemCreator.Generate(type, rarity);
            if (_!=null) {
                p.main_inventory.AddItem(_);
            }
        }
        p.main_inventory.RandomEquip();
    }

}