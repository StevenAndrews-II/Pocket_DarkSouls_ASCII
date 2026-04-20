using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;

namespace PocketDarkSouls
{
    /*
     * Spring 2026
     * This is the *node* in the graph
     * that is to become the game world.
     * The game world is a set of rooms
     * connected to each other.
     */
    public class Room
    {
        private Dictionary<string, Room> _exits;
        private string _tag;
        private string _conjunction;

        private  Dictionary<string, Player> players_in_room = new Dictionary<string, Player>();
         

        public string Tag { get { return _tag; } set { _tag = value; } }
        public string Conjunction { get { return _conjunction; } set { _conjunction = value; } }

        public string type { get; private set; }


        public Dictionary<string, Action> Actions { get; private set; }

        public Room() : this("empty", "in","normal", new Dictionary<string, Action>()) {}
        public Room(string tag) : this(tag, "in","normal", new Dictionary<string, Action>()) {}

        // Designated Constructor
        public Room(string tag, string conjunction , string type , Dictionary<string,Action> actions_)
        {
            _exits          = new Dictionary<string, Room>();
            Tag             = tag;
            Conjunction     = conjunction;
            this.type       = type;

            this.Actions    = actions_; // actions 
        }


        public void Set_Actions( string key,  Action action_)
        {
            Actions.Add(key, action_);
        }


        // entered room and exit room notification 
        public void PlayerHasEnteredRoom(Player player_)
        {
            players_in_room.Add(player_.name, player_);
        }

        public void PlayerHasLeftRoom(Player player_)
        {
            players_in_room.Remove(player_.name);
        }


        public Player? FindPlayerInRoom(string id) // new / communications /actions
        {
            if (players_in_room.ContainsKey(id))
            {
                return players_in_room[id];
            }
            return null;
        }




        public int GetOccupancyCount()
        {
            return players_in_room.Count;
        }


        public void SetExit(string exitName, Room room)
        {
            _exits[exitName] = room;
        }

        public Room GetExit(string exitName)
        {
            Room room = null;
            _exits.TryGetValue(exitName, out room);
            return room;
        }

        public string GetExits()
        {
            string exitNames = "Exits: ";
            Dictionary<string, Room>.KeyCollection keys = _exits.Keys;
            foreach (string exitName in keys)
            {
                exitNames += " " + exitName;
            }

            return exitNames;
        }

        public List<Room> GetExitsRoomList()
        {
            List<Room> out_ = new List<Room>();
            foreach (var(k,v) in _exits)
            {
                out_.Add(v);
            }

            return out_;
        }



        public List<List<string>> OccupancyToList()
        {
            List<List<string>> infolist = new List<List<string>>();

            foreach (var kv in players_in_room)
            {

                infolist.Add(kv.Value.GetInfo());

            }
            return infolist;
        }


        public string GetNearByPlayers(string name)
        {
            List<List<string>> occupancylist = OccupancyToList();
            string list_ = "";
            if (occupancylist.Count > 1)
            {
                list_ += "[Near Me]:\n";
            }
            foreach (List<string> index in occupancylist)
            {
                if (index[0] != name)
                {
                    list_ += $"\n{index[0],-20} : {index[1]}";
                }
            }
            return list_;
        }


        public string Description()
        {
            return "You are " + Conjunction + " " + Tag + " :: "+ type + ".\n *** " + this.GetExits() ;
        }
    }
}
