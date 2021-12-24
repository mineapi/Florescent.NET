using System.Diagnostics;
using Discord;
using Discord.Rest;
using Discord.WebSocket;
using FlorescentDotNet.Categories;
using FlorescentDotNet.Util;

namespace FlorescentDotNet.Commands;

public class Help : DiscordCommand
{
    public Help(Bot bot) : base(bot)
    {
        this.Name = "help";
        this.Description = "View a list of bot commands.";
        this.PermissionLevel = DiscordPermission.LOW;
        this.Category = "General";
    }

    public override async Task Run(SocketMessage message, string[] args)
    {
        EmbedBuilder helpEmbed = new EmbedBuilder();
        helpEmbed.Title = "Help";
        helpEmbed.Description = "React with category of commands you would to view!";
        helpEmbed.Color = Color.Purple;

        foreach (DiscordCommandCategory category in DiscordCommandCategory.Categories)
        {
            helpEmbed.AddField(category.Name + " " + category.Emoji, category.Description);
        }

        RestUserMessage sendHelpMessage = await message.Channel.SendMessageAsync("", false, helpEmbed.Build());

        foreach (DiscordCommandCategory category in DiscordCommandCategory.Categories)
        {
            await sendHelpMessage.AddReactionAsync(new Emoji(category.Emoji));
        }

        new ReactionAwaiter(Bot.client, sendHelpMessage, message.Author, (emoji) =>
        {
            bool successful = false;
            foreach (DiscordCommandCategory category in DiscordCommandCategory.Categories)
            {
                if (emoji.Equals(new Emoji(category.Emoji)))
                {
                    EmbedBuilder categoryEmbed = new EmbedBuilder();

                    categoryEmbed.Title = category.Name;
                    categoryEmbed.Description = category.Description;
                    categoryEmbed.Color = Color.Purple;

                    foreach (DiscordCommand command in Bot.commands)
                    {
                        if (command.Category.ToLower() == category.Name.ToLower())
                        {
                            categoryEmbed.AddField(command.Name, command.Description);
                        }
                    }

                    sendHelpMessage.ModifyAsync(message =>
                    {
                        message.Embed = categoryEmbed.Build();
                    });
                    sendHelpMessage.RemoveAllReactionsAsync();
                    successful = true;
                }
            }

            if (!successful)
            {
                sendHelpMessage.ModifyAsync(message =>
                {
                    message.Content = "Invalid option selected.";
                    message.Embed = new EmbedBuilder().Build();
                });
            }
            
            return Task.CompletedTask;
        });
    }
}