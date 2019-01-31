using System.Collections.Generic;
using System.Xml.Linq;

namespace TextAdventure_GameEngine
{
    public enum Genders { MALE, FEMALE };

    public class Player
    {
        public string Name;
        public Genders Gender;
        public int Gold;
        private List<Item> _items = new List<Item> { new Item
                     {
                         Keyword = "florens",
                         Description = "2",
                         DetailedDescription = "2",
                         Id = "00"
                     }, new Item
                     {
                         Keyword = "orens",
                         Description = "1",
                         DetailedDescription = "1",
                         Id = "00"
                     }, new Item
                     {
                         Keyword = "steelblade",
                         Description = "",
                         DetailedDescription = "Best for thwarting men and elves.",
                         Id = "2"
                     }, new Item
                     {
                         Keyword = "silverblade",
                         Description = "",
                         DetailedDescription = "Ideal for slaying monsters.",
                         Id = "3"
                     }};
        private string _location;

        public List<Item> Items { get { return _items; } }

        public void AddItem(Item item)
        {
            _items.Add(item);
            Save();
        }

        public bool HasItem(string keyword)
        {
            foreach (Item item in _items)
            {
                if (item.Keyword == keyword) return true;
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

        public void RemoveItem(Item item)
        {
            _items.Remove(item);
            Save();
        }

        public void UpdateLocation(Room room)
        {
            _location = room.FilePath;
            Save();
        }

        private void Save()
        {
            var data = new XElement("root",
                new XElement("name", Name),
                new XElement("gender", Gender),
                new XElement("gold", Gold),
                new XElement("location", _location));
            data = AddItemsToXElement(data);
            data.Save("player.xml");
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
