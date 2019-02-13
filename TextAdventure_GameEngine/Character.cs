using System.Collections.Generic;

namespace TextAdventure_GameEngine
{
    public class Character : InteractableObject
    {
        public string OnTalk { get; set; }
        public int CurrentDialogue { get; set; }
        public List<string> Dialogues { get; set; }
    }
}
