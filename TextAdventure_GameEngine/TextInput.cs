namespace TextAdventure_GameEngine
{
    public class TextInput
    {
        private GameController _gameController;

        public TextInput(GameController gameController)
        {
            _gameController = gameController;
        }

        public void Accept(string input)
        {
            input = input.ToLower();
            string[] inputWords = input.Split(' ');

            foreach (UserAction availableAction in _gameController.AvailableActions)
                if (availableAction.Keyword == inputWords[0])
                    availableAction.Respond(_gameController, inputWords);
        }
    }
}
