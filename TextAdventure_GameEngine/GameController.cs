using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure_GameEngine
{
    class GameController
    {
        private Room _room;
        public GameController()
        {
            _room = new Room("TestRoom_0.xml");

            while (true)
            {
                var input = Console.ReadLine();

                ChangeRoom(input);
            }
        }

        private void ChangeRoom(string direction)
        {
            if (_room.HasExit(direction))
            {
                Console.WriteLine("You head off to the {0}.", direction);
                _room = _room.Exit(direction);
            }
            else
            {
                Console.WriteLine("There is no path to the {0}.", direction);
            }
        }
    }
}
