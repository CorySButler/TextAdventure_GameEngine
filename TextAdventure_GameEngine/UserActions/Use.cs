namespace TextAdventure_GameEngine
{
    public class Use : UserAction
    {
        public override string Keyword { get { return "use"; } }
        public override void Respond(GameController gameController, string[] inputWords)
        {
            var keyword = inputWords.Length > 1 ? inputWords[1] : "";
            var targetKeyword = inputWords.Length > 2 ? inputWords[2] : "";
            gameController.Use(keyword, targetKeyword);
        }
    }
}
