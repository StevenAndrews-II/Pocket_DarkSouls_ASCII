using System;

namespace PocketDarkSouls
{
    public sealed class GameManager
    {
        private static GameManager? _instance;

        public static GameManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameManager();
                }

                return _instance;
            }
        }

        private GameManager()
        {
            IsRunning = true;
        }

        public bool IsRunning { get; private set; }

        public void QuitGame()
        {
            IsRunning = false;
        }

        public void StartGame()
        {
            IsRunning = true;
        }
    }
}