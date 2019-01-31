namespace TextAdventure_GameEngine
{
    public class Go : UserAction
    {
        public override string Keyword { get { return "go"; } }
        public override void Respond(GameController gameController, string[] inputWords)
        {
            gameController.Go(inputWords[1]);
        }
    }
}
