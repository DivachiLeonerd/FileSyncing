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
            string filePath;
            int syncInterval;
            string logPath;

            Console.WriteLine("Welcome to my Test task!");
            Thread.Sleep(1000);
            Console.WriteLine("This is a project about syncronization between a source file and a replica");
            Thread.Sleep(1000);
            logPath = InputHandling.GetExistingFPath(args[3]);
            filePath = InputHandling.GetExistingFPath(args[1]);
            syncInterval = InputHandling.GetSyncInterval(args[2]);
        }
    }
}
