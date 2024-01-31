using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_task
{
    internal class ErrorHandling:Exception
    {
        //Log itself using Console and log file
        private static string logPath;
        public static FileInfo log;
        public static void LogError(string errorMessage)
        {
            Console.WriteLine(errorMessage);
            File.AppendAllText("MyLog", errorMessage + "\n\n");
            
        }
        public static void CleanLog()
        {
            Console.WriteLine("Cleaning the file...");
            System.IO.File.WriteAllText(LogPath, string.Empty);
        }

        public static string LogPath
        {
            get { return (logPath); }
            set
            {
                try
                {
                    log = new FileInfo(value);
                    logPath = value;
                }
                catch (Exception e){ Console.WriteLine(e.Message); }
            }
        }
    }

}