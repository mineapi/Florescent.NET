using Discord;
using Discord.WebSocket;
using FlorescentDotNet.Categories;
using FlorescentDotNet.Database;
using FlorescentDotNet.Util;

namespace FlorescentDotNet.Commands.Base
{
    public abstract class DiscordCommand
    {
        public String Name { get; set; }
        public String Description { get; set; }
        
        public GuildPermission[] RequiredPermissions;
        public BotDatabase BotDatabase { get; }
        public Bot Bot { get; set; }

        public string Category { get; set; }

        public DiscordCommand(Bot bot)
        {
            BotDatabase = bot.Database;
            Bot = bot;
        }
    }
}