namespace TextAdventure_GameEngine
{
    public class TextInput
    {
        public void Accept(string input, GameController gameController)
        {
            input = input.ToLower();
            string[] inputWords = input.Split(' ');

            UserAction userAction = null;

            foreach (UserAction availableAction in gameController.AvailableActions)
                if (availableAction.Keyword == inputWords[0]) userAction = availableAction;

            if (userAction != null)
            {
                userAction.Respond(gameController, inputWords);
            }
            else
            {
                System.Console.WriteLine("\n\"{0}\" is not an available action.\n", inputWords[0]);
            }
        }
    }
}
