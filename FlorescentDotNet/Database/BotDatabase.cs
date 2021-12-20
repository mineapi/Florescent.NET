using System.Collections.Specialized;
using System.Data.SQLite;
using System.Diagnostics;

namespace FlorescentDotNet.Database
{
    public class BotDatabase
    {
        public SQLiteConnection _connection;
        
        /*
         * Default values.
         */

        public static String DefaultPrefix = ";";
        public static ulong DefaultAdminRole = 111111111111111111;
        
        public BotDatabase(string path)
        {
            this._connection = new SQLiteConnection("Data Source=database.db;Version=3;New=True;Compress=True;");

            try
            {
                _connection.Open();
            }
            catch (Exception ex)
            {
                Console.WriteLine("There was an error processing the internal database.");
                Console.WriteLine(ex.Message);
            }
        }
        
        public void CreateTable(string tableName)
        {
            SQLiteCommand sqLiteCommand = _connection.CreateCommand();
            sqLiteCommand.CommandText = String.Format("CREATE TABLE IF NOT EXISTS \"{0}\"" + "(\n" +
                "   id long PRIMARY KEY,\n" +
                "   prefix string NOT NULL,\n" +
                "   adminRole string NOT NULL\n" +
                ");", tableName);
            sqLiteCommand.ExecuteNonQuery();
        }

        public void InsertData(string sql)
        {
            SQLiteCommand sqLiteCommand = _connection.CreateCommand();
            sqLiteCommand.CommandText = sql;
            sqLiteCommand.ExecuteNonQuery();
        }

        public Dictionary<string, string> ReadData(string sql)
        {
            SQLiteCommand querySettings = _connection.CreateCommand();
            querySettings.CommandText = sql;

            SQLiteDataReader query = querySettings.ExecuteReader();

            Dictionary<String, String> queryOutput = new Dictionary<string, string>();

            NameValueCollection values = query.GetValues();

            foreach (String s in values.AllKeys)
            {
                queryOutput.Add(s, values.Get(s));
            }

            return queryOutput;
        }
    }
}