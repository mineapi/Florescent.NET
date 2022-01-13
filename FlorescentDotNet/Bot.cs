using System.Diagnostics;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using FlorescentDotNet.Categories;
using FlorescentDotNet.Commands;
using FlorescentDotNet.Commands.Base;
using FlorescentDotNet.Database;
using FlorescentDotNet.Util;

namespace FlorescentDotNet
{
    public class Bot
    {
        private String token;
        public DiscordSocketClient client;

        public List<DiscordCommand> commands = new List<DiscordCommand>();
        private List<IDiscordSlashCommand> _slashCommands;

        public BotDatabase Database;
        public DotEnvReader Env;

        public Bot()
        {
            Env = new DotEnvReader("config.env");
            
            this.token = Env.GetProperty("TOKEN");

            this.Database = new BotDatabase("data.sqlite"); //Initialize the bot database.

            client = new DiscordSocketClient(); //Initialize the Discord client.
            Database.CreateTable("table"); //Create the table.
        }

        public async Task Ready()
        {
            Logger.Log("Ready!");
            
            await client.SetActivityAsync(new Game("Built on the Florescent.NET framework.", ActivityType.Playing)); //Set the bot status.

            _slashCommands = new List<IDiscordSlashCommand>();

            foreach (DiscordCommand command in commands)
            {
                if (command is IDiscordSlashCommand)
                {
                    _slashCommands.Add((IDiscordSlashCommand) command);
                }
            }

            foreach (IDiscordSlashCommand command in _slashCommands)
            {
                SlashCommandBuilder commandBuilder = new SlashCommandBuilder()
                    .WithName(((DiscordCommand) command).Name)
                    .WithDescription(((DiscordCommand) command).Description);

                await client.CreateGlobalApplicationCommandAsync(commandBuilder.Build());
            }
            
            await Task.CompletedTask; //Event complete.
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
                        prefix = settings.Settings["prefix"];
                    }
                    catch (Exception exception) //Find any errors from getting settings.
                    {
                        settings = DatabaseUtils.GetDefaultGuildSettings();
                        Debug.WriteLine(exception.Message); //Log the error.
                    }
                    
                    if (message.Content.ToLower().StartsWith(prefix + command.Name))
                    {
                        SocketGuildUser user = (SocketGuildUser)message.Author; //Get the user as a guild user.

                        if (command is IDiscordMessageCommand)
                        {
                            IDiscordMessageCommand commandAsMessageCommand = (IDiscordMessageCommand) command;
                            if (Utils.UserHasAllPermissions(user, command.RequiredPermissions))
                            {
                                commandAsMessageCommand.RunMessageCommand(message,
                                    message.Content.Replace(prefix + command.Name + " ", "")
                                        .Split(" ")); //Run the command.
                            }
                        }
                        Logger.Log(String.Format("User {0} ran the command \'{1}\'!", user.Username + "#" + user.Discriminator, command.Name)); //Log the command.
                    }
                }
            }
            return Task.CompletedTask; //Event complete.
        }

        public async Task onSlashCommandRecieved(SocketSlashCommand command)
        {
            foreach (IDiscordSlashCommand slashCommand in _slashCommands)
            {
                if (command.Data.Name == ((DiscordCommand) slashCommand).Name)
                {
                    await slashCommand.RunSlashCommand(command);
                }
            }
        }

        public async Task Start()
        {
            initCommands(); //Load commands.

            client.Ready += Ready; //Set the ready event.
            client.MessageReceived += MessageRecieved; //Set the message received event.
            client.SlashCommandExecuted += onSlashCommandRecieved;
            

            Logger.Log("Logging in.");
            await client.LoginAsync(TokenType.Bot, token, true); //Login.
            Logger.Log("Starting.");
            await client.StartAsync(); //Start.

            await Task.Delay(-1); //Prevent process from ending.
        }

        public async Task ClearSlashCommands() //Call this if you need to clear slash commands.
        {
            foreach (SocketApplicationCommand command in await client.GetGlobalApplicationCommandsAsync())
            {
                await client.GetGlobalApplicationCommandAsync(command.Id).Result.DeleteAsync();
            }
        }

        private void initCommands()
        {
            /*
             * Add commands here.
             */
            commands.Add(new Settings(this));
            commands.Add(new Help(this));
            commands.Add(new Info(this));
            commands.Add(new HelloWorld(this));
        }
    }
}