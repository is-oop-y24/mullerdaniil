using System;
using System.Globalization;
using System.IO;
using BackupsExtra.Tools;

namespace BackupsExtra.Loggers
{
    public class FileLogger : ILogger
    {
        private bool _timeCodePrefixEnabled;
        private string _loggerPath;

        public FileLogger(string loggerPath)
        {
            _loggerPath = loggerPath;
            if (!File.Exists(loggerPath))
            {
                File.Create(loggerPath);
            }
        }

        public void Log(string message)
        {
            if (!File.Exists(_loggerPath))
            {
                throw new BackupsExtraException("Unable to find the logger");
            }

            if (_timeCodePrefixEnabled)
            {
                File.AppendAllText(_loggerPath, DateTime.Now.ToString(CultureInfo.InvariantCulture) + " " + message);
            }
            else
            {
                File.AppendAllText(_loggerPath, message);
            }
        }

        public void EnableTimeCodePrefix(bool value)
        {
            _timeCodePrefixEnabled = value;
        }
    }
}