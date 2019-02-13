using System.Collections.Generic;
using System.Xml.Linq;

namespace TextAdventure_GameEngine
{
    public class Character : InteractableObject
    {
        public string DisplayName { get; set; }
        public string OnTalk { get; set; }
        public int CurrentDialogue { get; set; }
        public int CurrentDescription { get; set; }
        public int CurrentDetailedDescription { get; set; }
        public List<string> Dialogues { get; set; }
        public List<string> Descriptions { get; set; }
        public List<string> DetailedDescriptions { get; set; }

        public void Save()
        {
            var data = new XElement("root",
                new XElement("displayName", DisplayName),
                new XElement("currentDialogue", CurrentDialogue),
                new XElement("currentDescription", CurrentDescription),
                new XElement("currentDetailedDescription", CurrentDetailedDescription));

            foreach (var dialogue in Dialogues)
            {
                data.LastNode.AddAfterSelf(new XElement("dialogue", dialogue));
            }

            foreach (var descriptions in Descriptions)
            {
                data.LastNode.AddAfterSelf(new XElement("descriptions", descriptions));
            }

            foreach (var detailedDescriptions in DetailedDescriptions)
            {
                data.LastNode.AddAfterSelf(new XElement("detailedDescriptions", detailedDescriptions));
            }

            data.Save("Characters\\" + Keyword + ".xml");
        }
    }
}
