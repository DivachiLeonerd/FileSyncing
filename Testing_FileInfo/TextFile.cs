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
            dir1Count = dir1.EnumerateDirectories().Count();
            dir2Count = dir2.EnumerateDirectories().Count();

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
    }
}
