using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETB.Logging
{
    public delegate void LogEvent(string message, params object[] args);
    public class Logger : ILogger
    {
        private Logger()
        {
            LogInfo = new LogEvent((x, y) => { });
            LogWarn = new LogEvent((x, y) => { });
            LogError = new LogEvent((x, y) => { });
            LogFatal = new LogEvent((x, y) => { });
        }
        private static readonly Logger _instance = new Logger();
        public event LogEvent LogInfo;
        public event LogEvent LogWarn;
        public event LogEvent LogError;
        public event LogEvent LogFatal;

        public static Logger Instance
        {
            get
            {
                return _instance;
            }
        }
        public void Error(string message, params object[] args)
        {
            LogError(message, args);
        }

        public void Fatal(string message, params object[] args)
        {
            LogFatal(message, args);
        }

        public void Info(string message, params object[] args)
        {
            LogInfo(message, args);
        }

        public void Warn(string message, params object[] args)
        {
            LogWarn(message, args);
        }
    }
}
