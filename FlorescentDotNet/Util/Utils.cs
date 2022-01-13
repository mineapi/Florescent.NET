using Discord;
using Discord.WebSocket;

namespace FlorescentDotNet.Util;

public class Utils
{
    public static bool UserHasRole(SocketGuildUser user, SocketRole role)
    {
        bool returnValue = false;

        foreach (SocketRole roles in user.Roles)
        {
            if (roles == role)
            {
                returnValue = true;
            }
        }
        return returnValue;
    }

    public static bool UserHasAllPermissions(SocketGuildUser user, GuildPermission[] permissions)
    {
        bool hasPermission = true;
        
        foreach (GuildPermission permission in permissions)
        {
            if (!user.GuildPermissions.Has(permission))
            {
                hasPermission = false;
                break;
            }
        }
        return hasPermission;
    }
}