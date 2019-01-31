namespace TextAdventure_GameEngine
{
    public class Take : UserAction
    {
        public override string Keyword { get { return "take"; } }
        public override void Respond(GameController gameController, string[] inputWords)
        {
            gameController.Take(inputWords[1]);
        }
    }
}
