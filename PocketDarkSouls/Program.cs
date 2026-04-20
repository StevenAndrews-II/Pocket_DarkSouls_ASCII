using System;

namespace PocketDarkSouls
{
    class Program
    {
        static void Main(string[] args)
        {
            // This is the top level of the application
            Game game = new Game();
            game.Start();
            game.Play();
            game.End();
        }
    }
}
