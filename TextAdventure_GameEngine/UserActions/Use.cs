namespace TextAdventure_GameEngine
{
    public class Use : UserAction
    {
        public override string Keyword { get { return "use"; } }
        public override void Respond(GameController gameController, string[] inputWords)
        {
            var targetKeyword = "";
            if (inputWords.Length >= 3)
                targetKeyword = inputWords[2];
            gameController.UseItem(inputWords[1], targetKeyword);
        }
    }
}
