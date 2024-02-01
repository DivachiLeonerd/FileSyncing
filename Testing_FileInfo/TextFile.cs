using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_task
{
    internal class TextFile
    {

        public static bool DirectoriesAreEqual(DirectoryInfo directory1, DirectoryInfo directory2)
        {
            Console.WriteLine(directory1.FullName + ' ' + directory2.FullName);
            if (directory1.LastWriteTime != directory2.LastWriteTime)
                return false;
            if (GetDirectoryFileSize(directory1) != GetDirectoryFileSize(directory2))
                return false;
            return true;
        }

        public static long GetDirectoryFileSize(DirectoryInfo directory)
        {
            IEnumerable<FileInfo> files = directory.EnumerateFiles();
            IEnumerable<DirectoryInfo> subfolders = directory.EnumerateDirectories();
            long dirSize = 0;

            foreach (FileInfo file in files)
            {
                dirSize += file.Length;
            }
            return dirSize;
        }

        public static void CreatingDifferingFiles(DirectoryInfo sourceDir, DirectoryInfo targetDir)
        {
            int i = 0;
            int smallerCount;
            var sourceFiles = sourceDir.GetFiles();
            var targetFiles = targetDir.GetFiles();
            int comparisonResult;

            comparisonResult = TextFile.CompareFileCount(ref sourceDir, ref targetDir, out smallerCount);
            while (i < smallerCount)
            {
                if (!TextFile.CompareFilesContent(sourceFiles.ElementAt(i), targetFiles.ElementAt(i)))
                {
                    ErrorHandling.LogError($"Copying... {targetFiles.ElementAt(i).FullName}");
                    FileCopy(sourceFiles.ElementAt(i), targetDir);
                }
                else if (sourceFiles.ElementAt(i).Name != targetFiles.ElementAt(i).Name)
                {
                    File.Move(targetFiles.ElementAt(i).FullName, targetFiles.ElementAt(i).FullName + ".temp");
                    File.Move(targetFiles.ElementAt(i).FullName + ".temp", targetDir.FullName + '\\' + sourceFiles.ElementAt(i).Name);
                }
                i++;
            }
            while (i < sourceFiles.Count())
            {
                Console.WriteLine(targetDir.FullName + "\\" + sourceFiles.ElementAt(i).Name);
                sourceFiles.ElementAt(i).CopyTo(targetDir.FullName + "\\" + sourceFiles.ElementAt(i).Name, true);
                i++;
            }
        }

        public static void DeleteDifferingFiles(ref DirectoryInfo sourceDir, ref DirectoryInfo targetDir)
        {
            int i = 0;
            var sourceFiles = sourceDir.EnumerateFiles();
            var targetFiles = targetDir.EnumerateFiles();
            int smallerCount;
            int comparisonResult;

            comparisonResult = TextFile.CompareFileCount(ref sourceDir, ref targetDir, out smallerCount);
            while (i < smallerCount)
            {
                if (!TextFile.CompareFilesContent(sourceFiles.ElementAt(i), targetFiles.ElementAt(i)))
                {
                    targetFiles.ElementAt(i).Delete();
                }
                i++;
            }
            if (comparisonResult < 0)
            {
                while (i < targetFiles.Count())
                {
                    targetFiles.ElementAt(i).Delete();
                    i++;
                }
            }
        }

        public static int CompareFileCount(ref DirectoryInfo dir1, ref DirectoryInfo dir2)
        {
            int dir1Count;
            int dir2Count;

            if (dir1 == null || dir2 == null)
                return 0;
            dir1Count = dir1.EnumerateDirectories().Count();
            dir2Count = dir2.EnumerateDirectories().Count();

            if (dir1Count < dir2Count)
                return (dir1Count);
            return (dir2Count);
        }

        public static int CompareFileCount(ref DirectoryInfo dir1, ref DirectoryInfo dir2, out int smallerCount)
        {
            int dir1Count;
            int dir2Count;

            if (dir1 == null || dir2 == null)
            {
                smallerCount = 0;
                return 0;
            }
            dir1Count = dir1.EnumerateFiles().Count();
            dir2Count = dir2.EnumerateFiles().Count();

            if (dir1Count < dir2Count)
            {
                smallerCount = dir1Count;
                return (-1);
            }
            smallerCount = dir2Count;
            return (1);
        }

        public static void CreateDifferingDirectories(ref DirectoryInfo source, ref DirectoryInfo target)
        {
            IEnumerable<DirectoryInfo> sourceDirectories = source.EnumerateDirectories();
            IEnumerable<DirectoryInfo> targetDirectories = target.EnumerateDirectories();
            int i = 0;
            int smallerCount;
            int comparisonResult = TextFile.CompareDirectoryCount(sourceDirectories, targetDirectories, out smallerCount);
            int sourceDirectoryCount;

            while (i < smallerCount)
            {
                if (!Directory.Exists(target.FullName + '\\' + sourceDirectories.ElementAt(i).Name))
                {
                    if (TextFile.DirectoriesAreEqual(sourceDirectories.ElementAt(i), targetDirectories.ElementAt(i)))
                    {
                        ErrorHandling.LogError($"Copying {sourceDirectories.ElementAt(i).FullName} to {target.FullName + '\\' + sourceDirectories.ElementAt(i).Name}");
                        Directory.Move(targetDirectories.ElementAt(i).FullName, target.FullName + '\\' + sourceDirectories.ElementAt(i).Name + "temp");
                        Directory.Move(target.FullName + '\\' + sourceDirectories.ElementAt(i).Name + "temp", target.FullName + '\\' +sourceDirectories.ElementAt(i).Name);
                    }
                    else
                    {
                        ErrorHandling.LogError($"Adding {sourceDirectories.ElementAt(i).FullName} to {target.FullName + '\\' + sourceDirectories.ElementAt(i).Name}");
                        Directory.CreateDirectory(target.FullName + '\\' + sourceDirectories.ElementAt(i).Name);
                    }
                }
                i++;
            }
            if (comparisonResult > 0)
            {
                sourceDirectoryCount = sourceDirectories.Count();
                while (i < sourceDirectoryCount)
                {
                    Directory.SetLastWriteTime(Directory.CreateDirectory(target.FullName + '\\' + sourceDirectories.ElementAt(i).Name).FullName, sourceDirectories.ElementAt(i).LastWriteTime);
                    i++;
                }
            }
        }

        public static void DeleteDirectory(DirectoryInfo dirInfo, bool recursive)
        {
            foreach (var file in dirInfo.EnumerateFiles())
            {
                file.Delete();
            }
            Console.WriteLine(dirInfo.FullName);
            foreach (var subfolder in dirInfo.EnumerateDirectories())
            {
                if (recursive)
                    DeleteDirectory(subfolder, recursive);
                else
                    try { subfolder.Delete(); } catch (Exception e) { ErrorHandling.LogError(e.Message); }
            }
            dirInfo.Delete();
        }

        public static void DeleteDifferingDirectories(IEnumerable<DirectoryInfo> sourceDirectories, IEnumerable<DirectoryInfo> targetDirectories)
        {
            int i = 0;
            int smallerCount;
            int comparisonResult;
            int aux;

            comparisonResult = TextFile.CompareDirectoryCount(sourceDirectories, targetDirectories, out smallerCount);
            while (i < smallerCount)
            {
                if (!TextFile.DirectoriesAreEqual(sourceDirectories.ElementAt(i), targetDirectories.ElementAt(i)))
                {
                    while (i < smallerCount)
                    {
                        Console.WriteLine(targetDirectories.ElementAt(i));
                        DeleteDirectory(targetDirectories.ElementAt(i), true);
                        smallerCount--;
                    }
                }
                i++;
            }
            if (comparisonResult > 0)
                return;
            aux = targetDirectories.Count();
            while (i < aux)
            {
                Console.WriteLine(targetDirectories.ElementAt(i).FullName);
                DeleteDirectory(targetDirectories.ElementAt(i), true);
            }
        }

        public static int CompareDirectoryCount(IEnumerable<DirectoryInfo> dir1, IEnumerable<DirectoryInfo> dir2, out int smallerCount)
        {
            int dir1Count;
            int dir2Count;

            if (dir1 == null || dir2 == null)
            {
                smallerCount = 0;
                return 0;
            }
            dir1Count = dir1.Count();
            dir2Count = dir2.Count();

            if (dir1Count < dir2Count)
            {
                smallerCount = dir1Count;
                return (-1);
            }
            smallerCount = dir2Count;
            return (1);
        }

        //It should compare if two files have the same content
        public static bool CompareFilesContent(FileInfo source, FileInfo target)
        {
            if (!source.Exists || !target.Exists)
                return false;
            if (source.Length != target.Length)
                return false;
            if (source.LastWriteTime != target.LastWriteTime)
                return false;
            return (CompareFileStreams(source, target));
        }

        static bool CompareFileStreams(FileInfo source, FileInfo target)
        {
            byte[] sourceBuffer = new byte[1024 * 1024 * 10];
            byte[] targetBuffer = new byte[1024 * 1024 * 10];
            int i = 0;
            int bytes;

            using (FileStream sourceStream = source.OpenRead(), targetStream = target.OpenRead())
            {
                while ((bytes = sourceStream.Read(sourceBuffer, 0, sourceBuffer.Length)) > 0 && targetStream.Read(targetBuffer, 0, sourceBuffer.Length) > 0)
                {
                    while (i < bytes && sourceBuffer[i] == targetBuffer[i])
                        i++;
                    if (i != bytes)
                        return false;
                }
                return true;
            }
        }

        static void FileCopy(FileInfo sourceFile, DirectoryInfo targetDir)
        {
            FileInfo newFile = sourceFile.CopyTo(targetDir.FullName + '\\' + sourceFile.Name, true);
            newFile.LastWriteTime = sourceFile.LastWriteTime;
            ErrorHandling.LogError($"Copying {sourceFile} to {targetDir.Name}");
        }



    }
}
