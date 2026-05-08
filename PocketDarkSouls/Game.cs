using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace PocketDarkSouls
{
    /*
     * Spring 2026
     */
    public class Game
    {
        private Player              _player;
        private Parser              _parser;
        private bool                _playing;
        private MapGenerator        mapGenerator;
        private CharacterCreator    Character_Creator;
        private ItemCreator         itemCreator;

        string IntroMessage = " drunk on more than just ale.\n" +
                   "Do you not feel it? The earth stirs beneath you. She whispers still.\n" +
                   "Far beyond these halls, the Mountain trembles — not with age, but with wrath.\n" +
                   "The paths above are choked... bound in the roots of the Abyss.\n" +
                   "Yet there are passages, hidden and half-forgotten — ways to where the old gods hoard their fading power.\n" +
                   "Carve your path through stone and shadow. The gods linger above.\n" +
                   "But heed this: let not the hunger below take hold of your soul.\n" +
                   "Seize what fate remains to you... or be buried with the rest.\n" +
                   "The tunnels shift. The deep closes in.\n" +
                   "Do not linger in doubt.\n" +
                   "Go forth. Seek out the power of the Gods...\n\n";

        public Game()
        {

            
            
            _playing                = false;
            itemCreator             = new ItemCreator();
            Character_Creator       = new CharacterCreator(itemCreator);
            mapGenerator            = new MapGenerator(Character_Creator, itemCreator);
            _parser                 = new Parser(new CommandWords());





            //Item potion1 = new HealingPotion("HP", 6, .1, 250, 10);


            Room start              = mapGenerator.Generate();
            _player                 = Character_Creator.createRandomPerson(null,"hero"); // main player 
            _player.SpawnWarp(start);



            //_player.main_inventory.AddItem(potion1);
            //_player.main_inventory.AddItem(armor);
        }





        public void Play()
        {

            // Enter the main command loop.  Here we repeatedly read commands and
            // execute them until the game is over.
            if(_playing)
            {
                bool finished = false;
                while (!finished)
                {

                    Character_Creator.update(); // internal updater for all players/NPC sub systems 

                    // death and restart screen 
                    if (!_player.Health.isAlive())
                    {
                        _player.Messenger.ErrorMessage("You have faild to reclaim yourself..." , ConsoleColor.Red);
                        _player.Messenger.ErrorMessage("Press Enter...", ConsoleColor.DarkRed);
                        string ok = Console.ReadLine();

                        // retart or respawn
                        if (!_player.Health.useLife())              // decrement and check
                        {
                            // new game - clear players list / generate new map + NPCs / respawn player in new location
                            Character_Creator.RemoveAllplayers();
                            Room start                  = mapGenerator.Generate();
                            _player                     = Character_Creator.createRandomPerson();
                            _player.SpawnWarp(start);
                        }
                        else
                        {
                            mapGenerator.GetRandomRoomByLevel(0);   // respawn at the bottom 
                            _player.Health.respawn();               // respawn Health to max health 
                        }
                    }


                    // GUI Hook / Render stats - for selected player
                    _player.Messenger.InfoMessage($"\n[{_player.Name} : {_player.GetType()}]", ConsoleColor.Cyan);
                    _player.Messenger.display_menu(ConsoleColor.Yellow,ConsoleColor.DarkYellow);
                    _player.Messenger.NormalMessage("\n" + _player.CurrentRoom.Description(), ConsoleColor.DarkYellow);
                    _player.Messenger.draw(); // render players GUI


                    Console.Write("\n>");
                    Command command = _parser.ParseCommand(Console.ReadLine());
                    Console.Clear();           // clear console
                    _player.Messenger.Clear(); // clear draw buffer 

                    if (command == null)
                    {   
                        _player.Messenger.ErrorMessage("I don't understand...", ConsoleColor.Red);
                    }
                    else
                    {
                        finished = command.Execute(_player);
                    }
                    
                }
            }

        }


        public void Start()
        {
            _playing = true;
            _player.Messenger.InfoMessage(Welcome()+ IntroMessage, ConsoleColor.Blue);
        }

        public void End()
        {
            _playing = false;
            //_player.Messenger.InfoMessage(Goodbye());
            _player.Messenger.InfoMessage(Goodbye(), ConsoleColor.Blue);
        }



        public string Welcome()
        {
            return $"Ahh... so you awaken once more, {_player.Name}";
        }

        public string Goodbye()
        {
            return "\nThank you for playing, Goodbye. \n";
        }

    }
}
