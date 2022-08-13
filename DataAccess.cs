using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Windows.Storage;

namespace B2DsqliteInsert02
{
    class DataAccess
    {
        public async static void InitializeDatabase()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync("sqliteSample.db", CreationCollisionOption.OpenIfExists);
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                String tableCommand = "CREATE TABLE IF NOT " +
                    "EXISTS MyTable (Primary_Key INTEGER PRIMARY KEY," +
                    "Text_Entry NVARCHAR(2048) NULL)";
                SqliteCommand createTable = new SqliteCommand(tableCommand, db);
                createTable.ExecuteReader();
            }
        } 
        public static void AddData(string inputText)
        {
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}") )
            {
                db.Open();
                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;

                //user parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "Insert Into MyTable Values (NULL,@Entry);";
                insertCommand.Parameters.AddWithValue("@Entry", inputText);
                insertCommand.ExecuteReader();
                db.Close();
            }
        }
        public static List<String> GetData()
        {
            List<String> entries = new List<string>();
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, "sqliteSample.db");
            using(SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand("Select Text_Entry from MyTable", db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while(query.Read())
                {
                    entries.Add(query.GetString(0));
                }
                db.Close();
            }
            return entries;
        }
    }
}
