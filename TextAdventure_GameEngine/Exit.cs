namespace TextAdventure_GameEngine
{
    public class Exit : InteractableObject
    {
        public string Destination { get; set; }
        public bool IsLocked { get; set; }
    }
}
