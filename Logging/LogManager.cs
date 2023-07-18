namespace Logging
{
    public class LogManager
    {
        public static ILog GetLogger(Type type)
        {
            return new Logger();
        }
    }
}