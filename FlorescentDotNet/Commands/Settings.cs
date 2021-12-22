using System.Diagnostics;
using Discord;
using Discord.WebSocket;
using FlorescentDotNet.Database;
using FlorescentDotNet.Util;

namespace FlorescentDotNet.Commands
{
    public class Settings : DiscordCommand
    {
        public Settings(Bot bot) : base(bot)
        {
            this.Name = "settings";
            this.Description = "Update settings for guild.";
            this.PermissionLevel = DiscordPermission.HIGH;
            this.Category = "Bot";
        }

        public async override Task Run(SocketMessage message, string[] args)
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
            
            EmbedBuilder embed = new EmbedBuilder();
            embed.Title = "Settings";
            embed.Description = "These are the bot settings for this guild.";
            embed.AddField("Prefix", settings.Prefix);
            embed.AddField("High Level Role", settings.AdminRole);
            embed.ThumbnailUrl = guildChannel.Guild.IconUrl;
            embed.Color = Color.Purple;
            embed.Footer = new EmbedFooterBuilder().WithText("Built on the Florescent.NET framework.")
                .WithIconUrl(Bot.client.CurrentUser.GetAvatarUrl());


            await channel.SendMessageAsync("", false, embed.Build());
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