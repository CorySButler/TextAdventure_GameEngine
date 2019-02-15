namespace TextAdventure_GameEngine
{
    public class Destroy : UserAction
    {
        public override string Keyword { get { return "destroy"; } }
        public override void Respond(GameController gameController, string[] inputWords)
        {
            var keyword = inputWords.Length > 1 ? inputWords[1] : "";
            gameController.Destroy(keyword);
        }
    }
}
