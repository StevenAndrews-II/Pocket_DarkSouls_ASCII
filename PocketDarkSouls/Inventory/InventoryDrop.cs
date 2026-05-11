using System;
using PocketDarkSouls;

namespace PocketDarkSouls
{
    public class InventoryDrop : ICs
    {
        public string keyword { get; } = "drop";

        public void Execute(Player p1, string key = null)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                p1.DropInventoryItem(null);
                return;
            }

            string[] parts = key.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            string itemKey = parts[0];
            int amount = 1;

            if (parts.Length > 1)
            {
                bool validAmount = int.TryParse(parts[1], out amount);

                if (!validAmount)
                {
                    p1.DropInventoryItem(itemKey, 1);
                    return;
                }
            }

            p1.DropInventoryItem(itemKey, amount);
        }
    }
}