namespace TextAdventure_GameEngine
{
    public class Exit : InteractableObject
    {
        public string Destination { get; set; }
        public string OnGo { get; set; }
        public bool IsLocked { get; set; }
    }
}
