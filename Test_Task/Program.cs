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

            updateThread = new ThreadCreation(syncInterval, source, replica, UpdateDirectories);
            userInputThread = new ThreadCreation();

            userInputThread.StartThread();
            updateThread.StartThread();

            updateThread.waitForThread();
            userInputThread.waitForThread();
            return ;
        }
        static void UpdateDirectories(long syncInterval, string sourceDir, string targetDir)
        {
            long lastUpdate = DateTime.Now.Ticks;
            DirectoryInfo source = new DirectoryInfo(sourceDir);
            DirectoryInfo target = new DirectoryInfo(targetDir);
            try
            {
                while (true)
                {
                    if (DateTime.Now.Ticks - lastUpdate > syncInterval)
                    {
                        Console.WriteLine("Time to update!");
                        UpdateFiles(source, target);
                        foreach (var currentDirectory in source.EnumerateDirectories())
                        {
                            Console.WriteLine(currentDirectory.FullName);
                            UpdateDirectories(syncInterval, currentDirectory.FullName, targetDir + currentDirectory.Name);
                        }
                        lastUpdate = DateTime.Now.Ticks;
                    }
                    
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("An error has occured: " + e.Message);
            }
        }

        static void UpdateFiles(DirectoryInfo sourceDir, DirectoryInfo targetDir)
        {
            var sourceFiles = sourceDir.EnumerateFiles();
            var targetFiles = targetDir.EnumerateFiles();
            bool targetIsBigger = false;

            int i = 0;
            int smallerCount;
            Console.WriteLine("Just entered in UpdateFiles");
            if (sourceFiles.Count() <= targetFiles.Count())
            {
                smallerCount = sourceFiles.Count();
                targetIsBigger = true;
            }
            else
                smallerCount = targetFiles.Count();
            while (i < smallerCount)
            {
                if (!sourceFiles.ElementAt(i).Equals(targetFiles.ElementAt(i)))
                    FileCopy(sourceFiles.ElementAt(i), targetDir);
                i++;
            }
            if (targetIsBigger)
            {
                while (i < targetFiles.Count())
                {
                    targetFiles.ElementAt(i).Delete();
                    i++;
                }
            }
            else
            {
                while (i < sourceFiles.Count())
                {
                    sourceFiles.ElementAt(i).CopyTo(targetDir.FullName + sourceFiles.ElementAt(i).Name, true);
                    i++;
                }
            }
            return ;
        }

        static void FileCopy(FileInfo sourceFile, DirectoryInfo targetDir)
        {
            sourceFile.CopyTo(targetDir.FullName + sourceFile.Name, true);
            ErrorHandling.LogError($"Copying {sourceFile} to {targetDir.Name}");
        }
        static void DirCopy(DirectoryInfo sourceSubFolder, DirectoryInfo target)
        {
            string newTargetName;
            DirectoryInfo targetSubFolder;

            newTargetName = target.FullName + sourceSubFolder.Name;
            targetSubFolder = Directory.CreateDirectory(newTargetName);
            UpdateFiles(sourceSubFolder, targetSubFolder);
        }
    }
}
