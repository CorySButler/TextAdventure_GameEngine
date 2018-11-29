using System;
using System.Collections.Generic;

namespace TextAdventure_GameEngine
{
    public class GameController
    {
        private Player _player;
        private TextInput _textInput;
        private Room _room;

        public List<UserAction> AvailableActions = new List<UserAction>() { new Examine(), new Go(), new Inventory(), new Take() };

        public GameController()
        {
            _player = new Player() { Name = "Henry", Gender = Genders.MALE, Gold = 10 };
            _textInput = new TextInput();
            _room = new Room("TestRoom_0.xml");
            _player.UpdateLocation(_room);

            while (true)
            {
                var input = Console.ReadLine();
                _textInput.Accept(input, this);
            }
        }

        public void ChangeRoom(string keyword)
        {
            if (_room.HasExit(keyword))
            {
                Console.WriteLine("\nYou head off to the {0}.\n", keyword);
                _room = _room.Exit(keyword);
                _player.UpdateLocation(_room);
            }
            else
            {
                Console.WriteLine("\nThere is no path to the {0}.\n", keyword);
            }
        }

        public void CheckInventory()
        {
            string inventory = "\nName: " + _player.Name + "\nGold: " + _player.Gold + "\n";
            foreach (Item item in _player.Items)
            {
                inventory += item.Keyword + ": " + item.DetailedDescription + "\n";
            }

            Console.WriteLine(inventory);
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
                Console.WriteLine("\nThere is no {0} to examine.\n", keyword);
            }
        }

        public void TakeItem(string keyword)
        {
            if (_room.HasItem(keyword))
            {
                Item item = _room.GetItem(keyword);
                _player.AddItem(item);
                _room.RemoveItem(item);
                Console.WriteLine("\nYou take the {0}.\n", keyword);
            }
            else
            {
                Console.WriteLine("\nThere is no {0} to take.\n", keyword);
            }
        }
    }
}
