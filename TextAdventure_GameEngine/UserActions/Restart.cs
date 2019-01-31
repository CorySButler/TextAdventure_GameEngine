namespace TextAdventure_GameEngine
{
    public class Restart : UserAction
    {
        public override string Keyword { get { return "restart"; } }
        public override void Respond(GameController gameController, string[] inputWords)
        {
            gameController.Restart();
        }
    }
}
