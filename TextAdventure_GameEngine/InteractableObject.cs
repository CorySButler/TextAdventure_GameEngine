namespace TextAdventure_GameEngine
{
    public abstract class InteractableObject
    {
        public string Keyword { get; set; }
        public string Description { get; set; }
        public string OnCheck { get; set; }
        public string WantsItemId { get; set; }
        public string OnUse { get; set; }
    }
}
