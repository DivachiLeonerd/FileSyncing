using System.Collections.Immutable;
using System.Diagnostics;
using Test_task;

namespace Test_fileInfo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //ThreadCreation updateDirThread = new ThreadCreation(1000000, "./Source", "./Target", UpdateDirectories);
            //ThreadCreation userInputThread = new ThreadCreation();
            ErrorHandling.LogPath = "./MyLog";
            ErrorHandling.CleanLog();
            Console.WriteLine("Cleaning Done! Starting program execution...");
            Update(10000000, "./Source", "./Target");
        }

        static void Update(long syncInterval, string sourceDir, string targetDir)
        {
            long lastUpdateTime;

            lastUpdateTime = DateTime.Now.Ticks;
            try
            {
                while (true)
                {
                    if (DateTime.Now.Ticks - lastUpdateTime > syncInterval)
                    {
                         UpdateDirectories(syncInterval, sourceDir, targetDir);
                        lastUpdateTime = DateTime.Now.Ticks;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }


        static void UpdateDirectories(long syncInterval, string sourceDir, string targetDir)
        {
            
            DirectoryInfo source = new DirectoryInfo(sourceDir);
            DirectoryInfo target = new DirectoryInfo(targetDir);
            IEnumerable<DirectoryInfo> sourceDirectories = source.EnumerateDirectories();

            Console.WriteLine("Time to update!");
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
            var sourceFiles = sourceDir.EnumerateFiles();
            var targetFiles = targetDir.EnumerateFiles();

            Console.WriteLine("Just entered in UpdateFiles");
            TextFile.DeleteDifferingFiles(ref sourceDir, ref targetDir);
            TextFile.DeleteDifferingDirectories(sourceDir.EnumerateDirectories().ToImmutableArray(), targetDir.EnumerateDirectories());

            TextFile.CreateDifferingDirectories(ref sourceDir, ref targetDir);
            TextFile.CreatingDifferingFiles(sourceDir, targetDir);
            return;
        }


    }
}
