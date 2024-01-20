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
            string targetPath;
            DirectoryInfo sourceDir;
            DirectoryInfo targetDir;

            Console.WriteLine("Welcome to my Test task!");
            //Thread.Sleep(1000);
            Console.WriteLine("This is a project about syncronization between a source folder and a replica");
            if (args.Length != 4) {
                Console.WriteLine(args.Length);
                Console.WriteLine("Not enough arguments! Try the following syntax:" +
                    "Test_task.exe [sourceFolderPath] [targetFolderPath] [syncTime] [logPath]");
                Console.WriteLine("Press Enter to Exit");
                Console.ReadLine();
                return ;
            }
            folderPath = InputHandling.GetExistingFPath(args[0]);
            targetPath = InputHandling.GetExistingFPath(args[1]);
            syncInterval = InputHandling.GetSyncInterval(args[2]);
            logPath = InputHandling.GetExistingFPath(args[3]);

            Console.WriteLine("If you wish to stop executing write \"exit\" to Exit.");
            sourceDir = new DirectoryInfo(folderPath);
            targetDir = new DirectoryInfo(targetPath);

           //if ReplicaPath is invalid OR empty then just copy Source
            
           //Update Replica and keep Source and Replica content and access data (new files/ file renames, file edit timestamps, ...)
           //If neither replica nor source have changes then nothing needs updating

            //if, for example, a file was accessed and it's timestamp is different, then we need to update
            return ;
        }

        static void UpdateReplicaFolder(DirectoryInfo source, DirectoryInfo replica)
        {
            Console.WriteLine("Updating replica folder");
            if (!source.Exists)
            {
                if (replica.Exists)
                {
                    replica.Delete(true);
                    ErrorHandling.LogError("Deleted Replica");
                }
                return ;
            }
            if (!replica.Exists)
            {
                DirCopy(source, replica);
            }
            else if (File.Equals(replica.EnumerateFiles(), source.EnumerateFiles()))
                return;
            else
            {
                
            }


            return;
        }

        static bool NeedsUpdate(DirectoryInfo sourceDir, DirectoryInfo targetDir)
        {
            return (Directory.Equals(sourceDir.EnumerateFiles(), targetDir.EnumerateFiles())) ;
        }

        static void UpdateFiles(IEnumerable<FileInfo> sourceDir, IEnumerable<FileInfo> targetDir)
        {
            int i = 0;
            int smallerCount;
            if (sourceDir.Count() <= targetDir.Count())
                smallerCount = sourceDir.Count();
            else
                smallerCount = targetDir.Count();
            while (i < smallerCount)
            {

            }
        }

        static void UpdateDirectories(string sourceDir, string targetDir)
        {
            DirectoryInfo source = new DirectoryInfo(sourceDir);
            DirectoryInfo target = new DirectoryInfo(targetDir);
            var sourceFiles = source.EnumerateFiles();
            var targetFiles = target.EnumerateFiles();

            UpdateFiles(sourceFiles, targetFiles);
            foreach (var currentDirectory in source.EnumerateDirectories())
            {
                Console.WriteLine(currentDirectory.FullName);
                UpdateDirectories(currentDirectory.FullName, targetDir + currentDirectory.Name);
            }
        }
        static void StartThreadWork(long syncInterval, DirectoryInfo sourceDir, DirectoryInfo targetDir)
        {
            long lastUpdate = DateTime.Now.Ticks;
            ThreadCreation updateThread;
            ThreadCreation userInputThread;

            userInputThread = new ThreadCreation();
            updateThread = new ThreadCreation(sourceDir, targetDir, UpdateReplicaFolder);
            while (true)
            {
                userInputThread.StartThread();
                while(DateTime.Now.Ticks - lastUpdate > syncInterval)
                {
                    updateThread.StartThread();
                }
            }
        }

        static void DirCopy(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

        }
    }
}
