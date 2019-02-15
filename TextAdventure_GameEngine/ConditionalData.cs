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

        public bool MeetsSkipConditions(CharacterDataBlock dataBlock)
        {
            if (string.IsNullOrWhiteSpace(Condition)) return false;

            bool allConditionsMet = true;

            var conditions = Condition.Split('+');

            foreach (var condition in conditions)
            {
                var conditionWords = condition.Trim().Split(' ');

                switch (conditionWords[0])
                {
                    case "isInParty":
                        allConditionsMet = IsInParty(conditionWords[1]);
                        break;
                    case "!isInParty":
                        allConditionsMet = !IsInParty(conditionWords[1]);
                        break;
                    case "numVisitsGreaterThan":
                        allConditionsMet = NumVisitsGreaterThan(conditionWords[1], dataBlock);
                        break;
                    case "playerHas":
                        allConditionsMet = PlayerHas(conditionWords[1]);
                        break;
                    case "!playerHas":
                        allConditionsMet = !PlayerHas(conditionWords[1]);
                        break;
                    default:
                        allConditionsMet = false;
                        break;
                }

                if (!allConditionsMet) return allConditionsMet;
            }

            return allConditionsMet;
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
