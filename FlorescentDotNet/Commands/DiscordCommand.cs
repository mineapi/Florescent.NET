using Discord.WebSocket;
using FlorescentDotNet.Database;

namespace FlorescentDotNet.Commands
{
    public abstract class DiscordCommand
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public DiscordPermission PermissionLevel { get; set; }
        
        public BotDatabase BotDatabase { get; }
        public Bot Bot { get; }

        public DiscordCommand(Bot bot)
        {
            BotDatabase = bot.Database;
            Bot = bot;
        }

        public abstract Task Run(SocketMessage message, string[] args);
    }

    public enum DiscordPermission
    {
        HIGH, //High is classified under the "ADMINISTRATOR" permission.
        MEDIUM, //Medium is classified under the "MANAGE CHANNELS" permission.
        LOW //Low is classified under the @everyone role.
    }
}