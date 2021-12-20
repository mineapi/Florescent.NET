using FlorescentDotNet.Util;

namespace FlorescentDotNet.Database
{
    public class DatabaseUtils
    {
        public static void CreateGuildProfile(BotDatabase database, ulong guildId)
        {
            database.InsertData(String.Format("INSERT INTO \"table\" (id, prefix, adminRole) values (\"{0}\", \"{1}\", \"{2}\");", guildId, BotDatabase.DefaultPrefix, BotDatabase.DefaultAdminRole));
        }
        
        public static GuildSettings GetSettingsFromGuildId(BotDatabase database, ulong guildId)
        {
            Dictionary<string, string> queryOutput =
                database.ReadData(String.Format("SELECT * FROM \"table\" WHERE id=\"{0}\";", guildId));

            return new GuildSettings(queryOutput["prefix"], ulong.Parse(queryOutput["adminRole"]));
        }

        public static void UpdateGuildSettings(BotDatabase database, ulong guildId, string key, string value)
        {
            database.InsertData("UPDATE \"table\" SET " + key + "=\"" + value +"\" WHERE id=\"" + guildId + "\"");
        }
    }
}