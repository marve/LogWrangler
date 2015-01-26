using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Humanizer;

namespace LogWrangler.LogGenerator
{
    public class Program
    {
        public static readonly string GENERATED_FILE = "generated.log";
        private static Random _random = new Random();

        static void Main(string[] args)
        {
            string[] types = new[] { "debug", "info", "warn", "error" };

            Task.Run(async () =>
            {
                try
                {
                    while (true)
                    {
                        int typeIndex = _random.Next(types.Length - 1);
                        int sentenceIndex = _random.Next(0);
                        string words = MakeSentence();
                        string message = string.Format("{0} [{1}]: {2}", DateTime.Now, types[typeIndex].ToUpper(), words);
                        File.AppendAllLines(GENERATED_FILE, new[] { message });
                        await Task.Delay(1.Milliseconds());
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            });
            Console.WriteLine("Press any key to stop.");
            Console.ReadLine();
        }

        private static string MakeWord()
        {
            int startingChar = (int)'a';
            int endingChar = (int)'z';
            int wordLength = _random.Next(1, 12);
            StringBuilder word = new StringBuilder(wordLength);
            for (int charIndex = 0; charIndex < wordLength; ++charIndex)
            {
                char character = (char)(_random.Next(startingChar, endingChar));
                word.Append((char)character);
            }
            return word.ToString();
        }

        private static string MakeSentence()
        {
            int words = _random.Next(4, 25);
            string sentence = string.Join(" ", Enumerable.Range(0, words).Select(i => MakeWord())) + ".";
            return sentence.ToString();
        }
    }
}
