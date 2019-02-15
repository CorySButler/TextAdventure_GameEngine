namespace TextAdventure_GameEngine
{
    public class Add : UserAction
    {
        public override string Keyword { get { return "add"; } }
        public override void Respond(GameController gameController, string[] inputWords)
        {
            var keyword = inputWords.Length > 1 ? inputWords[1] : "";
            gameController.Add(keyword);
        }
    }
}
