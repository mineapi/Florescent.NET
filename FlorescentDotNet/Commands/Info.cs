using Discord;
using Discord.WebSocket;
using FlorescentDotNet.Commands.Base;

namespace FlorescentDotNet.Commands;

public class Info : DiscordCommand, IDiscordMessageCommand
{
    public Info(Bot bot) : base(bot)
    {
        this.Name = "info";
        this.Description = "Information about the bot.";
        this.RequiredPermissions = new[] {GuildPermission.SendMessages};
        this.Category = "general";
    }
    
    public async Task RunMessageCommand(SocketMessage message, string[] args)
    {
        EmbedBuilder infoEmbed = new EmbedBuilder();
        infoEmbed.Title = "Info";
        infoEmbed.Description = "Florescent is a Discord bot framework built on the Discord.NET API wrapper.";
        infoEmbed.Color = Color.Purple;
        infoEmbed.AddField("Creator", "MineAPI");
        infoEmbed.AddField("Github Repo", "https://github.com/mineapi/Florescent.NET");
        infoEmbed.AddField("Website", "https://mineapi.me");
        infoEmbed.Footer = new EmbedFooterBuilder().WithText("Built on the Florescent.NET framework.").WithIconUrl(Bot.client.CurrentUser.GetAvatarUrl());

        await message.Channel.SendMessageAsync("", false, infoEmbed.Build());
    }
}