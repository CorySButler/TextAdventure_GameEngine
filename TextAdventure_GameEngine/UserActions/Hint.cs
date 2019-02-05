namespace TextAdventure_GameEngine
{
    public class Hint : UserAction
    {
        public override string Keyword { get { return "hint"; } }
        public override void Respond(GameController gameController, string[] inputWords)
        {
            gameController.Hint();
        }
    }
}
