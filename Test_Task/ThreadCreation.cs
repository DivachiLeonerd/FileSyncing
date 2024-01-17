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
        private Thread inputThread;
        private Thread updateThread;
        public ThreadCreation(string folderPath, string logPath, long syncTime) 
        {
            inputThread = new Thread(() => WaitUserInput());
            updateThread = new Thread(() => UpdateReplicaFolder(folderPath, logPath, syncTime));
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
                }
                    if (rawInput != null)
                    {
                        finalInput = rawInput;
                        if (finalInput.ToLower() == "exit")
                            Environment.Exit(0);
                    }
            }
        }
        public void StartThreads()
        {
            inputThread.Start();
            updateThread.Start();   
        }

        private void UpdateReplicaFolder(string folderPath, string logPath, long syncTime)
        {
            
        }

    }
}
