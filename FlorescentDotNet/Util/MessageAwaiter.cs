using Discord;
using Discord.Rest;
using Discord.WebSocket;
using Timer = System.Timers.Timer;

namespace FlorescentDotNet.Util;

public class MessageAwaiter
{
    private DiscordSocketClient _Client;
    private SocketChannel _Channel;
    private SocketUser _Replier;
    private Func<SocketMessage, Task> _onReply;
    private readonly DateTime _time = DateTime.Now;

    public MessageAwaiter(DiscordSocketClient client, SocketChannel channel, SocketUser replier, Func<SocketMessage, Task> onReply, TimeSpan timeout)
    {
        _Client = client;
        _Channel = channel;
        _Replier = replier;
        _onReply = onReply;
        
        client.MessageReceived += ExecuteAwaitMessage; //Register the event.
        
        
        new Thread(() => //Timeout
        {
            while (true) //Execute this as thread as the loop runs.
            {
                if (_time.AddSeconds(timeout.Seconds).Second == DateTime.Now.Second) //Check the time.
                {
                    client.MessageReceived -= ExecuteAwaitMessage; //If the timeout has been reached, stop listening for reactions.
                    break; //Exit the loop.
                }
            }
        }).Start();
    }
    
    private Task ExecuteAwaitMessage(SocketMessage message)
    {
        if (message.Channel == _Channel && message.Author == _Replier)
        {
            _onReply.Invoke(message); //Invoke the onReact method.
            _Client.MessageReceived -= ExecuteAwaitMessage; //Unregister event since it's already been completed.
        }
        return Task.CompletedTask;
    }
}