namespace FlorescentDotNet.Util;

public class GuildSettings
{
    public Dictionary<string, string> Settings { get; set; }

    public GuildSettings(Dictionary<string, string> settings)
    {
        Settings = settings;
    }
}