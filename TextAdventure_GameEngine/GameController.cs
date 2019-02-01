using System;
using System.Collections.Generic;
using System.IO;

namespace TextAdventure_GameEngine
{
    public class GameController
    {
        private Player _player;
        private TextInput _textInput;
        private Room _room;
        private bool _isGameOver = false;

        public List<UserAction> AvailableActions = new List<UserAction>() { new Check(), new Discard(), new Drop(), new DropSilent(), new Go(), new Inventory(), new Restart(), new Take(), new Talk(), new Use() };

        public GameController()
        {
            var source_dir = "Witcher";
            var save_dir = "Save";

            if (Directory.Exists(save_dir)) Directory.Delete(save_dir, true);

            _player = new Player() { Name = "Geralt", Gender = Genders.MALE, Gold = 12 };
            _textInput = new TextInput();
            _room = new Room("Village_Jack.xml");
            _player.UpdateLocation(_room);

            while (!_isGameOver)
            {
                var input = Console.ReadLine();
                _textInput.Accept(input, this);
            }
        }

        public void Check(string keyword)
        {
            if (_room.HasExit(keyword))
            {
                Exit exit = _room.GetExit(keyword);
                Console.WriteLine("\n{0}\n", exit.OnCheck);
            }
            else if (_room.HasCharacter(keyword))
            {
                Character character = _room.GetCharacter(keyword);
                Console.WriteLine("\n{0}\n", character.OnCheck);
            }
            else if (_room.HasProp(keyword))
            {
                Prop prop = _room.GetProp(keyword);
                Console.WriteLine("\n{0}\n", prop.OnCheck);
            }
            else if (_room.HasItem(keyword))
            {
                Item item = _room.GetItem(keyword);
                Console.WriteLine("\n{0}\n", item.OnCheck);
            }
            else
            {
                Console.WriteLine("\nThere is no {0} to examine.\n", keyword);
            }
        }

        public void Discard(string keyword)
        {
            if (_player.HasItem(keyword))
            {
                Item item = _player.GetItem(keyword);
                _player.RemoveItem(item);
            }
            else
            {
                Console.WriteLine("\nThere is no {0} in your inventory.\n", keyword);
            }
        }

        public void Drop(string keyword)
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

        public void DropSilent(string keyword)
        {
            if (_player.HasItem(keyword))
            {
                Item item = _player.GetItem(keyword);
                _room.AddItem(item);
                _player.RemoveItem(item);
            }
        }

        public void Go(string keyword)
        {
            if (_room.HasExit(keyword))
            {
                if (_room.GetExit(keyword).IsLocked)
                {
                    Console.WriteLine("\nThe {0} is locked.\n", keyword);
                }
                else
                {
                    bool isRestarting = false;
                    var actions = _room.GetExit(keyword).OnGo.Split('|');
                    foreach (var action in actions)
                    {
                        if (action == "") continue;
                        _textInput.Accept(action, this);
                        if (action.StartsWith("restart")) isRestarting = true;
                    }

                    if (isRestarting) return;

                    Console.WriteLine();

                    _room = _room.Exit(keyword);
                    _player.UpdateLocation(_room);
                }
            }
            else
            {
                Console.WriteLine("\nYou cannot go to the {0}.\n", keyword);
            }
        }

        public void Inventory()
        {
            string inventory = "\nName: " + _player.Name + "\n";// + "\nGold: " + _player.Gold + "\n";
            foreach (Item item in _player.Items)
            {
                inventory += item.Keyword + ": " + item.OnCheck + "\n";
            }

            Console.WriteLine(inventory);
        }

        public void Restart()
        {
            Directory.Delete("Save", true);
            Console.Clear();
            _isGameOver = true;
        }

        public void Take(string keyword)
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
                Console.WriteLine("\n{0}\n", character.OnTalk);
            }
            else
            {
                Console.WriteLine("\nYou cannot talk to the {0}.\n", keyword);
            }
        }

        public void Use(string keyword, string targetKeyword)
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

            InteractableObject target;
            if (_room.HasExit(targetKeyword))
            {
                target = _room.GetExit(targetKeyword);
            }
            else if (_room.HasCharacter(targetKeyword))
            {
                target = _room.GetCharacter(targetKeyword);
            } else if (_room.HasProp(targetKeyword))
            {
                target = _room.GetProp(targetKeyword);
            }
            else if (_room.HasItem(targetKeyword))
            {
                target = _room.GetItem(targetKeyword);
            }
            else
            {
                Console.WriteLine("\nThere is no {0}.\n", targetKeyword);
                return;
            }

            if (target.WantsItemId != item.Id)
            {
                Console.WriteLine("\nYou cannot use the {0} on the {1}.", keyword, targetKeyword);
                return;
            }

            Console.WriteLine("\nYou used the {0} on the {1}.\n", keyword, targetKeyword);
            var actions = target.OnUse.Split('|');
            foreach (var action in actions)
            {
                if (action == "") continue;
                _textInput.Accept(action, this);
            }
        }
    }
}