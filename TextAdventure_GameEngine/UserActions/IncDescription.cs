namespace TextAdventure_GameEngine
{
    public class IncDescription : UserAction
    {
        public override string Keyword { get { return "incdescription"; } }
        public override void Respond(GameController gameController, string[] inputWords)
        {
            var keyword = inputWords.Length > 1 ? inputWords[1] : "";
            gameController.IncDescription(keyword);
        }
    }
}
