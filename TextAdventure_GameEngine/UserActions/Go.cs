namespace TextAdventure_GameEngine
{
    public class Go : UserAction
    {
        public override string Keyword { get { return "go"; } }
        public override void Respond(GameController gameController, string[] inputWords)
        {
            var keyword = inputWords.Length > 1 ? inputWords[1] : "";
            gameController.Go(keyword);
        }
    }
}
