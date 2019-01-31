﻿namespace TextAdventure_GameEngine
{
    public class Drop : UserAction
    {
        public override string Keyword { get { return "drop"; } }
        public override void Respond(GameController gameController, string[] inputWords)
        {
            gameController.Drop(inputWords[1]);
        }
    }
}
