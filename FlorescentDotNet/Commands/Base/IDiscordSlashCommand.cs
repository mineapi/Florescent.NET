using Discord.WebSocket;

namespace FlorescentDotNet.Commands.Base;

public interface IDiscordSlashCommand
{
    public Task RunSlashCommand(SocketSlashCommand command);
}