public class LoggingSession
{
    public string PortName { get; set; }
    public string FilePath { get; set; }

    public LoggingSession(string portName, string filePath)
    {
        PortName = portName;
        FilePath = filePath;
    }
}
