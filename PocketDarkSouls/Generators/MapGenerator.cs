using PocketDarkSouls;


public class MapGenerator
{
    private Dictionary<int, Dictionary<int, Room>> boss_rooms_cache             = new Dictionary<int, Dictionary<int, Room>>();            // segmented - could move to deep dictionary 
    private Dictionary<int, Dictionary<int, Room>> towns_rooms_cache            = new Dictionary<int, Dictionary<int, Room>>();
    private Dictionary<int, Dictionary<int, Room>> mine_rooms_cache             = new Dictionary<int, Dictionary<int, Room>>();
    private Dictionary<int, Dictionary<int, Room>> artifact_rooms_cache         = new Dictionary<int, Dictionary<int, Room>>();
    private Dictionary<int, Dictionary<int, Room>> trap_rooms_cache             = new Dictionary<int, Dictionary<int, Room>>();
    private Dictionary<int, Dictionary<int, Room>> Unstable_mine_rooms_cache    = new Dictionary<int, Dictionary<int, Room>>();
    



    private Dictionary<int, Dictionary<int,Room>> rooms_cache;                  // all rooms 

    private readonly ItemCreator Item_Creator;

    private Random rand = new Random();
    private string[] directions = { "north", "south", "east", "west" };
    CharacterCreator Character_Creator;
    private int LEVELS = 10;
  

    private double town_spawn_congestion = 0.3;



    public MapGenerator(CharacterCreator Character_Creator,ItemCreator Item_Creator) { 
    
        this.Character_Creator      = Character_Creator;
        this.Item_Creator           = Item_Creator;

    }


    

    // populates the map with NPCs - driver - Everything except bosses
    public void PopulateNPCs(int population)
    {
        int town_population_distrobution = 0;

        for (int i = 0;i < population; i++)
        {
            
            // roll for type 
            int roll = rand.Next(0, 4);
            string type = "person";             // travelers
            switch (roll)
            {
                case 1: type    = "beggar";     // go out side of towns 
                        break;
                case 2: type    = "merchant";   // go to towns 
                        break;
                case 3:
                    type        = "drunk";      // go to towns / taverns
                    break;
            }

            Room? spawn_location    = GetNPCspawnLocations(type); // will spawn anywhere on the map that isnt a boss room 
            Player NPC              = Character_Creator.createRandomPerson(null, type);
            NPC.SpawnWarp(spawn_location);
        }
    }


    // gets all NPC locations by rolling for spots - controller 
    private Room? GetNPCspawnLocations(string NPC_type)
    {
        int roll = rand.Next(0, 1);
        switch (NPC_type)
        {
            case "person":
                if (roll == 0)
                {
                    RollForTowns();
                }
                return RollForTownOutskits();
                
            case "beggar":
                if (roll == 0)
                {
                    RollForMines();
                }
                return RollForTownOutskits();

            case "merchant":
                return RollForTowns();

            case "drunk":
                return RollForTowns();

        } return RollForTownOutskits();
    }


    // roll for a random mine location 
    public Room RollForMines()
    {
        List<Room> mines = new List<Room>();
        for (int i = 0; i < LEVELS; i++)
        {
            mines.AddRange(GetMinesAtLevel(i));
        }
        int roll = rand.Next(0, mines.Count);
        return mines[roll];
    }


    // roll for a random town location 
    public Room RollForTowns()
    {
        List<Room> towns = new List<Room>();
        for (int i = 0; i < LEVELS; i++)
        {
            towns.AddRange(GetTownsAtLevel(i));
        }
        int roll = rand.Next(0, towns.Count);
        return towns[roll];
    }


    // roll for the outskits fir a town
    public Room RollForTownOutskits()
    {
        // get outkirts of all towns 
        List<Room> outskirts = new List<Room>();
        for (int i = 0; i < LEVELS; i++)
        {
            List<Room> towns_on_level = GetTownsAtLevel(i);
            for (int town_index = 0; town_index < towns_on_level.Count; town_index++)
            {
                Room? found = GetTownsOutSkirts(towns_on_level[town_index]);
                if (found != null)
                {
                    outskirts.Add(found);
                }
            }
        }
        int roll = rand.Next(0, outskirts.Count);

        return outskirts[roll];
    }


    // get the outskits locations 
    public Room GetTownsOutSkirts(Room town)
    {
        List<Room> outskirts_ = town.GetExitsRoomList();
        for (int i = 0; i < outskirts_.Count; i++)
        {
            if (outskirts_[i].type == "mine")
            {
                return outskirts_[i];
            }
        }
        return null;
    }


    public List<Room> GetTownsAtLevel(int level)
    {
        List<Room> list = new List<Room>();
        foreach (var(k,v) in towns_rooms_cache[level])
        {
           list.Add(v);
        }
        return list;
    }

    public List<Room> GetMinesAtLevel(int level)
    {
        List<Room> list = new List<Room>();
        foreach (var (k, v) in mine_rooms_cache[level])
        {
            list.Add(v);
        }
        return list;
    }

    // Huge random underground maze geberator - very hard to find the way out 
    // randomly links rooms, some rooms fold back in ways that can not be described on a flat map 
    // could add a Z component to track you height in the dungon



