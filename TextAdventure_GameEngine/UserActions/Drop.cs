namespace TextAdventure_GameEngine
{
    public class Drop : UserAction
    {
        public override string Keyword { get { return "drop"; } }
        public override void Respond(GameController gameController, string[] inputWords)
        {
            var keyword = inputWords.Length > 1 ? inputWords[1] : "";
            gameController.Drop(keyword);
        }
    }
}
