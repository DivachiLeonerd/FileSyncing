using System.Security.Cryptography;
using System.IO;
using System.Diagnostics.Contracts;
using System.Reflection.Metadata.Ecma335;
using System.Collections.Immutable;

namespace Test_task
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string folderPath;
            int syncInterval;
            string targetPath;

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
            ErrorHandling.LogPath = args[3].Trim();

            
            Console.WriteLine("If you wish to stop executing write \"exit\" to Exit.");
            ErrorHandling.CleanLog();
            UpdateReplicaFolder(syncInterval, folderPath, targetPath);
            //if ReplicaPath is invalid OR empty then just copy Source

            //Update Replica and keep Source and Replica content and access data (new files/ file renames, file edit timestamps, ...)
            //If neither replica nor source have changes then nothing needs updating

            //if, for example, a file was accessed and it's timestamp is different, then we need to update
            Thread.CurrentThread.Join();
            return ;
        }

        static void UpdateReplicaFolder(long syncInterval, string source, string replica)
        {
            ThreadCreation updateThread;
            ThreadCreation userInputThread;

            updateThread = new ThreadCreation(syncInterval, source, replica, Update);
            userInputThread = new ThreadCreation();

            userInputThread.StartThread();
            updateThread.StartThread();

            updateThread.waitForThread();
            userInputThread.waitForThread();
            return ;
        }

        static void Update(long syncInterval, string sourceDir, string targetDir)
        {
            long lastUpdateTime = DateTime.Now.Ticks;

            while (true)
            {
                if (DateTime.Now.Ticks - lastUpdateTime > syncInterval)
                {
                    try
                    {
                        UpdateDirectories(syncInterval, sourceDir, targetDir);
                        lastUpdateTime = DateTime.Now.Ticks;
                    }
                    catch (Exception ex)
                    {
                        ErrorHandling.LogMessage(ex);
                        Update(syncInterval, sourceDir, targetDir);
                    }
                }
            }
        }

        static void UpdateDirectories(long syncInterval, string sourceDir, string targetDir)
        {

            DirectoryInfo source = new DirectoryInfo(sourceDir);
            DirectoryInfo target = new DirectoryInfo(targetDir);
            IEnumerable<DirectoryInfo> sourceDirectories = source.EnumerateDirectories();
            UpdateFiles(source, target);
            foreach (var subFolder in sourceDirectories)
            {
                Console.WriteLine(subFolder.FullName);
                UpdateDirectories(syncInterval, subFolder.FullName, targetDir + '\\' + subFolder.Name);
            }
            Directory.SetLastWriteTime(target.FullName, source.LastWriteTime);
        }

        static void UpdateFiles(DirectoryInfo sourceDir, DirectoryInfo targetDir)
        {
            TextFile.DeleteDifferingFiles(ref sourceDir, ref targetDir);
            TextFile.DeleteDifferingDirectories(sourceDir.EnumerateDirectories().ToImmutableArray(), targetDir.EnumerateDirectories());
            TextFile.CreateDifferingDirectories(ref sourceDir, ref targetDir);
            TextFile.CreatingDifferingFiles(sourceDir, targetDir);
            return;
        }


    }
}
