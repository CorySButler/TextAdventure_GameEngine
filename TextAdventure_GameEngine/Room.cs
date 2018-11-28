using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace TextAdventure_GameEngine
{
    public class Room
    {
        private string _description;
        private List<Exit> _exits;
        private List<Item> _items;

        public Room(string filePath)
        {
            var data = XElement.Load(filePath);
            _description = data.Element("description").Value;
            _exits = (from exit in data.Elements("exit")
                     select new Exit
                     {
                         Keyword = exit.Element("keyword").Value,
                         Description = exit.Element("description").Value,
                         Destination = exit.Element("destination").Value
                     }).ToList();

            _items = (from item in data.Elements("item")
                     select new Item
                     {
                         Keyword = item.Element("keyword").Value,
                         Description = item.Element("description").Value,
                         DetailedDescription = item.Element("detailedDescription").Value,
                         IsTakeable = item.Element("isTakeable").Value == "true"
                     }).ToList();

            Console.WriteLine(Describe());
        }

        private UserAction DetermineUserAction(string keyword)
        {
            switch (keyword)
            {
                case "examine":
                    return new Examine();
                case "take":
                    return new Take();
            }
            return null;
        }

        public string Describe()
        {
            string combinedText = _description + "\n";

            foreach (Exit exit in _exits)
            {
                combinedText += exit.Description + "\n";
            }

            foreach (Item item in _items)
            {
                combinedText += item.Description + "\n";
            }

            return combinedText;
        }

        public bool HasExit(string keyword)
        {
            foreach (Exit exit in _exits)
            {
                if (exit.Keyword == keyword) return true;
            }
            return false;
        }

        public Item GetItem(string keyword)
        {
            foreach (Item item in _items)
            {
                if (item.Keyword == keyword) return item;
            }
            return null;
        }

        public bool HasItem(string keyword)
        {
            foreach (Item item in _items)
            {
                if (item.Keyword == keyword) return true;
            }
            return false;
        }

        public Room Exit(string keyword)
        {
            Save();
            foreach (Exit exit in _exits)
            {
                if (exit.Keyword == keyword) return new Room(exit.Destination);
            }
            return this;
        }

        private void Save()
        {

        }
    }
}
