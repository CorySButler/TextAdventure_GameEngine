namespace TextAdventure_GameEngine
{
    public abstract class UserAction
    {
        public abstract string Keyword { get; }
        public abstract void Respond(GameController gameController, string[] inputWords);
    }
}
