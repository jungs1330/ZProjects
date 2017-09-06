using System;
using System.Collections.Generic;
using log4net;

namespace ZUtility
{
    public enum Category
    {
        Error,
        Info,
        Warn,
        Debug
    }

    public class Log4NetLogger
    {
        protected static readonly ILog Logger = LogManager.GetLogger(typeof(Log4NetLogger));
        private static readonly Log4NetLogger instance = new Log4NetLogger();
        public static Log4NetLogger GetInstance { get { return instance; } }

        static Log4NetLogger()
        {
            log4net.Config.XmlConfigurator.Configure();
        }


        private Log4NetLogger() { }

        public void Log(string message, Category category)
        {
            switch (category)
            {
                case Category.Debug:
                    Debug(message);
                    break;
                case Category.Error:
                    Error(message);
                    break;
                case Category.Info:
                    Info(message);
                    break;
                case Category.Warn:
                    Warn(message);
                    break;
            }
        }
        public void Info(string message)
        {
            Logger.Info(message);
        }

        public void Debug(string message)
        {
            Logger.Debug(message);
        }

        public void Error(string message)
        {
            Logger.Error(message);
        }

        public void Error(Exception exception)
        {
            Logger.Error(exception.Message);
        }

        public void Warn(string message)
        {
            Logger.Warn(message);
        }
    }
}
