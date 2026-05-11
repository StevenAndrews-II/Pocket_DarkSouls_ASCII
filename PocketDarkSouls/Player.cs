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

        private Stack<Room> _roomHistory = new Stack<Room>();

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
        /// Drops an item from the player's inventory into the current room. Validates input and provides feedback.
        /// </summary>
        /// <param name="itemKey"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public bool DropInventoryItem(string itemKey, int amount = 1)
        {
            if (string.IsNullOrWhiteSpace(itemKey))
            {
                Messenger.ErrorMessage("What should I drop?...", ConsoleColor.Red);
                return false;
            } 

            if (amount <= 0)
            { 
                Messenger.ErrorMessage("Drop amount must be greater than 0...", ConsoleColor.Red);
                return false;
            }

            if (CurrentRoom == null)
            { 
                Messenger.ErrorMessage("You are not in a room...", ConsoleColor.Red);
                return false;  
            }

            Item? droppedItem = Inventory.DropItem(itemKey, amount);

            if (droppedItem == null)
            {
                Messenger.ErrorMessage("You do not have that item...", ConsoleColor.Red);
                return false;
            }

            CurrentRoom.AddItemToRoom(droppedItem);

            Messenger.InfoMessage(
                $"Dropped {droppedItem.id} x{droppedItem.numberOf}.",
                ConsoleColor.Green
            );

            return true;
        }




        /// <summary>
        /// Scavenges the current room, showing description, nearby players, and items in the area.
        /// Provides contextual information for the player to make informed decisions.
        /// </summary>
        public void ScavengeCurrentRoom()
        {
            if (CurrentRoom == null)
            {
                Messenger.ErrorMessage("You are not in a room...", ConsoleColor.Red);
                return;
            }

            Messenger.InfoMessage("\nYou scavenge the area...", ConsoleColor.Yellow);

            Messenger.InfoMessage("\n" + CurrentRoom.Description(), ConsoleColor.White);

            string nearbyPlayers = CurrentRoom.GetNearByPlayers(Name);

            if (!string.IsNullOrWhiteSpace(nearbyPlayers))
            {
                Messenger.InfoMessage("\n" + nearbyPlayers, ConsoleColor.Gray);
            }

            Messenger.InfoMessage("\n" + CurrentRoom.ShowAllItems(), ConsoleColor.Green);
        }



        /// <summary>
        /// Equips an item from the inventory if possible. Validates input and provides feedback on success or failure.
        /// </summary>
        /// <param name="itemKey"></param>
        /// <returns></returns>
        public bool EquipItem(string itemKey)
        {
            if (string.IsNullOrWhiteSpace(itemKey))
            {
                Messenger.ErrorMessage("What should I equip?", ConsoleColor.Red);
                return false;
            }

            bool wasEquipped = Inventory.Equip(itemKey);

            if (wasEquipped)
            {
                Messenger.InfoMessage($"Equipped {itemKey}.", ConsoleColor.Green);
            }
            else
            {
                Messenger.ErrorMessage("Not equipable...", ConsoleColor.Red);
            }

            return wasEquipped;
        }



        /// <summary>
        ///  unequips an item from the inventory if possible. Validates input and provides feedback on success or failure.
        /// </summary>
        /// <param name="itemKey"></param>
        /// <returns></returns>
        public bool UnequipItem(string itemKey)
        {
            if (string.IsNullOrWhiteSpace(itemKey))
            {
                Messenger.ErrorMessage("What should I unequip?", ConsoleColor.Red);
                return false;
            }

            bool wasUnequipped = Inventory.Unequip(itemKey);

            if (wasUnequipped)
            {
                Messenger.InfoMessage($"Unequipped {itemKey}.", ConsoleColor.Green);
            }
            else
            {
                Messenger.ErrorMessage("Could not unequip that item...", ConsoleColor.Red);
            }

            return wasUnequipped;
        }


        /// <summary>
        /// Picks up an item from the current room and adds it to the inventory if possible. Validates input and provides feedback on success or failure.
        /// </summary>
        /// <param name="itemId"></param>
        public void PickupItem(string itemId)
        {
            if (string.IsNullOrWhiteSpace(itemId))
            {
                Messenger.ErrorMessage("Pickup what?", ConsoleColor.Red);
                return;
            }

            if (CurrentRoom == null)
            {
                Messenger.ErrorMessage("You are not in a room...", ConsoleColor.Red);
                return;
            }

            Item? item = CurrentRoom.GetItemFromRoom(itemId);

            if (item == null)
            {
                Messenger.ErrorMessage("That item is not in this room...", ConsoleColor.Red);
                return;
            }

            bool added = Inventory.AddItem(item);

            if (added)
            {
                Messenger.InfoMessage($"Picked up {item.id} x{item.numberOf}.", ConsoleColor.Green);
            }
            else
            {
                CurrentRoom.AddItemToRoom(item);
                Messenger.ErrorMessage("You cannot carry that item....", ConsoleColor.Red);
            }
        }

        /// <summary>
        /// Trades with another player, showing their items for sale and allowing the player to select one to purchase. Validates input and provides feedback throughout the process.
        /// </summary>
        /// <param name="p2">The player to trade with.</param>
        /// <param name="Dialog">The dialog options for the trade.</param>
        /// <returns>The key of the selected item, or null if no item was selected.</returns>
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


        /// <summary>
        /// Purchases an item from another player if it is still for sale and the player has enough gold.
        /// Handles the transaction and item transfer, providing feedback on success or failure.
        /// </summary>
        /// <param name="p2">The player to purchase the item from.</param>
        /// <param name="Dialog">The dialog options for the trade.</param>
        /// <param name="selected">The key of the selected item.        </param>
        public void PurchaseItem(Player p2, Dictionary<string, List<string>> Dialog, string? selected)
        {
            Item? purchase = p2.Inventory.getForSaleItem(selected);
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
            // transfer 
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

        /// <summary>
        /// Drop item menu with console input. Validates input and provides feedback on success or failure. Adds dropped item to current room if successful.
        /// </summary>
        /// <param name="key"></param>
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
        /// Go back to the previous room if there is one in the history stack. Provides feedback if there is nowhere to go back to.
        /// </summary>
        public void GoBack()
        {
            if (_roomHistory.Count == 0)
            {
                Messenger.InfoMessage("There is nowhere to go back to.", ConsoleColor.Blue);
                return;
            }

            Room previousRoom = _roomHistory.Pop();
            SpawnWarp(previousRoom);
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
                _roomHistory.Push(CurrentRoom);
                CurrentRoom = nextRoom;
            }
            else
            {
                Messenger.ErrorMessage("There is no path " + direction, ConsoleColor.Red);
            }
        }
    }
}