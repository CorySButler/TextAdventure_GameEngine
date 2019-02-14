using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace TextAdventure_GameEngine
{
    public class Character : InteractableObject
    {
        public string DisplayName { get; private set; }
        public string NeedsItemId { get; set; }
        private List<CharacterDataBlock> _dataBlocks;

        public Character(string keyword)
        {
            Keyword = keyword;

            if (!File.Exists("Characters//" + Keyword + ".xml")) return;

            var characterData = XElement.Load("Characters//" + Keyword + ".xml");
            
            DisplayName = characterData.Element("displayName").Value;
            NeedsItemId = characterData.Element("needsItemId").Value;

            _dataBlocks = (from dataBlock in characterData.Elements("dataBlock")
                          select new CharacterDataBlock
                          {
                              Id = dataBlock.Element("id").Value,
                              CurrentDescription = int.Parse(dataBlock.Element("currentDescription").Value),
                              CurrentDetailedDescription = int.Parse(dataBlock.Element("currentDetailedDescription").Value),
                              CurrentDialogue = int.Parse(dataBlock.Element("currentDialogue").Value),

                              Descriptions = (from description in dataBlock.Elements("description")
                                              select description.Value).ToList(),

                              DetailedDescriptions = (from detailedDescription in dataBlock.Elements("detailedDescription")
                                              select detailedDescription.Value).ToList(),

                              Dialogues = (from dialogue in dataBlock.Elements("dialogue")
                                              select dialogue.Value).ToList()

                          }).ToList();
        }

        public void IncDetailedDescription(Room room)
        {
            try
            {
                var i = _dataBlocks.IndexOf(_dataBlocks.Where(db => db.Id == room.Id).ToList()[0]);
                _dataBlocks[i].CurrentDetailedDescription++;

                if (_dataBlocks[i].CurrentDetailedDescription == _dataBlocks[i].DetailedDescriptions.Count)
                    _dataBlocks[i].CurrentDetailedDescription--;
            }
            catch { /* NOOP */ }
        }

        public void IncDescription(Room room)
        {
            try
            {
                var i = _dataBlocks.IndexOf(_dataBlocks.Where(db => db.Id == room.Id).ToList()[0]);
                _dataBlocks[i].CurrentDescription++;

                if (_dataBlocks[i].CurrentDescription == _dataBlocks[i].Descriptions.Count)
                    _dataBlocks[i].CurrentDescription--;
            }
            catch { /* NOOP */ }
        }

        public void IncDialogue(Room room)
        {
            try
            {
                var i = _dataBlocks.IndexOf(_dataBlocks.Where(db => db.Id == room.Id).ToList()[0]);
                _dataBlocks[i].CurrentDialogue++;

                if (_dataBlocks[i].CurrentDialogue == _dataBlocks[i].Dialogues.Count)
                    _dataBlocks[i].CurrentDialogue--;
            }
            catch { /* NOOP */ }
        }

        public void Save()
        {
            var data = new XElement("root",
                new XElement("displayName", DisplayName));

            foreach (var dataBlock in _dataBlocks)
            {
                var block = new XElement("dataBlock",
                    new XElement("id", dataBlock.Id),
                    new XElement("currentDescription", dataBlock.CurrentDescription),
                    new XElement("currentDetailedDescription", dataBlock.CurrentDetailedDescription),
                    new XElement("currentDialogue", dataBlock.CurrentDialogue));

                foreach (var description in dataBlock.Descriptions)
                    block.Add(new XElement("description", description));

                foreach (var detailedDescription in dataBlock.DetailedDescriptions)
                    block.Add(new XElement("detailedDescription", detailedDescription));

                foreach (var dialogue in dataBlock.Dialogues)
                    block.Add(new XElement("dialogue", dialogue));

                data.Add(block);
            }

            data.Save("Characters\\" + Keyword + ".xml");
        }

        public string Describe(Room room)
        {
            try
            {
                var i = _dataBlocks.IndexOf(_dataBlocks.First(db => db.Id == room.Id));
                return _dataBlocks[i].Descriptions[_dataBlocks[i].CurrentDescription];
            }
            catch
            {
                return "";
            }
        }

        public string DescribeDetails(Room room)
        {
            try
            {
                var i = _dataBlocks.IndexOf(_dataBlocks.First(db => db.Id == room.Id));
                return _dataBlocks[i].DetailedDescriptions[_dataBlocks[i].CurrentDetailedDescription];
            }
            catch
            {
                return "";
            }
        }

        public string Talk(Room room)
        {
            try
            {
                var i = _dataBlocks.IndexOf(_dataBlocks.First(db => db.Id == room.Id));
                return DisplayName + ": " + _dataBlocks[i].Dialogues[_dataBlocks[i].CurrentDialogue];
            }
            catch
            {
                return DisplayName + " has nothing to say.";
            }
        }
    }
}
