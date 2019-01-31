namespace TextAdventure_GameEngine
{
    public class Talk : UserAction
    {
        public override string Keyword { get { return "talk"; } }
        public override void Respond(GameController gameController, string[] inputWords)
        {
            gameController.Talk(inputWords[1]);
        }
    }
}
