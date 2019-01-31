namespace TextAdventure_GameEngine
{
    public class Inventory : UserAction
    {
        public override string Keyword { get { return "inventory"; } }
        public override void Respond(GameController gameController, string[] inputWords)
        {
            gameController.Inventory();
        }
    }
}
