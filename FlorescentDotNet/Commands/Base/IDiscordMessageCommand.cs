using Discord.WebSocket;

namespace FlorescentDotNet.Commands.Base;

public interface IDiscordMessageCommand
{
    public Task RunMessageCommand(SocketMessage message, string[] args);
}