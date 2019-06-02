using System;
using NLog;

namespace ClassLibrary1
{
    public class Class1
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        /// <inheritdoc />
        public Class1()
        {
            Logger.Info("Init class1");
        }
    }
}
