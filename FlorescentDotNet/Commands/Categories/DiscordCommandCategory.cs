namespace FlorescentDotNet.Categories;

public class DiscordCommandCategory
{
    public string Name { get; }
    public string Emoji { get; }
    public string Description { get; }
    
    /*
     * Add categories to here.
     */
    public static DiscordCommandCategory[] Categories = { 
        new("General", "☕", "General bot commands."),
        new("Bot", "🤖", "Commands relating to bot configuration.")
    };

    public DiscordCommandCategory(string name, string emoji, string description)
    {
        Name = name;
        Emoji = emoji;
        Description = description;
    }
}