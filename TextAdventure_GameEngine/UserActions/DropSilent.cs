namespace TextAdventure_GameEngine
{
    public class DropSilent : UserAction
    {
        public override string Keyword { get { return "dropsilent"; } }
        public override void Respond(GameController gameController, string[] inputWords)
        {
            gameController.DropSilent(inputWords[1]);
        }
    }
}
