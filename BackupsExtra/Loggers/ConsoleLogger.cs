using System;
using System.Globalization;

namespace BackupsExtra.Loggers
{
    public class ConsoleLogger : ILogger
    {
        private bool _timeCodePrefixEnabled;
        public void Log(string message)
        {
            if (_timeCodePrefixEnabled)
            {
                Console.WriteLine(DateTime.Now.ToString(CultureInfo.InvariantCulture) + " " + message);
            }

            Console.WriteLine(message);
        }

        public void EnableTimeCodePrefix(bool value)
        {
            _timeCodePrefixEnabled = value;
        }
    }
}