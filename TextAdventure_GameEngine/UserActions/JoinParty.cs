namespace TextAdventure_GameEngine
{
    public class JoinParty : UserAction
    {
        public override string Keyword { get { return "joinparty"; } }
        public override void Respond(GameController gameController, string[] inputWords)
        {
            var keyword = inputWords.Length > 1 ? inputWords[1] : "";
            gameController.JoinParty(keyword);
        }
    }
}