    public Room Generate(int levels = 10, int sprawl = 40,int NPCnumber = 200)
    {
        LEVELS          = levels; // update the levels number 
        rooms_cache     = new Dictionary<int, Dictionary<int,Room>>();

        for (int levels_ = 0; levels_ < levels; levels_++)
        {
            rooms_cache[levels_]                    = new Dictionary<int, Room>();
            boss_rooms_cache[levels_]               = new Dictionary<int, Room>();
            towns_rooms_cache[levels_]              = new Dictionary<int, Room>();
            mine_rooms_cache[levels_]               = new Dictionary<int, Room>();
            artifact_rooms_cache[levels_]           = new Dictionary<int, Room>();
            trap_rooms_cache[levels_]               = new Dictionary<int, Room>();
            Unstable_mine_rooms_cache[levels_]      = new Dictionary<int, Room>();

            for (int sprawl_ = 0; sprawl_ < sprawl; sprawl_++)
            {
                string key = $"level{levels_} : area{sprawl_}";
                string type = TypeDiceRoll(); // roll for a type 

                Dictionary<string, Action> actions_ = BindActions(type);
                rooms_cache[levels_][sprawl_] = new Room(
                                            key,            // location referenct - for display
                                            "in",
                                            type,           // rolled by dice roll 
                                            actions_        // pass activities here
                                            );

                cache_rooms(type, levels_,sprawl_);         // cache locations in groups 

            }

            // chain link rooms
            for (int sprawl_ = 0; sprawl_ < sprawl-1; sprawl_++)
            {
                Room a = rooms_cache[levels_][sprawl_];
                Room b = rooms_cache[levels_][sprawl_+1];

                string dir          = GetRandomDirection();
                string opposite     = GetOpposite(dir);

                a.SetExit(dir, b);
                b.SetExit(opposite, a);
            }

    
            // add random connections 
            for (int i = 0; i < sprawl; i++)
            {
                Room a = GetRandomRoomByLevel(levels_);
                Room b = GetRandomRoomByLevel(levels_);

                if (a == b) continue;

                string dir      = GetRandomDirection();
                string opposite = GetOpposite(dir);

                a.SetExit(dir, b);
                b.SetExit(opposite, a);
            }

            // set up rooms // path to next level 
            if (levels_ >= 1)
            {
                Room a = GetRandomRoomByLevel(levels_ - 1);     // lower room 
                Room b = GetRandomRoomByLevel(levels_);         // upper room

                a.SetExit("up", b);
                b.SetExit("down", a);
            }


        }
        PopulateNPCs(NPCnumber);
        return rooms_cache[0][0]; 
    }



    private Dictionary<string,Action> BindActions(string type)
    {

        Dictionary<string, Action>  actions_  = new Dictionary<string, Action>(); 

        if (type == "mine")
        {
            actions_.Add("mining",new MineAction());
            // search - returns things in the room like objects
            // other stuff here
        }
        return actions_;
    }

  



    private string TypeDiceRoll()
    {
        Random dice = new Random();
        double roll = 0;
        int count = 0;
        
        // roll a few times and get average 
        for (; count < 5; count++)
        {
            roll  += dice.NextDouble(); 
        }
        roll = roll / count;              // average roll 


        string type = "mine";           // defualt 
        if (roll >= .4 && roll < .5)    // 10% roll     // make this set actions to rooms
        {
            type = "town";
        }

        if (roll >= .5 && roll < .55)   // 5% roll
        {
            type = "Unstable_mine";           
        }

        if (roll >= .55 && roll < .56)  // 1% roll 
        {
            type = "trap"; // teleporter
        }

        if (roll >= .56 && roll < .57)  // 1% roll 
        {
            type = "artifact";
        }

        if (roll >= .57 && roll < .59)  // 2% roll 
        {
            type = "boss";
        }


        return type;
    }




    // skips boss rooms 
    public Room GetRandomRoomByLevel(int level)
    {
        int index = rand.Next(rooms_cache[level].Count);
        foreach (var room in rooms_cache[level].Values)
        {
           
                if (index-- == 0)
                    return room;
           
        }
        return null;
    }



    private string GetRandomDirection()
    {
        return directions[rand.Next(directions.Length)];
    }



    private string GetOpposite(string dir)
    {
        switch (dir)
        {
            case "north": return "south";
            case "south": return "north";
            case "east": return "west";
            case "west": return "east";
            default: return "north";
        }
    }

    private void cache_rooms(string type, int lvls, int sprwl)
    {
        // Get the room from the main cache first to avoid repeating the lookup
        var roomToCache = rooms_cache[lvls][sprwl];
        switch (type)
        {
            case "boss": boss_rooms_cache[lvls][sprwl]                    = roomToCache; break;
            case "town": towns_rooms_cache[lvls][sprwl]                   = roomToCache; break;
            case "mine": mine_rooms_cache[lvls][sprwl]                    = roomToCache; break;
            case "unstable_mine": Unstable_mine_rooms_cache[lvls][sprwl]  = roomToCache; break;
            case "trap": trap_rooms_cache[lvls][sprwl]                    = roomToCache; break;
            case "artifact": artifact_rooms_cache[lvls][sprwl]            = roomToCache; break;
        }
    }

    public Room GetRoom(int level, int room)
    {
        if (rooms_cache.ContainsKey(level) && rooms_cache[level].ContainsKey(room))
        {
            return rooms_cache[level][room];
        }
        return null;
    }
}