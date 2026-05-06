
using PocketDarkSouls;
public class InventoryUse : ICs
{
    public string keyword { get; } = "use"; // comand to initate this section

    public void Execute(Player p1, string? key = null)
    {
        if (key != null)
        {
            p1.UseItemMenu(key);
        }
        else{
            Console.WriteLine("Use what? ...");
        }
    }
}
