using LogReader.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Humanizer;

namespace LogReader
{
    public class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                using (FileStream fileStream = new FileStream(Settings.Default.LogFile, FileMode.Open, 
                    FileAccess.Read, FileShare.ReadWrite, 0x1000, FileOptions.SequentialScan))
                using (StreamReader reader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    while (true)
                    {
                        string line = await reader.ReadLineAsync();
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            Console.WriteLine(line);
                        }
                        await Task.Delay(10.Milliseconds());
                    }
                }
            });
            Console.WriteLine("Press any key to quit.");
            Console.ReadLine();
        }
    }
}
