using System.Diagnostics;
using Discord;
using Discord.WebSocket;
using FlorescentDotNet.Commands;
using FlorescentDotNet.Database;
using FlorescentDotNet.Util;

namespace FlorescentDotNet
{
    public class Bot
    {
        private String token;
        public DiscordSocketClient client;

        public List<DiscordCommand> commands = new List<DiscordCommand>();

        public BotDatabase Database;

        public Bot(String token)
        {
            this.token = token; //Initialize the bot token.

            this.Database = new BotDatabase("data.sqlite"); //Initialize the bot database.

            client = new DiscordSocketClient(); //Initialize the Discord client.
            Database.CreateTable("table"); //Create the table.
        }

        public Task Ready()
        {
            Logger.Log("Ready!");
            
            client.SetActivityAsync(new Game("Built on the Florescent.NET framework.", ActivityType.Playing)); //Set the bot status.
            
            return Task.CompletedTask; //Event complete.
        }

        public Task MessageRecieved(SocketMessage message)
        {
            if (!message.Author.IsBot) //Make sure the command sender isn't a bot.
            {
                foreach (DiscordCommand command in commands) //Look for command.
                {
                    SocketGuildChannel targetChannel = (SocketGuildChannel) message.Channel; //Get the guild channel.
                    SocketGuild targetGuild = targetChannel.Guild; //Get the guild.
                        
                    GuildSettings settings;
                    string prefix = BotDatabase.DefaultPrefix;
                    try
                    {
                        settings = DatabaseUtils.GetSettingsFromGuildId(Database, targetGuild.Id); //Get settings for the guild from the id.
                        prefix = settings.Prefix;
                    }
                    catch (Exception exception) //Find any errors from getting settings.
                    {
                        settings = new GuildSettings(prefix, BotDatabase.DefaultAdminRole);
                        Debug.WriteLine(exception.Message); //Log the error.
                    }
                    
                    if (message.Content.ToLower().StartsWith(prefix + command.Name))
                    {
                        IDisposable typingState = message.Channel.EnterTypingState(); //Start typing.
                        
                        SocketGuildUser user = (SocketGuildUser)message.Author; //Get the user as a guild user.
                        try
                        {
                            switch (command.PermissionLevel)
                            {
                                case DiscordPermission.HIGH:
                                    if (user.GuildPermissions.Administrator ||
                                        Utils.UserHasRole(user, user.Guild.GetRole(settings.AdminRole)))
                                    {
                                        command.Run(message,
                                            message.Content.Replace(prefix + command.Name + " ", "")
                                                .Split(" ")); //Run the command.
                                        Console.WriteLine("Ran the command \'" + command.Name + "\'!");
                                    }

                                    break;
                                case DiscordPermission.LOW:
                                    command.Run(message,
                                        message.Content.Replace(prefix + command.Name + " ", "")
                                            .Split(" ")); //Run the command.
                                    Console.WriteLine("Ran the command \'" + command.Name + "\'!");
                                    break;
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex);
                        }
                        
                        Logger.Log(String.Format("User {0} ran the command {1}", user.Username, command.Name)); //Log the command.
                        
                        typingState.Dispose(); //Stop typing.
                    }
                }
            }

            return Task.CompletedTask; //Event complete.
        }

        public async Task Start()
        {
            initCommands(); //Load commands.

            client.Ready += () => Ready(); //Set the ready event.
            client.MessageReceived += (SocketMessage message) => MessageRecieved(message); //Set the message received event.

            Logger.Log("Logging in.");
            await client.LoginAsync(TokenType.Bot, token, true); //Login.
            Logger.Log("Starting.");
            await client.StartAsync(); //Start.

            await Task.Delay(-1); //Prevent process from ending.
        }

        private void initCommands()
        {
            /*
             * Add commands here.
             */
            commands.Add(new Settings(this));
            commands.Add(new Help(this));
            commands.Add(new Info(this));
        }
    }
}