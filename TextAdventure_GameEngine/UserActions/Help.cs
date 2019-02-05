namespace TextAdventure_GameEngine
{
    public class Help : UserAction
    {
        public override string Keyword { get { return "help"; } }
        public override void Respond(GameController gameController, string[] inputWords)
        {
            gameController.Help();
        }
    }
}
