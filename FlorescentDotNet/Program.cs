using FlorescentDotNet;

public class Program
{
    private static Bot bot;
    
    public static void Main(string[] args)
    {
        bot = new Bot();

        bot.Start().GetAwaiter().GetResult();
    }
}