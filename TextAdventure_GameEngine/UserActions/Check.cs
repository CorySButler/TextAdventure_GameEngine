namespace TextAdventure_GameEngine
{
    public class Check : UserAction
    {
        public override string Keyword { get { return "check"; } }
        public override void Respond(GameController gameController, string[] inputWords)
        {
            var keyword = inputWords.Length > 1 ? inputWords[1] : "";
            gameController.Check(keyword);
        }
    }
}
