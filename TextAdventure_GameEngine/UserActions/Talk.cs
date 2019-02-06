namespace TextAdventure_GameEngine
{
    public class Talk : UserAction
    {
        public override string Keyword { get { return "talk"; } }
        public override void Respond(GameController gameController, string[] inputWords)
        {
            var keyword = inputWords.Length > 1 ? inputWords[1] : "";
            gameController.Talk(keyword);
        }
    }
}
