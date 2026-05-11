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

        public string? TradeMenu(Player p2, Dictionary<string, List<string>> Dialog)
        {
            Messenger.WarningMessage($"Trading with : [ {p2.Name} : {p2.GetType()} ] ", ConsoleColor.Yellow);

            Dictionary<string, Item> forsale = p2.Inventory.getAllItemsMarkedForSale();
            string selected = null;
            if (forsale.Count > 0)
            {
                Messenger.ReciveMessage(p2.Name, p2.DialogHandler.TradeSpeach(Dialog), ConsoleColor.Magenta);

                while (selected is null)
                {
                    Console.WriteLine("------------------------------------[ Trade Menu ]------------------------------------", ConsoleColor.White);

                    foreach (var (k, v) in forsale)//(int i = 0; i < forsale.Count; i++)
                    {
                        Console.WriteLine($"{k,-35}  >> ", ConsoleColor.White);
                        Console.WriteLine(v.ToString(), ConsoleColor.White);                   // use abstract ovveride of ToString 
                    }

                    Console.WriteLine($"Selected : ", ConsoleColor.Yellow);
                    string input = Console.ReadLine();

                    if (input == null || input.ToLower() == "exit")
                    {
                        Messenger.ReciveMessage(p2.Name, p2.DialogHandler.QuitTrade(Dialog), ConsoleColor.Magenta);
                        break;
                    }

                    if (forsale.ContainsKey(input))
                    {
                        Console.WriteLine($"Selected:  {input}", ConsoleColor.Yellow);
                        selected = input;
                        break;
                    }
                    else
                    {
                        Console.Clear();
                        Console.WriteLine($"Not Found:  {input}", ConsoleColor.Red);
                    }
                }

            }
            else
            {
                Messenger.ReciveMessage(p2.Name, p2.DialogHandler.NotEnoughToTrade(Dialog), ConsoleColor.Magenta);
            }
                return selected;
        }

        public void PurchaseItem(Player p2, Dictionary<string, List<string>> Dialog, string? selected)
        {
            Item? purchase = p2.Inventory.getForSaleItem(selected); // broken asf<----------------------------------------------------//////
            // handle case where item is no longer for sale
            if (purchase == null)
            {
                Messenger.ReciveMessage(p2.Name, "Seems there is nothing available for purchase.", ConsoleColor.Magenta);
                return;
            }
            // handle cash transaction 

            if (Wallet.gold < purchase.price)
            {
                Messenger.ReciveMessage(p2.Name, p2.DialogHandler.NotEnoughToTrade(Dialog), ConsoleColor.Magenta);
                return;
            }
            else
            {
                Wallet.GiveGold(purchase.price);
                p2.Wallet.AddGold(purchase.price);
            }
            // transfer item 
            p2.Inventory.SoldItem(purchase.id, 1);
            Inventory.AddItem(purchase);

            Messenger.ReciveMessage(p2.Name, p2.DialogHandler.ThankYouSpeach(Dialog), ConsoleColor.Magenta);
        }
   

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