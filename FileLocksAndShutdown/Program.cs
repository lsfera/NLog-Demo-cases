using System;
using System.IO;
using System.Runtime.InteropServices;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace FileLocksAndShutdown
{
    class Program
    {
        static void Main(string[] args)
        {
            const string fileName = "C:/temp/logs.txt";

            var config = CreateLogConfig(fileName);
            LogManager.Configuration = config;

            Console.WriteLine("Write logs");
            var myLogger = LogManager.GetLogger("myLogger");
            myLogger.Info("Hi from my logger!");

            LogManager.Flush();// flush to check if really locked. You could disable this and it still works.

            WriteConsoleFileExistsAndLockInfo(fileName);

            Console.WriteLine("Shutdown NLog");
            LogManager.Shutdown(); //logs get flushed and written to the file

            WriteConsoleFileExistsAndLockInfo(fileName);
            Console.WriteLine($"File content: {File.ReadAllText(fileName)}");
            Console.WriteLine($"Full path: {new FileInfo(fileName).FullName}");
            Console.WriteLine("Deleting file ...");
            File.Delete(fileName);
            Console.WriteLine("Deleted file");
            WriteConsoleFileExistsAndLockInfo(fileName);
        }

        private static void WriteConsoleFileExistsAndLockInfo(string fileName)
        {
            var exists = File.Exists(fileName);
            var locked = IsFileLocked(fileName);
            Console.WriteLine($"File exists: {exists}. File locked: {locked}");
        }

        private static LoggingConfiguration CreateLogConfig(string fileName)
        {
            var fileTarget = new FileTarget()
            {
                FileName = fileName,
                //   Layout = <string layout>,
                KeepFileOpen = true,
                ArchiveAboveSize = 50000000,
                ArchiveEvery = FileArchivePeriod.Day,
                ArchiveNumbering = ArchiveNumberingMode.DateAndSequence,
                //   ArchiveDateFormat = DatePattern
            };

            var asyncFileTarget =
                new NLog.Targets.Wrappers.AsyncTargetWrapper(fileTarget, 10000, NLog.Targets.Wrappers.AsyncTargetWrapperOverflowAction.Block);
            asyncFileTarget.Name = "async_target";

            var config = new LoggingConfiguration();
            config.AddTarget(asyncFileTarget);
            config.AddRule(NLog.LogLevel.Debug, NLog.LogLevel.Fatal, asyncFileTarget);
            return config;
        }

        public static bool IsFileLocked(string filePath)
        {
            try
            {
                using (File.Open(filePath, FileMode.Open)) { }
            }
            catch (IOException e)
            {
                var errorCode = Marshal.GetHRForException(e) & ((1 << 16) - 1);

                return errorCode == 32 || errorCode == 33;
            }

            return false;
        }
    }
}
