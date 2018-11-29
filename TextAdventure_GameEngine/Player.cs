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
        private List<Item> _items = new List<Item>();
        private string _location;

        public List<Item> Items { get { return _items; } }

        public void AddItem(Item item)
        {
            _items.Add(item);
            Save();
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
                    new XElement("detailedDescription", item.DetailedDescription),
                    new XElement("isTakeable", item.IsTakeable.ToString().ToLower())
                    ));
            }

            return xElement;
        }
    }
}
