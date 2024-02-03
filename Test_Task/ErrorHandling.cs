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
        public static void LogMessage(int messageType, string filename)
        {
            switch (messageType)
            {
                case 0: Console.WriteLine($"Creating '{filename}' in target repo");
                        File.AppendAllText(logPath, $"Creating {filename}"); break;

                case 1: Console.WriteLine($"Deleting '{filename}' in target repo");
                        File.AppendAllText(logPath, $"Deleting {filename}"); break;

                case 2: Console.WriteLine($"Copying '{filename}'  in target repo");
                        File.AppendAllText(logPath, $"Copying '{filename}'"); break;

                default: Console.WriteLine(logPath, "This is not a valid messageType");break;
            }
            Console.WriteLine('\\');
            File.AppendAllText(logPath, "\n\n");
        }

        public static void LogMessage(Exception ex)
        {
            Console.WriteLine($"[ERROR]: {ex.StackTrace}\n\t---> {ex.Message}");
            File.AppendAllText(logPath, $"[ERROR]: {ex.StackTrace}\n\t---> {ex.Message}");
            Console.WriteLine('\\');
            File.AppendAllText(logPath, "\n\n");
        }

        public static void CleanLog()
        {
            Console.WriteLine("Cleaning the log for new run cycle...");
            File.WriteAllText(LogPath, string.Empty);
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
                catch (Exception ex){ Console.WriteLine(ex.Message); throw; }
            }
        }
    }

}