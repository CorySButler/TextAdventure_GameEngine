using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

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
            if (string.IsNullOrWhiteSpace(str)) return;

            var forcedLines = str.Split('\n');
            List<string> words = new List<string>();
            foreach (var forcedLine in forcedLines)
            {
                var wordsInForcedLine = forcedLine.Split(' ');
                foreach (var word in wordsInForcedLine)
                    words.Add(word);
                if (words[words.Count - 1] != "\n" && !words[words.Count - 1].EndsWith("\n"))
                    words.Add("\n");
            }

            var sb = new StringBuilder();

            var lineLength = 80;
            var lines = new List<string>();
            var currLine = "";

            foreach (var word in words)
            {
                if (currLine.Length + word.Length + 1 > lineLength || word == "\n")
                {
                    lines.Add(currLine.Trim());
                    currLine = "";
                }
                
                if (word != "\n")
                    currLine += word;

                currLine += " ";
            }
            if (!string.IsNullOrWhiteSpace(currLine))
                lines.Add(currLine.Trim());

            
            Console.ForegroundColor = ConsoleColor.White;
            foreach (var line in lines)
            {
                Console.Write("\n");
                foreach (var letter in line)
                {
                    if (letter == '#')
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                    }
                    else if (letter == '@')
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    }
                    else if (letter == '^')
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.Write(letter);
                        Thread.Sleep(5);
                    }
                }
            }
            
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
