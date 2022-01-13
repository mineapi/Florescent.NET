using System.Diagnostics;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Timer = System.Timers.Timer;

namespace FlorescentDotNet.Util;

public class ReactionAwaiter
{
    private DiscordSocketClient _Client;
    private RestUserMessage _Message;
    private SocketUser _Replier;
    private Func<Emoji, Task> _onReact;
    private readonly DateTime _time = DateTime.Now;

    public ReactionAwaiter(DiscordSocketClient client, RestUserMessage message, SocketUser replier, Func<Emoji, Task> onReact, TimeSpan timeout)
    {
        _Client = client;
        _Message = message;
        _Replier = replier;
        this._onReact = onReact;
        
        client.ReactionAdded += ExecuteAwaitReaction; //Register the event.
        
        new Thread(() => //Timeout
        {
            while (true) //Execute this as thread as the loop runs.
            {
                if (_time.AddSeconds(timeout.Seconds).Second == DateTime.Now.Second) //Check the time.
                {
                    client.ReactionAdded -= ExecuteAwaitReaction; //If the timeout has been reached, stop listening for reactions.
                    break; //Exit the loop.
                }
            }
        }).Start();
    }
    
    private Task ExecuteAwaitReaction(Cacheable<IUserMessage, ulong> userMessage, Cacheable<IMessageChannel, ulong> channel, SocketReaction emoji)
    {
        Console.WriteLine("Test");
        Console.WriteLine(userMessage.Id);
        Console.WriteLine(_Message.Id);
        
        if (emoji.MessageId == _Message.Id  && emoji.UserId == _Replier.Id)
        {
            _onReact.Invoke(new Emoji(emoji.Emote.Name)); //Invoke the onReact method.
            _Client.ReactionAdded -= ExecuteAwaitReaction; //Unregister event since it's already been completed.
        }
        return Task.CompletedTask;
    }
}