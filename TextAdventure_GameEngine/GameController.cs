using System;
using System.Collections.Generic;

namespace TextAdventure_GameEngine
{
    public class GameController
    {
        private Player _player;
        private TextInput _textInput;
        private Room _room;

        public List<UserAction> AvailableActions = new List<UserAction>() { new Check(), new Drop(), new Go(), new Inventory(), new Take(), new Talk(), new Use() };

        public GameController()
        {
            _player = new Player() { Name = "Geralt", Gender = Genders.MALE, Gold = 12 };
            _textInput = new TextInput();
            _room = new Room("Village_Jack.xml");
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
                if (_room.GetExit(keyword).IsLocked)
                {
                    Console.WriteLine("\nThe path to the {0} is locked.\n", keyword);
                }
                else
                { 
                    _room = _room.Exit(keyword);
                    _player.UpdateLocation(_room);
                }
            }
            else
            {
                Console.WriteLine("\nThere is no path to the {0}.\n", keyword);
            }
        }

        public void CheckInventory()
        {
            string inventory = "\nName: " + _player.Name + "\ncrowns: " + _player.Gold + "\n";
            foreach (Item item in _player.Items)
            {
                inventory += item.Keyword + ": " + item.DetailedDescription + "\n";
            }

            Console.WriteLine(inventory);
        }

        public void DropItem(string keyword)
        {
            if (_player.HasItem(keyword))
            {
                Item item = _player.GetItem(keyword);
                _room.AddItem(item);
                _player.RemoveItem(item);
                Console.WriteLine("\nYou drop the {0}.\n", keyword);
            }
            else
            {
                Console.WriteLine("\nThere is no {0} in your inventory.\n", keyword);
            }
        }

        public void ExamineItem(string keyword)
        {
            if (_room.HasExit(keyword))
            {
                Exit exit = _room.GetExit(keyword);
                Console.WriteLine("\n{0}\n", exit.DetailedDescription);
            }
            else if (_room.HasProp(keyword))
            {
                Prop prop = _room.GetProp(keyword);
                Console.WriteLine("\n{0}\n", prop.DetailedDescription);
            }
            else if (_room.HasItem(keyword))
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
                if (item.OnTake != "")
                    _textInput.Accept(item.OnTake, this);
            }
            else if (_room.HasProp(keyword))
            {
                Console.WriteLine("\nYou cannot take the {0}.\n", keyword);
            }
            else if (_room.HasCharacter(keyword))
            {
                Console.WriteLine("\nYou cannot take {0}.\n", keyword);
            }
            else
            {
                Console.WriteLine("\nThere is no {0} to take.\n", keyword);
            }
        }

        public void Talk(string keyword)
        {
            if (_room.HasCharacter(keyword))
            {
                Character character = _room.GetCharacter(keyword);
                Console.WriteLine(character.OnTalk);
            }
            else
            {
                Console.WriteLine("You cannot talk to the {0}.", keyword);
            }
        }

        public void UseItem(string keyword, string targetKeyword)
        {
            Item item;
            if (_player.HasItem(keyword))
            {
                item = _player.GetItem(keyword);
            }
            else
            {
                Console.WriteLine("\nThere is no {0} in your inventory.\n", keyword);
                return;
            }

            Prop interactableObject;
            /*if (_room.HasExit(targetKeyword))
            {
                interactableObject = _room.GetExit(targetKeyword);
            }
            else*/ if (_room.HasProp(targetKeyword))
            {
                interactableObject = _room.GetProp(targetKeyword);
            }/*
            else if (_room.HasItem(targetKeyword))
            {
                interactableObject = _room.GetItem(targetKeyword);
            }*/
            else
            {
                Console.WriteLine("\nThere is no {0}.\n", targetKeyword);
                return;
            }

            if (interactableObject.WantItemId != item.Id)
            {
                Console.WriteLine("\nYou cannot use the {0} on the {1}.", keyword, targetKeyword);
                return;
            }

            Console.WriteLine("\nYou used the {0} on the {1}.\n", keyword, targetKeyword);
            _textInput.Accept(interactableObject.OnUse, this);
            _player.RemoveItem(item);
        }
    }
}