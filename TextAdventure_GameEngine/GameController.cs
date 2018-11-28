using System;
using System.Collections.Generic;

namespace TextAdventure_GameEngine
{
    public class GameController
    {
        private Room _room;
        private TextInput _textInput;

        public List<UserAction> AvailableActions = new List<UserAction>() { new Examine(), new Go(), new Take() };

        public GameController()
        {
            _room = new Room("TestRoom_0.xml");
            _textInput = new TextInput(this);

            while (true)
            {
                var input = Console.ReadLine();
                _textInput.Accept(input);
            }
        }

        public void ExamineItem(string keyword)
        {
            if (_room.HasItem(keyword))
            {
                Item item = _room.GetItem(keyword);
                Console.WriteLine("\n{0}\n", item.DetailedDescription);
            }
            else
            {
                Console.WriteLine("\nThere is no {0}.\n", keyword);
            }
        }

        public void ChangeRoom(string keyword)
        {
            if (_room.HasExit(keyword))
            {
                Console.WriteLine("\nYou head off to the {0}.\n", keyword);
                _room = _room.Exit(keyword);
            }
            else
            {
                Console.WriteLine("\nThere is no path to the {0}.\n", keyword);
            }
        }
    }
}
