using System.Security.Cryptography;
using System.IO;
using System.Diagnostics.Contracts;
using System.Reflection.Metadata.Ecma335;

namespace Test_task
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string folderPath;
            int syncInterval;
            string logPath;
            long lastUpdate = DateTime.Now.Ticks;

            Console.WriteLine("Welcome to my Test task!");
            Thread.Sleep(1000);
            Console.WriteLine("This is a project about syncronization between a source file and a replica");

            logPath = InputHandling.GetExistingFPath(args[3]);
            folderPath = InputHandling.GetExistingFPath(args[1]);
            syncInterval = InputHandling.GetSyncInterval(args[2]);

            Console.WriteLine("If you wish to stop executing write \"exit\" to Exit.");
            
        }
    }
}
