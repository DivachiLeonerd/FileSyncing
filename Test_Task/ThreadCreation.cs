using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_task
{
    internal class ThreadCreation
    {
        private Thread workerThread;
        public ThreadCreation(long syncInterval, string sourceDir, string targetDir, Action <long, string, string> UpdateDirectories) 
        {
            workerThread = new Thread(() => UpdateDirectories(syncInterval, sourceDir, targetDir));
        }

        public ThreadCreation()
        {
            workerThread = new Thread(() => WaitUserInput());
        }

        private void WaitUserInput()
        {
            string? rawInput;
            string finalInput;

            rawInput = null;
            while (true)
            {
                while (rawInput == null)
                {
                    rawInput = Console.ReadLine();
                    Console.WriteLine("Exiting...");
                }
                if (rawInput != null)
                {
                    finalInput = rawInput;
                    if (finalInput.ToLower() == "exit")
                        Environment.Exit(0);
                }
            }
        }
        public void StartThread()
        {
            workerThread.Start();
        }

        public void waitForThread()
        {
            this.workerThread.Join();
        }
    }
}
