namespace TextAdventure_GameEngine
{
    public class LeaveParty : UserAction
    {
        public override string Keyword { get { return "leaveparty"; } }
        public override void Respond(GameController gameController, string[] inputWords)
        {
            var keyword = inputWords.Length > 1 ? inputWords[1] : "";
            gameController.LeaveParty(keyword);
        }
    }
}
