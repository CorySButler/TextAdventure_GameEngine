namespace TextAdventure_GameEngine
{
    public class Check : UserAction
    {
        public override string Keyword { get { return "check"; } }
        public override void Respond(GameController gameController, string[] inputWords)
        {
            gameController.Check(inputWords[1]);
        }
    }
}
