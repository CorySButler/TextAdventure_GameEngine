namespace TextAdventure_GameEngine
{
    public class Describe : UserAction
    {
        public override string Keyword { get { return "describe"; } }
        public override void Respond(GameController gameController, string[] inputWords)
        {
            var keyword = inputWords.Length > 1 ? inputWords[1] : "";
            gameController.Describe(keyword);
        }
    }
}
