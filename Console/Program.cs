using System;
using ClassLibrary1;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace Console_with_class_library
{
    class Program
    {
        static void Main(string[] args)
        {
            var class1 = new Class1();
            Console.WriteLine("Press any key ...");
            Console.ReadLine();
        }
    }
}
