namespace TextAdventure_GameEngine
{
    public class ConditionalData
    {
        public string Condition { get; private set; }
        public string Data { get; private set; }

        public ConditionalData(string condiction, string data)
        {
            Condition = condiction;
            Data = data;
        }

        public bool MeetsSkipCondition(CharacterDataBlock dataBlock)
        {
            if (string.IsNullOrWhiteSpace(Condition)) return false;

            var conditionWords = Condition.Split(' ');

            switch(conditionWords[0])
            {
                case "isInParty":
                    return IsInParty(conditionWords[1]);
                case "numVisitsGreaterThan":
                    return NumVisitsGreaterThan(conditionWords[1], dataBlock);
                case "playerHas":
                    return PlayerHas(conditionWords[1]);
                default:
                    return false;
            }
        }

        private bool IsInParty(string keyword)
        {
            var player = new Player().Load();
            return player.Party.Contains(new Character(keyword));
        }

        private bool NumVisitsGreaterThan(string number, CharacterDataBlock dataBlock)
        {
            int n = int.Parse(number);
            return dataBlock.NumVisits > n;
        }

        private bool PlayerHas(string keyword)
        {
            Player player = new Player().Load();
            return (player.HasItem(keyword));
        }
    }
}
