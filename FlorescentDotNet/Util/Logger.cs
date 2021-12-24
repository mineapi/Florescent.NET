using System.Globalization;

namespace FlorescentDotNet.Util;

public class Logger
{
    public static List<String> LogCache = new List<string>();

    public static void Log(String value)
    {
        string timestamp =
            String.Format("[{0}:{1}:{2}]", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        string log = String.Format("{0} {1}", timestamp, value);
        
        Console.WriteLine(log);
        LogCache.Add(log);
    }

    public static void WriteLogCacheToFile()
    {
        string fileName = String.Format("{0}{1}{2} {3}{4}{5}.txt", DateTime.Now.Year, DateTime.Now.Month,
            DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        
        File.WriteAllLines(fileName, LogCache.ToArray());
    }
}