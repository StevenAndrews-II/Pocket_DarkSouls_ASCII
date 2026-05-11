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

        private bool isWarpRoom = false;
        private bool hasWarpedPlayer = false;
        private Room warpTarget = null;

        private  Dictionary<string, Player> players_in_room = new Dictionary<string, Player>();
         
        private bool WiningRoom = false;

        public string Tag { get { return _tag; } set { _tag = value; } }
        public string Conjunction { get { return _conjunction; } set { _conjunction = value; } }

        public string type { get; private set; }


        public Dictionary<string, Action> Actions { get; private set; }

        private  Dictionary<string,Item> ItemsInRoom = new Dictionary<string, Item>();

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


        // ----------------------------------------------------
        // ITEMS IN ROOM MANAGEMENT
        // ----------------------------------------------------

        /// <summary>
        /// Generates a unique slot for an item in the room based on the item's id. If the item id already exists in the room, it appends a suffix to create a new unique id. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GenerateSlot(string id)
        {
            int suffix = 1;
            string newId = $"{id}_{suffix}";
            while (ItemsInRoom.ContainsKey(newId))
            {
                suffix++;
                newId = $"{id}_{suffix}";
            }
            return newId;
        }

        /// <summary>
        /// Adds an item to the room. If the item already exists in the room, it increases the quantity of that item by the specified amount. 
        /// </summary>
        /// <param name="item"></param> 
        /// <param name="amt"></param>
        public void AddItemToRoom(Item item, int amt)
        {
            int amountToAdd = Math.Abs(amt);

            if (item == null || amountToAdd <= 0)
            {
                return;
            }

            foreach (var entry in ItemsInRoom)
            {
                Item roomItem = entry.Value;

                if (roomItem.Equals(item))
                {
                    roomItem.numberOf += amountToAdd;
                    return;
                }
            }

            Item newRoomItem = item.CloneWithAmount(amountToAdd);
            string newId = GenerateSlot(item.id);

            ItemsInRoom[newId] = newRoomItem;
        }


        /// <summary>
        /// Adds an item to the room. If the item already exists in the room, it increases the quantity of that item by 1.
        /// </summary>
        /// <param name="item"></param>
        public void AddItemToRoom(Item item)
        {
            if (item == null)
            {
                return;
            }

            AddItemToRoom(item, item.numberOf);
        }

        /// <summary>
        /// Removes a single item from the room. If there are more than 1 of that item in the room, it decreases the quantity by 1. 
        /// </summary>
        /// <param name="itemId"></param>
        public void RemoveItemFromRoom(string itemId) // removes a singe item from the room, if there are more than 1 it decreases the quantity by 1
        {
            if (ItemsInRoom.ContainsKey(itemId))
            {
                if (ItemsInRoom[itemId].numberOf > 1)
                {
                    ItemsInRoom[itemId].numberOf--;
                }
                else
                {
                    ItemsInRoom.Remove(itemId);
                }
            }   
        }

        /// <summary>
        /// Removes a specified quantity of an item from the room, or removes the item entirely if the quantity to
        /// remove is greater than or equal to the quantity present.
        /// </summary>
        /// <param name="itemId">The unique identifier of the item to remove.</param>
        /// <param name="quantity">The number of items to remove from the room.</param>
        public void RemoveItemFromRoom(string itemId, int quantity) // removes a specified quantity of the item from the room, if the quantity to remove is greater than the quantity in the room it removes the item entirely
        {
            if (ItemsInRoom.ContainsKey(itemId))
            {
                if (ItemsInRoom[itemId].numberOf > quantity)
                {
                    ItemsInRoom[itemId].numberOf -= quantity;
                }
                else
                {
                    ItemsInRoom.Remove(itemId);
                }
            }
        }

        /// <summary>
        /// Removes and returns the item with the specified ID from the room.
        /// </summary>
        /// <param name="itemId">The unique identifier of the item to retrieve.</param>
        /// <returns>The item if found and removed; otherwise, null.</returns>
        public Item? GetItemFromRoom(string itemId) // removes the item from the room and returns it, if the item is not in the room it returns null
        {
            if (ItemsInRoom.ContainsKey(itemId))
            {
                Item item = ItemsInRoom[itemId];
                ItemsInRoom.Remove(itemId);
                return item;
            }
            return null;
        }


        /// <summary>
        /// Show all items in the room. If there are no items, it indicates that the room is empty. Otherwise, it lists each item along with its unique key, id, and quantity.
        /// </summary>
        /// <returns></returns>
        public string ShowAllItems()
        {
            if (ItemsInRoom.Count == 0)
            {
                return "Items in room:\n- None\n";
            }

            string itemList = "Items in room:\n";

            foreach (var entry in ItemsInRoom)
            {
                string itemKey = entry.Key;
                Item item = entry.Value;

                itemList += $"- {itemKey} : {item.id} x{item.numberOf}\n";
            }

            return itemList;
        }



        // ----------------------------------------------------
        // ACTIONS IN ROOM MANAGEMENT
        // ----------------------------------------------------
        // Actions are things that can be done in the room, they are not necessarily tied to items, they can be things like "look around" or "rest"
        public void Set_Actions( string key,  Action action_)
        {
            Actions.Add(key, action_);
        }


        // entered room and exit room notification 
        public void PlayerHasEnteredRoom(Player player_)
        {
            players_in_room.Add(player_.Name, player_);
            TryWarp(player_);
        }

        public void PlayerHasLeftRoom(Player player_)
        {
            players_in_room.Remove(player_.Name);
        }


        public Player? FindPlayerInRoom(string id) // new / communications /actions
        {
            if (players_in_room.ContainsKey(id))
            {
                return players_in_room[id];
            }
            return null;
        }

        public void SetWinCondition()
        {
            if (type == "boss")
            {
                WiningRoom = true;
            }
        }


        // ----------------------------------------------
        // Motion and Room Transition Management
        // ----------------------------------------------

        public void MakeWarpRoom(Room targetRoom)
        {
            isWarpRoom = true;
            warpTarget = targetRoom;
        }

        public bool IsWarpRoom()
        {
            return isWarpRoom;
        }

        public void TryWarp(Player player)
        {
            if (!isWarpRoom)
            {
                return;
            }

            // Prevents infinite warp loops.
            if (hasWarpedPlayer)
            {
                return;
            }

            if (warpTarget == null)
            {
                player.Messenger.ErrorMessage("The warp magic fizzles out...", ConsoleColor.Red);
                return;
            }

            hasWarpedPlayer = true;

            player.Messenger.InfoMessage(
                "The room twists around you. A dark portal pulls you somewhere else!",
                ConsoleColor.DarkMagenta
            );

            player.SpawnWarp(warpTarget);
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

        // ----------------------------------------------   
        // Player Information Management
        // ----------------------------------------------

        public List<List<string>> OccupancyToList()
        {
            List<List<string>> infolist = new List<List<string>>();

            foreach (var kv in players_in_room)
            {

                infolist.Add(kv.Value.GetInfo());

            }
            return infolist;
        }


        public string GetNearByPlayers(string Name)
        {
            List<List<string>> occupancylist = OccupancyToList();
            string list_ = "";
            if (occupancylist.Count > 1)
            {
                list_ += "[Near Me]:\n";
            }
            foreach (List<string> index in occupancylist)
            {
                if (index[0] != Name)
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
