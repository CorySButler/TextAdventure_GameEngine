﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace TextAdventure_GameEngine
{
    public class Room
    {
        public string Id { get; private set; }
        private string _savePath;
        private string _filePath;
        private string _description;
        private string _hint;
        private List<Exit> _exits;
        private List<Item> _items;
        private List<Prop> _props;
        private List<Container> _containers;
        private List<Character> _characters = new List<Character>();
        private GameLog _gameLog;

        public List<Container> Containers { get { return _containers; } }
        public string FilePath { get { return _filePath; } }

        public Room(string fileName, GameLog gameLog, Player player)
        {
            Id = fileName;
            _gameLog = gameLog;
            _savePath = "Save\\" + fileName;

            if (File.Exists(_savePath))
                _filePath = _savePath;
            else
                _filePath = "GameData\\" + fileName;

            var data = XElement.Load(_filePath);
            _description = data.Element("description").Value;
            if (data.Elements("hint").Any()) _hint = data.Element("hint").Value;
            _exits = (from exit in data.Elements("exit")
                     select new Exit
                     {
                         Keyword = exit.Element("keyword").Value,
                         Description = exit.Element("description").Value,
                         OnCheck = exit.Element("onCheck").Value,
                         Destination = exit.Element("destination").Value,
                         OnGo = exit.Elements("onGo").Any() ? exit.Element("onGo").Value : "",
                         IsLocked = exit.Elements("isLocked").Any() ? exit.Element("isLocked").Value != "false" : false,
                         WantsItemId = exit.Elements("wantsItemId").Any() ? exit.Element("wantsItemId").Value : "",
                         OnUse = exit.Elements("onUse").Any() ? exit.Element("onUse").Value : ""
                     }).ToList();

            _props = (from prop in data.Elements("prop")
                      select new Prop
                      {
                          Keyword = prop.Element("keyword").Value,
                          Description = prop.Element("description").Value,
                          OnCheck = prop.Element("onCheck").Value,
                          WantsItemId = prop.Elements("wantsItemId").Any() ? prop.Element("wantsItemId").Value : "",
                          OnUse = prop.Elements("onUse").Any() ? prop.Element("onUse").Value : ""
                      }).ToList();

            var characterKeywords = (from character in data.Elements("character")
                      select character.Element("keyword").Value).ToList();

            _containers = (from container in data.Elements("container")
                      select new Container
                      {
                          Keyword = container.Element("keyword").Value,
                          Description = container.Element("description").Value,
                          OnCheck = container.Element("onCheck").Value,
                          WantsItemId = container.Elements("wantsItemId").Any() ? container.Element("wantsItemId").Value : "",
                          OnUse = container.Elements("onUse").Any() ? container.Element("onUse").Value : "",
                          Inventory = (from item in container.Elements("item")
                         select new Item
                         {
                             Keyword = item.Element("keyword").Value,
                             Description = item.Element("description").Value,
                             OnCheck = item.Element("onCheck").Value,
                             Id = item.Element("id").Value,
                             OnTake = item.Elements("onTake").Any() ? item.Element("onTake").Value : "",
                             WantsItemId = item.Elements("wantsItemId").Any() ? item.Element("wantsItemId").Value : "",
                             OnUse = item.Elements("onUse").Any() ? item.Element("onUse").Value : ""
                         }).ToList()
                      }).ToList();

            _items = (from item in data.Elements("item")
                     select new Item
                     {
                         Keyword = item.Element("keyword").Value,
                         Description = item.Element("description").Value,
                         OnCheck = item.Element("onCheck").Value,
                         Id = item.Element("id").Value,
                         OnTake = item.Elements("onTake").Any() ? item.Element("onTake").Value : "",
                         WantsItemId = item.Elements("wantsItemId").Any() ? item.Element("wantsItemId").Value : "",
                         OnUse = item.Elements("onUse").Any() ? item.Element("onUse").Value : ""
                     }).ToList();

            foreach (var member in player.Party) _characters.Add(member);

            foreach (var characterKeyword in characterKeywords.Where(ck => !_characters.Any(c => c.Keyword == ck)))
                _characters.Add(new Character(characterKeyword, this));

            foreach (var character in _characters)
                character.IncNumVisits(this);

            _gameLog.Write(Describe());
        }

        public string Describe()
        {
            string combinedText = _description + " ";

            foreach (Exit exit in _exits)
            {
                if (exit.Description != "") combinedText += exit.Description + " ";
            }

            foreach (Prop prop in _props)
            {
                if (prop.Description != "") combinedText += prop.Description + " ";
            }

            foreach (Container container in _containers)
            {
                if (container.Description != "") combinedText += container.Description + " ";
            }

            foreach (Character character in _characters)
            {
                if (character.Description != "") combinedText += character.Describe(this) + " ";
            }

            foreach (Item item in _items)
            {
                if (item.Description != "") combinedText += item.Description + " ";
            }

            return combinedText;
        }

        public string GetHint()
        {
            return _hint != "" ? _hint : "No hint available.";
        }

        public void AddCharacter(Character character)
        {
            _characters.Add(character);
        }

        public void AddItem(Item item)
        {
            _items.Add(item);
            Save();
        }

        public void DestroyCharacter(Character character)
        {
            _characters.Remove(character);
            character.Describe(this);
        }

        public void DestroyProp(Prop prop)
        {
            _props.Remove(prop);
        }

        public bool HasCharacter(string keyword)
        {
            foreach (Character character in _characters)
            {
                if (character.Keyword == keyword) return true;
            }
            return false;
        }

        public bool HasExit(string keyword)
        {
            foreach (Exit exit in _exits)
            {
                if (exit.Keyword == keyword) return true;
            }
            return false;
        }

        public bool HasItem(string keyword)
        {
            foreach (Item item in _items)
            {
                if (item.Keyword == keyword) return true;
            }
            return false;
        }

        public bool HasProp(string keyword)
        {
            foreach (Prop prop in _props)
            {
                if (prop.Keyword == keyword) return true;
            }
            return false;
        }

        public bool HasContainer(string keyword)
        {
            foreach (Container container in _containers)
            {
                if (container.Keyword == keyword) return true;
            }
            return false;
        }

        public Character GetCharacter(string keyword)
        {
            foreach (Character character in _characters)
            {
                if (character.Keyword == keyword) return character;
            }
            return null;
        }

        public Exit GetExit(string keyword)
        {
            foreach (Exit exit in _exits)
            {
                if (exit.Keyword == keyword) return exit;
            }
            return null;
        }

        public Item GetItem(string keyword)
        {
            foreach (Item item in _items)
            {
                if (item.Keyword == keyword) return item;
            }
            return null;
        }

        public Prop GetProp(string keyword)
        {
            foreach (Prop prop in _props)
            {
                if (prop.Keyword == keyword) return prop;
            }
            return null;
        }

        public Container GetContainer(string keyword)
        {
            foreach (Container container in _containers)
            {
                if (container.Keyword == keyword) return container;
            }
            return null;
        }

        public void RemoveCharacter(Character character)
        {
            _characters.Remove(character);
            Save();
        }

        public void RemoveItem(Item item)
        {
            _items.Remove(item);
            Save();
        }

        public Room Exit(string keyword, Player player)
        {
            foreach (Exit exit in _exits)
            {
                if (exit.Keyword == keyword)
                {
                    if (exit.IsLocked) return this;
                    Save();
                    return new Room(exit.Destination, _gameLog, player);
                }
            }
            return this;
        }

        private void Save()
        {
            if (!Directory.Exists("Save")) Directory.CreateDirectory("Save");

            var data = new XElement("root", new XElement("description", _description), new XElement("hint", _hint));
            data = AddCharactersToXElement(data);
            data = AddContainersToXElement(data);
            data = AddExitsToXElement(data);
            data = AddPropsToXElement(data);
            data = AddItemsToXElement(data);
            data.Save(_savePath);
        }

        private XElement AddCharactersToXElement(XElement xElement)
        {
            foreach (Character character in _characters)
            {
                xElement.Add(new XElement("character",
                    new XElement("keyword", character.Keyword),
                    new XElement("description", character.Description),
                    new XElement("onCheck", character.OnCheck),
                    //new XElement("onTalk", character.OnTalk),
                    new XElement("wantsItemId", character.WantsItemId),
                    new XElement("onUse", character.OnUse)
                    ));
            }

            return xElement;
        }

        private XElement AddContainersToXElement(XElement xElement)
        {
            foreach (Container container in _containers)
            {
                xElement.Add(new XElement("container",
                    new XElement("keyword", container.Keyword),
                    new XElement("description", container.Description),
                    new XElement("onCheck", container.OnCheck),
                    new XElement("wantsItemId", container.WantsItemId),
                    new XElement("onUse", container.OnUse)
                    ));

                foreach (var item in container.Inventory)
                {
                    xElement.LastNode.AddAfterSelf(new XElement("item",
                        new XElement("keyword", item.Keyword),
                        new XElement("description", item.Description),
                        new XElement("onCheck", item.OnCheck),
                        new XElement("id", item.Id),
                        new XElement("onTake", item.OnTake),
                        new XElement("wantsItemId", item.WantsItemId),
                        new XElement("onUse", item.OnUse)
                        ));
                }
            }

            return xElement;
        }

        private XElement AddExitsToXElement(XElement xElement)
        {
            foreach (Exit exit in _exits)
            {
                xElement.Add(new XElement("exit",
                    new XElement("keyword", exit.Keyword),
                    new XElement("description", exit.Description),
                    new XElement("onCheck", exit.OnCheck),
                    new XElement("destination", exit.Destination),
                    new XElement("isLocked", exit.IsLocked),
                    new XElement("wantsItemId", exit.WantsItemId),
                    new XElement("onUse", exit.OnUse),
                    new XElement("onGo", exit.OnGo)
                    ));
            }

            return xElement;
        }

        private XElement AddPropsToXElement(XElement xElement)
        {
            foreach (Prop prop in _props)
            {
                xElement.Add(new XElement("prop",
                    new XElement("keyword", prop.Keyword),
                    new XElement("description", prop.Description),
                    new XElement("onCheck", prop.OnCheck),
                    new XElement("wantsItemId", prop.WantsItemId),
                    new XElement("onUse", prop.OnUse)
                    ));
            }

            return xElement;
        }

        private XElement AddItemsToXElement(XElement xElement)
        {
            foreach (Item item in _items)
            {
                xElement.Add(new XElement("item",
                    new XElement("keyword", item.Keyword),
                    new XElement("description", item.Description),
                    new XElement("onCheck", item.OnCheck),
                    new XElement("id", item.Id),
                    new XElement("onTake", item.OnTake),
                    new XElement("wantsItemId", item.WantsItemId),
                    new XElement("onUse", item.OnUse)
                    ));
            }

            return xElement;
        }
    }
}
