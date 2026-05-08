using System;
using System.Collections.Generic;

namespace PocketDarkSouls
{
    /// <summary>
    /// Base player class - handles core systems like inventory, health, wallet, and world interaction.
    /// Designed as a composition-based entity container rather than a logic-heavy class.
    /// </summary>
    public abstract class Player
    {

        public Room CurrentRoom { get; private set; }


        public string Name { get; }


        public string Type { get; init; }

        //-----------------------------------------------------------------------------------------
        // CORE SYSTEMS (COMPOSITION)
        //-----------------------------------------------------------------------------------------

        public Inventory Inventory { get; }


        public Wallet Wallet { get; }


        public HealthSystem Health { get; }

        public DialogHandler DialogHandler { get; }


        public Messenger Messenger { get; }

        public EntityEvents Events { get; }

        //-----------------------------------------------------------------------------------------
        // DIALOG / INTERACTION SYSTEM
        //-----------------------------------------------------------------------------------------

        private readonly Dictionary<string, Speak> _speakCommands = new();


        public Player(
            string name,
            List<Speak> dialog,
            EntityEvents events,
            Room startingRoom)
        {
            Name            = name;
            Events          = events;

            Wallet          = new Wallet(500, 100000);
            Inventory       = new Inventory(this);
            Health          = new HealthSystem(this, events);
            DialogHandler   = new DialogHandler();
            Messenger       = new Messenger(this);

            CurrentRoom     = startingRoom;

            AddSpeakCommands(dialog);
        }

        //-----------------------------------------------------------------------------------------
        // UPDATE LOOP
        //-----------------------------------------------------------------------------------------

        /// <summary>
        /// Called every game tick. Updates all player systems.
        /// </summary>
        public void Update()
        {
            Health.update();
            Wallet.update();
            AI();
        }

        /// <summary>
        /// Override for AI behavior in non-player implementations.
        /// </summary>
        public virtual void AI() { }

        //-----------------------------------------------------------------------------------------
        // SPEAK SYSTEM
        //-----------------------------------------------------------------------------------------

        /// <summary>
        /// Adds speak commands from a list into the player's command dictionary.
        /// </summary>
        private void AddSpeakCommands(List<Speak> commands)
        {
            foreach (var speak in commands)
            {
                _speakCommands[speak.keyword] = speak;
            }
        }

        /// <summary>
        /// Looks up a speak command by keyword.
        /// </summary>
        public Speak? LookUpSpeakCommand(string key)
        {
            if (_speakCommands.TryGetValue(key, out var speak))
                return speak;

            return null;
        }

        //-----------------------------------------------------------------------------------------
        // ITEM SYSTEM
        //-----------------------------------------------------------------------------------------

        /// <summary>
        /// Simple console-based item usage menu.
        /// </summary>
        public void UseItemMenu(string key)
        {
            bool used = false;

            while (key != null)
            {
                Console.WriteLine(Inventory.getItemInfo(key));
                Console.WriteLine("Input an amount to use:");

                string input = Console.ReadLine();

                if (int.TryParse(input, out int amount))
                {
                    if (amount <= 0)
                    {
                        Console.WriteLine("Canceled...");
                        break;
                    }

                    used = Inventory.useItem(key, amount);
                    break;
                }

                Console.Clear();
            }

            if (!used)
            {
                Messenger.WarningMessage("Item could not be used..", ConsoleColor.Red);
            }
        }


        public void DropItemMenu(string key)
        {
            bool dropped = false;
            while (key != null)
            {
                Console.WriteLine(Inventory.getItemInfo(key));
                Console.WriteLine("Input an amount to drop:");
                string input = Console.ReadLine();
                if (int.TryParse(input, out int amount))
                {
                    if (amount <= 0)
                    {
                        Console.WriteLine("Canceled...");
                        break;
                    }
                    Item? droppedItem = Inventory.DropItem(key, amount);
                    if (droppedItem != null)
                    {
                        CurrentRoom.AddItemToRoom(droppedItem);
                        dropped = true;
                    }
                    break;
                }
                Console.Clear();
            }
            if (!dropped)
            {
                Messenger.WarningMessage("Item could not be dropped..", ConsoleColor.Red);
            }
        }



        //-----------------------------------------------------------------------------------------
        // PLAYER INFO
        //-----------------------------------------------------------------------------------------

        /// <summary>
        /// Returns basic player information for UI/debugging.
        /// </summary>
        public List<string> GetInfo()
        {
            return new List<string>
            {
                Name,
                GetType().Name
            };
        }

        //-----------------------------------------------------------------------------------------
        // MOVEMENT SYSTEM
        //-----------------------------------------------------------------------------------------

        /// <summary>
        /// Warps player directly into a room.
        /// </summary>
        public void SpawnWarp(Room room)
        {
            if (CurrentRoom != null)
            {
                CurrentRoom.PlayerHasLeftRoom(this);
            }

            if (room != null)
            {
                CurrentRoom = room;
                room.PlayerHasEnteredRoom(this);
            }
        }

        /// <summary>
        /// Moves player through a directional exit if available.
        /// </summary>
        public void GoTo(string direction)
        {
            Room nextRoom = CurrentRoom.GetExit(direction);

            if (nextRoom != null)
            {
                CurrentRoom.PlayerHasLeftRoom(this);
                nextRoom.PlayerHasEnteredRoom(this);
                CurrentRoom = nextRoom;
            }
            else
            {
                Messenger.ErrorMessage("There is no path " + direction, ConsoleColor.Red);
            }
        }
    }
}