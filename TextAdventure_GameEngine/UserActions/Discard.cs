﻿namespace TextAdventure_GameEngine
{
    public class Discard : UserAction
    {
        public override string Keyword { get { return "discard"; } }
        public override void Respond(GameController gameController, string[] inputWords)
        {
            var keyword = inputWords.Length > 1 ? inputWords[1] : "";
            gameController.Discard(keyword);
        }
    }
}
