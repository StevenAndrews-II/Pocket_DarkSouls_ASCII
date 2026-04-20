using System;
using PocketDarkSouls;
public interface ICs
{
    public string keyword { get; }
    public void Execute(Player p1, string key = null);
}
