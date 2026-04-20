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

        public Inventory        main_inventory; // move this to a maker 
        public Wallet           wallet;
        public HealthSystem     health;
        public DialogHandler    dialogHandler;
        public Messenger        messenger;

        


        public Dictionary<string, Speak> SpeakCommands { get; init; } = new Dictionary<string, Speak>();    
        public Dictionary<string, ICs> InventoryCommands { get; init; } = new Dictionary<string, ICs>();




        public string name { get; init; }
        public Player(string name, List<Speak> dialog, Inventory I_, List<ICs> InventoryCommands ,Wallet W_, HealthSystem H_, Room room)
        {
            AddSpeakCommand(dialog);
            AddInventoryCommand(InventoryCommands);

            this.main_inventory     = I_;
            this.wallet             = W_;
            this.health             = H_;
            this._currentRoom       = room;
            this.name               = name;
            this.messenger          = new Messenger(this);
            this.dialogHandler      = new DialogHandler(this);
           
        }





        // internal updater 
        public void update()
        {
            health.update();
            wallet.update();
            AI();
        }

        // Overwritten by Player/Character types - see character abstracts
        public virtual void AI() { } 
            

        // add speak command user interface linkers 
        public void AddSpeakCommand(List<Speak> cmd)
        {
            foreach (Speak speak in cmd)
            {
                SpeakCommands.Add(speak.keyword, speak);
            }
        }
        // Internal player systems hooking 
        public void AddInventoryCommand(List<ICs> cmd)
        {
            foreach (ICs inv_cmd in cmd)
            {
                InventoryCommands.Add(inv_cmd.keyword, inv_cmd);
            }
        }




        // looks internally in the list of availible speak commands and passes it to subsystems
        public Speak? LookUpSpeakCommand(string key)
        {
            if (SpeakCommands.ContainsKey(key))
            {
                return SpeakCommands[key];
            }
            return null;
        }



        // get infor about player ( name and type // EG: Steven_andrews : Merchant )
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

        // warps directly to room - used for spawn as well 
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
