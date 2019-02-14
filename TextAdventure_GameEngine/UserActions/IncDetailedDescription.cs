namespace TextAdventure_GameEngine
{
    public class IncDetailedDescription : UserAction
    {
        public override string Keyword { get { return "incdetaileddescription"; } }
        public override void Respond(GameController gameController, string[] inputWords)
        {
            var keyword = inputWords.Length > 1 ? inputWords[1] : "";
            gameController.IncDetailedDescription(keyword);
        }
    }
}
