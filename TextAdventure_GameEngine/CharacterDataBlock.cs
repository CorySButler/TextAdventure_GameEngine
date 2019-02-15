using System.Collections.Generic;

namespace TextAdventure_GameEngine
{
    public class CharacterDataBlock
    {
        public string Id { get; set; }
        public int NumVisits { get; set; }
        public int CurrentDescription { get; set; }
        public int CurrentDetailedDescription { get; set; }
        public int CurrentDialogue { get; set; }
        public List<ConditionalData> Descriptions { get; set; }
        public List<ConditionalData> DetailedDescriptions { get; set; }
        public List<ConditionalData> Dialogues { get; set; }
    }
}
