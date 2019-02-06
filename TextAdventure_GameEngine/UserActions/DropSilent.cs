namespace TextAdventure_GameEngine
{
    public class DropSilent : UserAction
    {
        public override string Keyword { get { return "dropsilent"; } }
        public override void Respond(GameController gameController, string[] inputWords)
        {
            var keyword = inputWords.Length > 1 ? inputWords[1] : "";
            gameController.DropSilent(keyword);
        }
    }
}
