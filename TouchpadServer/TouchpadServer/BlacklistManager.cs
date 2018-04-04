using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace TouchpadServer {
    static class BlacklistManager {
        public static event EventHandler changeEventHandler;
        private const string getCount = "SELECT COUNT(address) FROM blacklistButton";
        private const string getAllItems = "SELECT * FROM blacklistButton";
        private const string removeItem = "DELETE FROM blacklistButton WHERE address='{0}'";
        private const string insertItem = "INSERT INTO blacklistButton (name, address) VALUES ('{0}', '{1}')";
        private const string setConnection = "Data Source=blacklistButton.sqlite;Version=3;";
        private const string createTable = "CREATE TABLE blacklistButton (name string, address string)";
        private const string getItemByAddress = "SELECT * FROM blacklistButton WHERE address='{0}'";
        public static void SetUp() {
            SQLiteConnection.CreateFile("blacklistButton.sqlite");
            SQLiteConnection connection = new SQLiteConnection(setConnection);
            SQLiteCommand command = new SQLiteCommand(createTable, connection);
            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();
            BlacklistManager.OnChange();
        }
        public static long GetCount() {
            SQLiteConnection connection = new SQLiteConnection(setConnection);
            SQLiteCommand command = new SQLiteCommand(getCount, connection);
            long result = 0;
            try {
                connection.Open();
                SQLiteDataReader reader = command.ExecuteReader();
                reader.Read();
                result = (long)reader[0];
                reader.Close();
            }
            catch (SQLiteException) {
                connection.Close();
                BlacklistManager.SetUp();
                return 0;
            }
            finally {
                connection.Close();
            }
            return result;
        }
        public static void Insert(string name, string address) {
            SQLiteConnection connection = new SQLiteConnection(setConnection);
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(String.Format(insertItem, name, address), connection);
            try {
                command.ExecuteNonQuery();
            }
            catch (SQLiteException) {
                connection.Close();
                BlacklistManager.SetUp();
                connection.Open();
                command.ExecuteNonQuery();
            }
            finally {
                connection.Close();
            }
            BlacklistManager.OnChange();
        }
        public static string[][] GetAllItems() {
            string[][] result = new string[BlacklistManager.GetCount()][];
            SQLiteConnection connection = new SQLiteConnection(setConnection);
            SQLiteCommand command = new SQLiteCommand(getAllItems, connection);
            connection.Open();
            SQLiteDataReader reader = null;
            try {
                reader = command.ExecuteReader();
                for (int i = 0; reader.Read(); i++) {
                    result[i] = new string[2] { (string)reader["name"], (string)reader["address"] };
                }
                reader.Close();
            }
            catch (SQLiteException) {
                connection.Close();
                BlacklistManager.SetUp();
                //should be empty now, so no loop
            }
            finally {
                connection.Close();
            }
            return result;
        }
        public static void Delete(string address) {
            SQLiteConnection connection = new SQLiteConnection(setConnection);
            connection.Open();
            SQLiteCommand command = new SQLiteCommand(String.Format(removeItem, address), connection);
            try {
                command.ExecuteNonQuery();
                BlacklistManager.OnChange();
            }
            catch (SQLiteException) {
                connection.Close();
                BlacklistManager.SetUp();
            }
            finally {
                connection.Close();
            }
        }
        public static bool Contains(string address) {
            SQLiteConnection connection = new SQLiteConnection(setConnection);
            SQLiteCommand command = new SQLiteCommand(String.Format(getItemByAddress, address), connection);
            connection.Open();
            SQLiteDataReader reader = null;
            try {
                reader = command.ExecuteReader();
                while (reader.Read()) {
                    reader.Close();
                    return true;
                }
                reader.Close();
            }
            catch (SQLiteException) {
                connection.Close();
                BlacklistManager.SetUp();
                //should always be empty here, so no loop
            }
            finally {
                connection.Close();
            }
            return false;
        }
        private static void OnChange() {
            if (changeEventHandler != null)
                changeEventHandler(null, new EventArgs());
        }

    }
}
