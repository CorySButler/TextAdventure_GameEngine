using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TextAdventure_GameEngine
{
    class Room
    {
        private string _description;
        private List<Exit> _exits;
        private List<string> _items;

        public Room(string filePath)
        {
            var data = XElement.Load(filePath);
            _description = data.Element("description").Value;
            _exits = (from exit in data.Elements("exit")
                     select new Exit
                     {
                         Direction = exit.Element("direction").Value,
                         Description = exit.Element("description").Value,
                         Destination = exit.Element("destination").Value
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

            return combinedText;
        }

        public bool HasExit(string direction)
        {
            foreach (Exit exit in _exits)
            {
                if (exit.Direction == direction) return true;
            }
            return false;
        }

        public Room Exit(string direction)
        {
            Save();
            foreach (Exit exit in _exits)
            {
                if (exit.Direction == direction) return new Room(exit.Destination);
            }
            return this;
        }

        private void Save()
        {

        }
    }
}
