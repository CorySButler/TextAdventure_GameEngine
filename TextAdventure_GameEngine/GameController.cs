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
        private GameLog _gameLog;

        public List<UserAction> AvailableActions = new List<UserAction>() { new Check(), new Discard(), new Drop(), new DropSilent(), new Go(), new Hint(), new Help(), new Inventory(), new Restart(), new Take(), new Talk(), new Use() };

        public GameController()
        {
            var save_dir = "Save";

            if (Directory.Exists(save_dir)) Directory.Delete(save_dir, true);

            _gameLog = new GameLog();
            _player = new Player() { Name = "Geralt", Gender = Genders.MALE, Gold = 12 };
            _textInput = new TextInput();
            _room = new Room("Village_Jack.xml", _gameLog);
            _player.UpdateLocation(_room);

            while (!_isGameOver)
            {
                var input = Console.ReadLine();
                _gameLog.WriteSilent(input);
                var response = _textInput.Accept(input, this);
                if (response != "") _gameLog.Write(response);
            }
        }

        public void Check(string keyword)
        {
            if (_room.HasExit(keyword))
            {
                Exit exit = _room.GetExit(keyword);
                _gameLog.Write(exit.OnCheck);
            }
            else if (_room.HasCharacter(keyword))
            {
                Character character = _room.GetCharacter(keyword);
                _gameLog.Write(character.OnCheck);
            }
            else if (_room.HasProp(keyword))
            {
                Prop prop = _room.GetProp(keyword);
                _gameLog.Write(prop.OnCheck);
            }
            else if (_room.HasItem(keyword))
            {
                Item item = _room.GetItem(keyword);
                _gameLog.Write(item.OnCheck);
            }
            else
            {
                _gameLog.Write("There is no " + keyword + " to examine.");
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
                _gameLog.Write("There is no " + keyword + " in your inventory.");
            }
        }

        public void Drop(string keyword)
        {
            if (_player.HasItem(keyword))
            {
                Item item = _player.GetItem(keyword);
                _room.AddItem(item);
                _player.RemoveItem(item);
                _gameLog.Write("You drop the " + keyword + ".");
            }
            else
            {
                _gameLog.Write("There is no " + keyword + " in your inventory.");
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
                    _gameLog.Write("The " + keyword + " is locked.");
                }
                else
                {
                    bool isRestarting = false;
                    var actions = _room.GetExit(keyword).OnGo.Split('|');
                    foreach (var action in actions)
                    {
                        if (action == "") continue;
                    var response = _textInput.Accept(action, this);
                    if (response != "") _gameLog.Write(response);
                        if (action.StartsWith("restart")) isRestarting = true;
                    }

                    if (isRestarting)
                    {
                        //TODO: save game log
                        return;
                    }

                    _gameLog.Write("");
                    Console.Clear();

                    _room = _room.Exit(keyword);
                    _player.UpdateLocation(_room);
                }
            }
            else
            {
                _gameLog.Write("You cannot go to the " + keyword + ".");
            }
        }

        public void Hint()
        {
            _gameLog.Write(_room.GetHint());
        }

        public void Help()
        {
            _gameLog.Write(_room.GetHint());
        }

        public void Inventory()
        {
            string inventory = "Name: " + _player.Name + "\n";// + "Gold: " + _player.Gold + "";
            foreach (Item item in _player.Items)
            {
                inventory += item.Keyword + ": " + item.OnCheck + "\n";
            }

            _gameLog.Write(inventory);
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
                _gameLog.Write("You take the " + keyword + ".");
                var actions = item.OnTake.Split('|');
                foreach (var action in actions)
                {
                    if (action == "") continue;
                    var response = _textInput.Accept(action, this);
                    if (response != "") _gameLog.Write(response);
                }
            }
            else if (_room.HasProp(keyword))
            {
                _gameLog.Write("You cannot take the " + keyword + ".");
            }
            else if (_room.HasCharacter(keyword))
            {
                _gameLog.Write("You cannot take " + keyword + ".");
            }
            else
            {
                _gameLog.Write("There is no " + keyword + " to take.");
            }
        }

        public void Talk(string keyword)
        {
            if (_room.HasCharacter(keyword))
            {
                Character character = _room.GetCharacter(keyword);
                _gameLog.Write(character.OnTalk);
            }
            else
            {
                _gameLog.Write("You cannot talk to the " + keyword + ".");
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
                _gameLog.Write("There is no " + keyword + " in your inventory.");
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
                _gameLog.Write("There is no " + keyword + ".");
                return;
            }

            if (target.WantsItemId != item.Id)
            {
                _gameLog.Write("You cannot use the " + keyword + " on the " + targetKeyword + ".");
                return;
            }

            _gameLog.Write("You used the " + keyword + " on the " + targetKeyword + ".");
            var actions = target.OnUse.Split('|');
            foreach (var action in actions)
            {
                if (action == "") continue;
                var response = _textInput.Accept(action, this);
                if (response != "") _gameLog.Write(response);
            }
        }
    }
}