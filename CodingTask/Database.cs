using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SQLite;
using System.IO;
using System.Data;

namespace CodingTask
{
    class Database
    {
        SQLiteConnection sQLiteConnection;

        private static Database database = null;
        // makes connection method so it's possible to reach
        // database from other files
        public static Database GetConnection()
        {
            if (database == null)
            {
                database = new Database();
            }
            return database;
        }

        public void Init()
        {
            // creates database file if missing
            if (!File.Exists("db.sqlite"))
            {
                SQLiteConnection.CreateFile("db.sqlite");
            }

            // connects to sqlite
            sQLiteConnection = new SQLiteConnection("Data Source=db.sqlite;Version=3;");
            sQLiteConnection.Open();

            if (sQLiteConnection.State == ConnectionState.Open)
            {
                sQLiteConnection.Close();
                sQLiteConnection.Open();
            }

            // creates array table if missing
            string sql = "CREATE TABLE IF NOT EXISTS arrays (array VARCHAR(100), winnable INT)";
            SQLiteCommand command = new SQLiteCommand(sql, sQLiteConnection);
            command.ExecuteNonQuery();
        }

        public void Insert(string array, int winnable)
        {
            string sql = "INSERT INTO arrays (array, winnable) VALUES ($array, $winnable)";
            SQLiteCommand command = new SQLiteCommand(sql, sQLiteConnection);
            command.Parameters.AddWithValue("$array", array);
            command.Parameters.AddWithValue("$winnable", winnable);
            command.ExecuteNonQuery();
        }

        public int IsExisting(string array)
        {
            string sql = "SELECT * FROM arrays WHERE array = $array";
            SQLiteCommand command = new SQLiteCommand(sql, sQLiteConnection);
            command.Parameters.AddWithValue("$array", array);
            SQLiteDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                // if array already exists, returns winnable value
                if (reader["array"].ToString() == array)
                {
                    return reader.GetInt32("winnable");
                }
            }
            // else returns 2, because 0 and 1 are occupied
            return 2;
        }

        public void PrintValues()
        {
            string sql = "SELECT * FROM arrays";
            SQLiteCommand command = new SQLiteCommand(sql, sQLiteConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            // checks if there are arrays added, if yes, prints them
            if (!reader.HasRows)
            {
                Console.WriteLine("No arrays added yet.");
                Console.WriteLine();
            }
            else
            {
                while (reader.Read())
                {
                    Console.WriteLine("Array: " + reader["array"]);
                    Console.Write("Winnable? ");
                    if (reader["winnable"].ToString() == "1") Console.Write("Yes");
                    else Console.Write("No");
                    Console.WriteLine();
                    Console.WriteLine();
                }
            }

        }

        public void ClearTable()
        {
            string sql = "DELETE FROM arrays";
            SQLiteCommand command = new SQLiteCommand(sql, sQLiteConnection);
            command.ExecuteNonQuery();
        }

        public void DropTable()
        {
            string sql = "DROP TABLE arrays";
            SQLiteCommand command = new SQLiteCommand(sql, sQLiteConnection);
            command.ExecuteNonQuery();
        }
    }
}
