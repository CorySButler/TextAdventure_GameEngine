using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;

namespace TextAdventure_GameEngine
{
    public enum Genders { MALE, FEMALE };

    public class Player
    {
        public string Name;
        public Genders Gender;
        public int Gold;
        public List<Character> Party = new List<Character>();
        private List<Item> _items = new List<Item> { new Item
                     {
                         Keyword = "crowns",
                         Description = "12",
                         OnCheck = "12",
                         Id = "00",
                         OnTake = "",
                         OnUse = "",
                         WantsItemId = ""
                     }, new Item
                     {
                         Keyword = "florens",
                         Description = "2",
                         OnCheck = "2",
                         Id = "00",
                         OnTake = "",
                         OnUse = "",
                         WantsItemId = ""
                     }, new Item
                     {
                         Keyword = "orens",
                         Description = "1",
                         OnCheck = "1",
                         Id = "00",
                         OnTake = "",
                         OnUse = "",
                         WantsItemId = ""
                     }, new Item
                     {
                         Keyword = "steelblade",
                         Description = "",
                         OnCheck = "Best for thwarting men and elves.",
                         Id = "2",
                         OnTake = "",
                         OnUse = "",
                         WantsItemId = ""
                     }, new Item
                     {
                         Keyword = "silverblade",
                         Description = "",
                         OnCheck = "Ideal for slaying monsters.",
                         Id = "3",
                         OnTake = "",
                         OnUse = "",
                         WantsItemId = ""
                     }};
        private string _location;

        public List<Item> Items { get { return _items; } private set { _items = value; } }

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

        public void Save()
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
                    new XElement("onCheck", item.OnCheck),
                    new XElement("id", item.Id)
                    ));
            }

            return xElement;
        }

        public Player Load()
        {
            var playerData = XElement.Load("player.xml");

            var player = new Player
            {
                Name = playerData.Element("name").Value,
                Gender = playerData.Element("gender").Value.ToLower() == "male" ? Genders.MALE : Genders.FEMALE,
                Gold = int.Parse(playerData.Element("gold").Value),
                Items = (from item in playerData.Elements("item")
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
            };

            return player;
        }
    }
}
