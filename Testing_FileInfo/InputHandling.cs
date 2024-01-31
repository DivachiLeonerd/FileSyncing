using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection.Metadata.Ecma335;

namespace Test_task
{
    internal class InputHandling
    {
        internal static string GetNonNullInput(string? lineRead)
        {
            while (lineRead == null || lineRead[0] == '\0')
            {
                Console.WriteLine("Hello There! It seems you gave me a null string which I REALLY shouldn't use. Could you give me another one?");
                lineRead = Console.ReadLine();
            }
            return (lineRead);
        }

        internal static string GetExistingFPath(string? path)
        {
            string  verifiedPath = GetNonNullInput(path);
            string  ?createNow;
            DirectoryInfo dir;
            try
            {
                dir = new DirectoryInfo(verifiedPath.Trim());
                while (!Directory.Exists(dir.FullName))
                {
                    
                    Console.WriteLine($"{verifiedPath} isnt a viable folder path. Do you want to create a new folder? Y/N Default:No");
                    Console.WriteLine("Or input \"exit\" to Exit");
                    createNow = Console.ReadLine();
                    if (createNow != null)
                    {
                        if (createNow.ToLower() == "exit")
                            Environment.Exit(0);
                        if (createNow.ToLower() == "y" || createNow.ToLower() == "yes")
                        {
                            dir = Directory.CreateDirectory(verifiedPath);
                            break ;
                        }
                    }
                    Console.WriteLine("Specify Existing Folder name:");
                    verifiedPath = GetNonNullInput(Console.ReadLine());
                }
                return (dir.FullName);
            }
            catch (Exception e) { ErrorHandling.LogError(e.Message); }
            return (GetExistingFPath(verifiedPath));
        }

        internal static int GetSyncInterval(string? syncArg)
        {
            int syncInterval;

            try
            {
                if (int.TryParse(syncArg, out syncInterval))
                    return syncInterval;
                else
                    throw new Exception("A problem ocurred while parsing Sync Times");
            }
            catch (Exception e) { ErrorHandling.LogError(e.Message); }
            Environment.Exit(0);
            return (-1);
        }
    }


}
