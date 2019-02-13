﻿namespace TextAdventure_GameEngine
{
    public class IncDialogue : UserAction
    {
        public override string Keyword { get { return "incdialogue"; } }
        public override void Respond(GameController gameController, string[] inputWords)
        {
            var keyword = inputWords.Length > 1 ? inputWords[1] : "";
            gameController.IncDialogue(keyword);
        }
    }
}
