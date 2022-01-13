using System.Diagnostics;
using Discord;
using Discord.WebSocket;
using FlorescentDotNet.Commands.Base;
using FlorescentDotNet.Database;
using FlorescentDotNet.Util;

namespace FlorescentDotNet.Commands
{
    public class Settings : DiscordCommand, IDiscordMessageCommand
    {
        public Settings(Bot bot) : base(bot)
        {
            this.Name = "settings";
            this.Description = "Update settings for guild.";
            this.RequiredPermissions = new[] {GuildPermission.ManageChannels};
            this.Category = "Bot";
        }
        
        public async Task RunMessageCommand(SocketMessage message, string[] args)
        {
            SocketGuildChannel targetChannel = (SocketGuildChannel) message.Channel;
            SocketGuild targetGuild = targetChannel.Guild;

            GuildSettings settings = GetGuildSettings(targetGuild.Id);

            if (args.Length >= 1)
            {
                switch (args[0].ToLower())
                {
                    case "edit":
                        if (args.Length >= 3)
                        {
                            try
                            {
                                DatabaseUtils.UpdateGuildSettings(BotDatabase, targetGuild.Id, args[1], args[2]);

                                await message.Channel.SendMessageAsync(
                                    "Updated key " + args[1] + " to \'" + args[2] + "\'!");
                            }
                            catch (Exception exception)
                            {
                                Console.WriteLine("Unable to update database");

                                await message.Channel.SendMessageAsync("Provided key or value is invalid.");
                            }
                        }
                        break;
                    default:
                        await SendSettingEmbed(settings, message.Channel);
                        break;
                }

                await Task.CompletedTask;
            }
        }

        public async Task SendSettingEmbed(GuildSettings settings, ISocketMessageChannel channel)
        {
            SocketGuildChannel guildChannel = (SocketGuildChannel) channel;
            
            EmbedBuilder settingsEmbedBuilder = new EmbedBuilder()
                .WithTitle("Settings")
                .WithDescription("Modify bot configuration for this guild.")
                .WithThumbnailUrl(guildChannel.Guild.IconUrl)
                .WithColor(Color.Purple)
                .WithFooter(new EmbedFooterBuilder().WithText("Built on the Florescent.NET framework.").WithIconUrl(Bot.client.CurrentUser.GetAvatarUrl()));

            foreach (string setting in settings.Settings.Keys)
            {
                if (setting != "id")
                {
                    settingsEmbedBuilder.AddField(setting, settings.Settings[setting]);
                }
            }


            await channel.SendMessageAsync("", false, settingsEmbedBuilder.Build());
        }

        public GuildSettings GetGuildSettings(ulong guildId)
        {
            GuildSettings settings;
            try
            {
                settings = DatabaseUtils.GetSettingsFromGuildId(BotDatabase, guildId);
            }
            catch (Exception exception)
            {
                DatabaseUtils.CreateGuildProfile(BotDatabase, guildId);
                settings = DatabaseUtils.GetSettingsFromGuildId(BotDatabase, guildId);
                Console.WriteLine(exception.Message);
            }

            return settings;
        }
    }
}