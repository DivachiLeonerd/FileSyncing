﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_task
{
    internal class ErrorHandling:Exception
    {
        //Log itself using Console and log file
        public static void LogError(Exception e)
        {
            Console.WriteLine(e.Message);
            File.AppendAllText("Mylog", e.Message);
        }

        private static void CleanLog(string logPath)
        {
            Console.WriteLine("Cleaning the file...");
            System.IO.File.WriteAllText(logPath, string.Empty);
        }
    }

}