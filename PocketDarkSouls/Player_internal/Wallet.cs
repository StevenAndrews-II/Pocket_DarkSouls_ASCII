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

    // spells that make you loose gold here 
    public void update()
    {

    }


    public double GetGoldInWallet()
    {
        return gold;
    }

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
