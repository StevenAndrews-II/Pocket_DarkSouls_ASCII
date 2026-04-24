using System.Collections;
using System.Collections.Generic;
using System;

namespace PocketDarkSouls
{

    public abstract class Player
    {

        private Room _currentRoom = null;
        public Room CurrentRoom { get { return _currentRoom; } set { _currentRoom = value; } }

        public string type { get; init; }

        public Inventory        main_inventory; 
        public Wallet           wallet;
        public HealthSystem     health;
        public DialogHandler    dialogHandler;
        public Messenger        messenger;

        public EntityEvents     EventManager;


        public Dictionary<string, Speak> SpeakCommands { get; init; } = new Dictionary<string, Speak>();    




        public string name { get; init; }


        /// <summary>
        /// Player constructor, initializes the player's name, inventory, wallet, health system, dialog options, and event manager.
        /// </summary>
        /// <param name="name"              >The name of the player.</param>
        /// <param name="dialog"            >A list of dialogue options available to the player.</param>
        /// <param name="I_"                >The player's inventory.</param>
        /// <param name="InventoryCommands" >A list of inventory commands available to the player.</param>
        /// <param name="EventManager"      >The event manager for handling game events.</param>
        /// <param name="W_"                >The player's wallet.</param>
        /// <param name="H_"                >The player's health system.</param>
        /// <param name="room"              >The initial room the player is in.</param>
        public Player(string name, List<Speak> dialog, Inventory I_, EntityEvents EventManager, Wallet W_, HealthSystem H_, Room room)
        {
            AddSpeakCommand(dialog);

            this.main_inventory     = I_;
            this.wallet             = W_;
            this.health             = H_;
            this._currentRoom       = room;
            this.name               = name;
            this.messenger          = new Messenger(this);
            this.dialogHandler      = new DialogHandler();
            this.EventManager       = EventManager; 


        }





        /// <summary>
        /// Internal update function - should be called in the main game loop, handles time based effects and AI for non player characters.
        /// </summary>
        public void update()
        {
            health.update();
            wallet.update();
            AI();
        }

        /// <summary>
        /// Abstract optional function for AI behavior, should be overridden in non player character classes to define their behavior. Called in the update loop.
        /// </summary>
        public virtual void AI() { }


        /// <summary>
        /// Adds speak commands to the player's list of available commands, allowing them to interact with the game world through dialogue.
        /// Should be called during player initialization to set up their dialogue options.
        /// </summary>
        /// <param name="cmd"></param>
        public void AddSpeakCommand(List<Speak> cmd)
        {
            foreach (Speak speak in cmd)
            {
                SpeakCommands.Add(speak.keyword, speak);
            }
        }






        /// <summary>
        /// Looks up a speak command by its keyword, returning the corresponding Speak object if found, or null if not found.
        /// </summary>
        /// <param name="key">The keyword of the speak command to look up.</param>
        /// <returns>The corresponding Speak object if found; otherwise, null.</returns>
        public Speak? LookUpSpeakCommand(string key)
        {
            if (SpeakCommands.ContainsKey(key))
            {
                return SpeakCommands[key];
            }
            return null;
        }



        /// <summary>
        /// Gets basic information about the player, such as their name and type, and returns it as a list of strings.
        /// This can be used for display purposes or other game mechanics that require player information.
        /// </summary>
        /// <returns></returns>
        public List<string> GetInfo()
        {
            var info = new List<string>();
            info.Add(name);
            info.Add(this.GetType().Name);
            return info;
        }


        //-----------------------------------------------------------------------------------------
        // motion 
        //-----------------------------------------------------------------------------------------

        /// <summary>
        /// Spawns the player into a specified room, handling the necessary logic for leaving the current room and entering the new room.
        /// </summary>
        /// <param name="room">The room to spawn the player into.</param>
        public void SpawnWarp(Room room) // push to room 
        {
            if (CurrentRoom != null)
            {
                CurrentRoom.PlayerHasLeftRoom(this); // leave old room
            }
            if (room != null)
            {
                room.PlayerHasEnteredRoom(this); // enter next 
                this._currentRoom = room;        // set the ref 
            }
        }


        /// <summary>
        /// Moves the player to a new room in the specified direction, if an exit exists in that direction.
        /// Handles the necessary logic for leaving the current room and entering the new room, and provides feedback if no exit exists in the specified direction.
        /// </summary>
        /// <param name="direction"></param>
        public void goTo(string direction)
        {
            Room nextRoom = CurrentRoom.GetExit(direction);
            if (nextRoom != null)
            {


                CurrentRoom.PlayerHasLeftRoom(this); // leave old room 
                nextRoom.PlayerHasEnteredRoom(this); // enter next 
                CurrentRoom = nextRoom;              // set ref inside player

            }
            else
            {
                messenger.ErrorMessage("\nThere is no path " + direction, ConsoleColor.Red);
            }
        }
    }
}
