using System;
using PocketDarkSouls;
public class Wallet
{

    public double gold { get; private set; }
    private int capacity { get; set; }
    private string name { get; set; }

    public int Capacity { get; private set; }

    public Wallet(int start,int capacity)
    {
        this.Capacity = capacity;
        gold          = start;

    }

    // spells that make you lose gold here 
    public void update()
    {

    }

    /// <summary>
    /// Gets the current amount of gold in the wallet.
    /// </summary>
    /// <returns>The amount of gold currently in the wallet.</returns>
    public double GetGoldInWallet()
    {
        return gold;
    }

    /// <summary>
    /// Adds gold to the wallet, ensuring that the total does not exceed the wallet's capacity. If adding the specified 
    /// amount would exceed the capacity, the method returns false and does not add any gold. Otherwise, it adds the gold and returns true.
    /// </summary>
    /// <param name="amount">The amount of gold to add.</param>
    /// <returns>True if the gold was added successfully, false if it would exceed the capacity. </returns>
    public bool AddGold(double amount)
    {
        if (gold + Math.Abs(amount) > Capacity)
        {
            return false;
        }
        else
        {
            gold += amount;
            return true;
        }
    }
    /// <summary>
    /// Attempts to subtract the specified amount of gold if sufficient balance is available.
    /// </summary>
    /// <param name="amount">The amount of gold to give.</param>
    /// <returns>True if the gold was successfully given; otherwise, false.</returns>
    public bool GiveGold(double amount)
    {
        if (gold >= Math.Abs(amount))
        {
            gold -= amount;
            return true;
        }
        return false;
    }
    


}
