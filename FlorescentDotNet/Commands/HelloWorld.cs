using Discord;
using Discord.WebSocket;
using FlorescentDotNet.Commands.Base;

namespace FlorescentDotNet.Commands;

public class HelloWorld : DiscordCommand, IDiscordMessageCommand
{
    public HelloWorld(Bot bot) : base(bot)
    {
        Name = "helloworld";
        Description = "Print hello world!";
        RequiredPermissions = new[] {GuildPermission.SendMessages};
        Category = "general";
    }

    public async Task RunMessageCommand(SocketMessage message, string[] args)
    {
        await message.Channel.SendMessageAsync("Hello, world!");
    }
}