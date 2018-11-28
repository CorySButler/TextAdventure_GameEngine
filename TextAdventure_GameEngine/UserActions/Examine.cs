namespace TextAdventure_GameEngine
{
    public class Examine : UserAction
    {
        public override string Keyword { get { return "examine"; } }
        public override void Respond(GameController gameController, string[] inputWords)
        {
            gameController.ExamineItem(inputWords[1]);
        }
    }
}
