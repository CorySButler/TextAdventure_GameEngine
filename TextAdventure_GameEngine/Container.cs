using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure_GameEngine
{
    public class Container : InteractableObject
    {
        public List<Item> Inventory { get; set; }

        public string ListInventory()
        {
            var sb = new StringBuilder();
            int i;
            for (i = 0; i < Inventory.Count -1; i++)
            {
                sb.Append(Inventory[i].Description + ", ");
                if (i == Inventory.Count - 2) sb.Append("and ");
            }
            sb.Append(Inventory[i].Description + ".");
            return sb.ToString();
        }

        public void AddItem(Item item)
        {
            Inventory.Add(item);
        }

        public Item GetItem(string keyword)
        {
            foreach (Item item in Inventory)
            {
                if (item.Keyword == keyword) return item;
            }
            return null;
        }

        public bool HasItem(string keyword)
        {
            foreach (var item in Inventory)
            {
                if (item.Keyword == keyword)
                    return true;
            }
            return false;
        }

        public void RemoveItem(Item item)
        {
            Inventory.Remove(item);
        }
    }
}
