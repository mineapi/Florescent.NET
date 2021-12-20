namespace FlorescentDotNet.Util;

public class GuildSettings
{
    public String Prefix { get; set; }
    public ulong AdminRole { get; set; }
    
    /*
     * Add variables for guild settings here!
     */

    public GuildSettings(string prefix, ulong adminRole)
    {
        Prefix = prefix;
        AdminRole = adminRole;
    }
}