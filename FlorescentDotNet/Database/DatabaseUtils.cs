using FlorescentDotNet.Util;

namespace FlorescentDotNet.Database
{
    public class DatabaseUtils
    {
        public static void CreateGuildProfile(BotDatabase database, ulong guildId)
        {
            database.InsertData(String.Format("INSERT INTO \"table\" (id, prefix, adminRole) values (\"{0}\", \"{1}\", \"{2}\");", guildId, BotDatabase.DefaultPrefix));
        }
        
        public static GuildSettings GetSettingsFromGuildId(BotDatabase database, ulong guildId)
        {
            Dictionary<string, string> queryOutput =
                database.ReadData(String.Format("SELECT * FROM \"table\" WHERE id=\"{0}\";", guildId));

            return new GuildSettings(queryOutput);
        }

        public static void UpdateGuildSettings(BotDatabase database, ulong guildId, string key, string value)
        {
            database.InsertData("UPDATE \"table\" SET " + key + "=\"" + value +"\" WHERE id=\"" + guildId + "\"");
        }

        public static GuildSettings GetDefaultGuildSettings()
        {
            Dictionary<string, string> defaultSettings = new Dictionary<string, string>();
            
            defaultSettings.Add("prefix", new DotEnvReader("config.env").GetProperty("DEFUALT_PREFIX"));

            return new GuildSettings(defaultSettings);
        }
        
        public static GuildSettings GetGuildSettings(BotDatabase botDatabase, ulong guildId)
        {
            GuildSettings settings;
            try
            {
                settings = GetSettingsFromGuildId(botDatabase, guildId);
            }
            catch (Exception exception)
            {
                CreateGuildProfile(botDatabase, guildId);
                settings = GetSettingsFromGuildId(botDatabase, guildId);
                Console.WriteLine(exception.Message);
            }

            return settings;
        }
    }
}