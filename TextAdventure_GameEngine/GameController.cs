using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure_GameEngine
{
    class GameController
    {
        public GameController()
        {
            var room = new Room("..\\..\\TestRoom_0.xml");
            Console.ReadLine();
        }
    }
}
