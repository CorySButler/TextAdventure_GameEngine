using System;
using System.IO;

namespace TextAdventure_GameEngine
{
    public class GameLog
    {
        private string _logPath;

        public GameLog()
        {
            if (!Directory.Exists("GameLogs")) Directory.CreateDirectory("GameLogs");

            int i = 0;
            while (File.Exists("GameLogs\\PlayThrough_" + i + ".txt")) i++;

            _logPath = "GameLogs\\PlayThrough_" + i + ".txt";
        }

        public void Write(string str)
        {
            str = "\n" + str + "\n";
            Console.WriteLine(str);
            
            Save(str);
        }

        public void WriteSilent(string str)
        {
            str = "\n" + str + "\n";            
            Save(str);
        }

        private void Save(string str)
        {
            if (!Directory.Exists("GameLogs")) Directory.CreateDirectory("GameLogs");

            File.AppendAllText(_logPath, str);
        }
    }
}
