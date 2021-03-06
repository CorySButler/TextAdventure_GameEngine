﻿namespace TextAdventure_GameEngine
{
    public class Take : UserAction
    {
        public override string Keyword { get { return "take"; } }
        public override void Respond(GameController gameController, string[] inputWords)
        {
            var keyword = inputWords.Length > 1 ? inputWords[1] : "";
            gameController.Take(keyword);
        }
    }
}
