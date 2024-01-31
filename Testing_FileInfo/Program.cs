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
            Update(100000000, "./Source", "./Target");
        }

        static void Update(long syncInterval, string sourceDir, string targetDir)
        {
            long lastUpdateTime;

            lastUpdateTime = DateTime.Now.Ticks;
            try
            {
                while (true)
                {
                  //  if (DateTime.Now.Ticks - lastUpdateTime > syncInterval)
                    //{
                        UpdateDirectories(syncInterval, sourceDir, targetDir);
                        lastUpdateTime = DateTime.Now.Ticks;
                    //}
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        static void DeleteDirectory(DirectoryInfo dirInfo, bool recursive)
        {
            foreach (var file in  dirInfo.EnumerateFiles())
            {
                file.Delete();
            }
            Console.WriteLine(dirInfo.FullName);
            foreach (var subfolder in  dirInfo.EnumerateDirectories())
            {
                if (recursive)
                    DeleteDirectory(subfolder, recursive);
                else
                    try { subfolder.Delete(); } catch (Exception e) { ErrorHandling.LogError(e.Message); }
            }
            dirInfo.Delete();
        }
        static void UpdateDirectories(long syncInterval, string sourceDir, string targetDir)
        {
            DirectoryInfo source = new DirectoryInfo(sourceDir);
            DirectoryInfo target = new DirectoryInfo(targetDir);
            IEnumerable<DirectoryInfo> sourceDirectories = source.EnumerateDirectories();

            Console.WriteLine("Time to update!");
            UpdateFiles(source, target);
            if (!sourceDirectories.Any())
                return;

            DeleteDifferentDirectories(ref source, ref target);
            CreatingDifferingDirectories(ref source, ref target);
            foreach (var subFolder in sourceDirectories)
            {
                Console.WriteLine(subFolder.FullName);
                UpdateDirectories(syncInterval, subFolder.FullName, targetDir + '\\' + subFolder.Name);
            }
        }

            //Deletes all Directories that are different from source
        static void DeleteDifferentDirectories(ref DirectoryInfo source, ref DirectoryInfo target)
        {
            IEnumerable<DirectoryInfo> sourceDirectories = source.EnumerateDirectories();
            IEnumerable<DirectoryInfo> targetDirectories = source.EnumerateDirectories();

            int i = 0;
            int smallerCount;
            int comparisonResult;
            int aux;

            comparisonResult = TextFile.CompareFileCount(ref source, ref target, out smallerCount);
            while (i < smallerCount)
            {
                if (!TextFile.DirectoriesAreEqual(sourceDirectories.ElementAt(i), targetDirectories.ElementAt(i)))
                {
                    DeleteDirectory(targetDirectories.ElementAt(i), true);
                }
                i++;
            }
            if (comparisonResult > 0)
                return;
            aux = targetDirectories.Count();
            while (i < aux)
                targetDirectories.ElementAt(i).Delete();
        }

         //Copies (or renames existing ones) all Directories from source
        static void CreatingDifferingDirectories(ref DirectoryInfo source, ref DirectoryInfo target)
        {
            IEnumerable<DirectoryInfo> sourceDirectories = source.EnumerateDirectories();
            IEnumerable<DirectoryInfo> targetDirectories = target.EnumerateDirectories();
            int i = 0;
            int smallerCount;
            int comparisonResult = TextFile.CompareFileCount(ref source, ref target, out smallerCount);
            int sourceFileCount;

            while (i < smallerCount)
            {
                if (!Directory.Exists(target.FullName + '\\' + sourceDirectories.ElementAt(i).Name))
                {
                    if (TextFile.DirectoriesAreEqual(sourceDirectories.ElementAt(i), targetDirectories.ElementAt(i)))
                    {
                        ErrorHandling.LogError($"Copying {sourceDirectories.ElementAt(i).FullName} to {target.FullName + '\\' + sourceDirectories.ElementAt(i).Name}");
                        Directory.Move(targetDirectories.ElementAt(i).FullName, target.FullName + '\\' + sourceDirectories.ElementAt(i).Name + "temp");
                        Directory.Move(target.FullName + '\\' + sourceDirectories.ElementAt(i).Name + "temp", target.FullName + sourceDirectories.ElementAt(i).Name);
                    }
                    else
                    {
                        ErrorHandling.LogError($"Adding {sourceDirectories.ElementAt(i).FullName} to {target.FullName + '\\' + sourceDirectories.ElementAt(i).Name}");
                        Directory.CreateDirectory(target.FullName + '\\' + sourceDirectories.ElementAt(i).Name);
                        Directory.SetLastWriteTime(targetDirectories.ElementAt(i).FullName, sourceDirectories.ElementAt(i).LastWriteTime);
                    }
                }
                i++;
            }
            if (comparisonResult > 0)
            {
                sourceFileCount = sourceDirectories.Count();
                while (i < sourceFileCount)
                {
                    Directory.SetLastWriteTime(Directory.CreateDirectory(target.FullName + '\\' + sourceDirectories.ElementAt(i).Name).FullName, sourceDirectories.ElementAt(i).LastWriteTime);
                    i++;
                }
            }
        }
        static void FileCopy(FileInfo sourceFile, DirectoryInfo targetDir)
        {

            FileInfo newFile = sourceFile.CopyTo(targetDir.FullName + '\\' + sourceFile.Name, true);
            newFile.LastWriteTime = sourceFile.LastWriteTime;
            ErrorHandling.LogError($"Copying {sourceFile} to {targetDir.Name}");
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
                if (!TextFile.CompareFilesContent(sourceFiles.ElementAt(i), targetFiles.ElementAt(i)))
                {
                    ErrorHandling.LogError($"Copying... {targetFiles.ElementAt(i).FullName}");
                    FileCopy(sourceFiles.ElementAt(i), targetDir);
                }
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
                    Console.WriteLine("Am I entering this at all?");
                    Console.WriteLine(targetDir.FullName + "\\" + sourceFiles.ElementAt(i).Name);
                    sourceFiles.ElementAt(i).CopyTo(targetDir.FullName + "\\" + sourceFiles.ElementAt(i).Name, true);
                    i++;
                }
            }
            return;
        }
        }
    }
