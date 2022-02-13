namespace BackupsExtra.Loggers
{
    public interface ILogger
    {
        void Log(string message);
        void EnableTimeCodePrefix(bool value);
    }
}