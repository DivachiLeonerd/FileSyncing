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
        public ThreadCreation(DirectoryInfo sourceDir, DirectoryInfo targetDir, Action <DirectoryInfo, DirectoryInfo> UpdateReplicaFolder) 
        {
            workerThread = new Thread(() => UpdateReplicaFolder(sourceDir, targetDir));
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
    }
}
