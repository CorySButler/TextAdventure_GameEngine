namespace TextAdventure_GameEngine
{
    public class Discard : UserAction
    {
        public override string Keyword { get { return "discard"; } }
        public override void Respond(GameController gameController, string[] inputWords)
        {
            gameController.Discard(inputWords[1]);
        }
    }
}
