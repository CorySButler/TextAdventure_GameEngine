using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace TextAdventure_GameEngine
{
    public class Room
    {
        private string _filePath;
        private string _description;
        private List<Exit> _exits;
        private List<Item> _items;
        private List<Prop> _props;

        public string FilePath { get { return _filePath; } }

        public Room(string filePath)
        {
            _filePath = filePath;

            var data = XElement.Load(_filePath);
            _description = data.Element("description").Value;
            _exits = (from exit in data.Elements("exit")
                     select new Exit
                     {
                         Keyword = exit.Element("keyword").Value,
                         Description = exit.Element("description").Value,
                         Destination = exit.Element("destination").Value,
                         IsLocked = exit.Element("isLocked").Value == "true"
                     }).ToList();

            _items = (from item in data.Elements("item")
                     select new Item
                     {
                         Keyword = item.Element("keyword").Value,
                         Description = item.Element("description").Value,
                         DetailedDescription = item.Element("detailedDescription").Value
                     }).ToList();

            _props = (from prop in data.Elements("prop")
                      select new Prop
                      {
                          Keyword = prop.Element("keyword").Value,
                          Description = prop.Element("description").Value,
                          DetailedDescription = prop.Element("detailedDescription").Value
                      }).ToList();

            Console.WriteLine(Describe());
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

            foreach (Prop prop in _props)
            {
                combinedText += prop.Description + "\n";
            }

            return combinedText;
        }

        public void AddItem(Item item)
        {
            _items.Add(item);
            Save();
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

        public void RemoveItem(Item item)
        {
            _items.Remove(item);
            Save();
        }

        public Room Exit(string keyword)
        {
            foreach (Exit exit in _exits)
            {
                if (exit.Keyword == keyword)
                {
                    if (exit.IsLocked) return this;
                    Save();
                    return new Room(exit.Destination);
                }
            }
            return this;
        }

        private void Save()
        {
            var data = new XElement("root", new XElement("description", _description));
            data = AddExitsToXElement(data);
            data = AddItemsToXElement(data);
            data.Save(_filePath);
        }

        private XElement AddExitsToXElement(XElement xElement)
        {
            foreach (Exit exit in _exits)
            {
                xElement.Add(new XElement("exit",
                    new XElement("keyword", exit.Keyword),
                    new XElement("description", exit.Description),
                    new XElement("destination", exit.Destination)
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
                    new XElement("detailedDescription", item.DetailedDescription)
                    ));
            }

            return xElement;
        }
    }
}
