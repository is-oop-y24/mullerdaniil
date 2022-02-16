using System;

namespace Reports.Server.Tools
{
    public class ReportsException : Exception
    {
        public ReportsException()
        {
        }

        public ReportsException(string message)
            : base(message)
        {
        }

        public ReportsException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}