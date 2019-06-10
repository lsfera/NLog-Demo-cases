using System;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace ClassLibrary1
{
    public class Class1
    {


        /// <inheritdoc />
        public Class1()
        {
            // setup config
            var configuration = new LoggingConfiguration();
            configuration.AddRuleForAllLevels(new ConsoleTarget());
            LogManager.Configuration = configuration;

            // log
            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("Init class1");
        }
    }
}
