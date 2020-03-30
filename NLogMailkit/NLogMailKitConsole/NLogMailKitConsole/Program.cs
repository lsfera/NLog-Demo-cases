using System;
using NLog;

namespace NLogMailKitConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = LogManager.GetLogger("logger1");
            logger.Error("Hi from main()");
            LogManager.Shutdown();
            Console.WriteLine("Log written");
        }
    }
}
