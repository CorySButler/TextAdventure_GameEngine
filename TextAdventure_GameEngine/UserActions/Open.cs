namespace TextAdventure_GameEngine
{
    public class Open : UserAction
    {
        public override string Keyword { get { return "open"; } }
        public override void Respond(GameController gameController, string[] inputWords)
        {
            var keyword = inputWords.Length > 1 ? inputWords[1] : "";
            gameController.Open(keyword);
        }
    }
}
