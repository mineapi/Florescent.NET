using Discord;
using Discord.Rest;
using Discord.WebSocket;

namespace FlorescentDotNet.Util;

public class ReactionAwaiter
{
    private DiscordSocketClient Client;
    private RestUserMessage Message;
    private SocketUser Replier;
    private Func<Emoji, Task> onReact;

    public ReactionAwaiter(DiscordSocketClient client, RestUserMessage message, SocketUser replier, Func<Emoji, Task> onReact)
    {
        Client = client;
        Message = message;
        Replier = replier;
        this.onReact = onReact;
        
        client.ReactionAdded += ExecuteAwaitReaction; //Register the event.
    }
    
    private Task ExecuteAwaitReaction(Cacheable<IUserMessage, ulong> userMessage, ISocketMessageChannel channel, SocketReaction emoji)
    {
        if (emoji.MessageId == Message.Id  && emoji.UserId == Replier.Id)
        {
            onReact.Invoke(new Emoji(emoji.Emote.Name)); //Invoke the onReact method.
            Client.ReactionAdded -= ExecuteAwaitReaction; //Unregister event since it's already been completed.
        }
        return Task.CompletedTask;
    }
}