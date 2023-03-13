using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// https://learn.microsoft.com/en-us/visualstudio/ide/default-keyboard-shortcuts-in-visual-studio?view=vs-2022
namespace Notenverwaltung {
    internal static class Database {

        private const String databaseFile = "GradeDb.sqlite";
        public static void InitCompleteDatabase(bool resetDatabase = true) {
            if (resetDatabase) {
                ResetDatabase();
            }
            CreateDatabaseFile();
            CreateAllTables();
            InsertDemoData();
        }

        ////////////////////////////////////////////////////////////////////////////////////
        /// Verbindungsaufbau und Query-Ausführung
        ////////////////////////////////////////////////////////////////////////////////////

        public static void CreateDatabaseFile() {
            // Abfrage um ein versehentliches Löschen zu vermeiden
            if (!File.Exists(databaseFile)) {
                // this creates a zero-byte file
                SQLiteConnection.CreateFile(databaseFile);
            }

        }

        private static SQLiteConnection OpenConnection() {
            string connectionString = "Data Source=" + databaseFile + ";Version=3;";
            SQLiteConnection m_dbConnection = new SQLiteConnection(connectionString);
            m_dbConnection.Open();
            return m_dbConnection;
        }

        private static void ExecuteQueryList(string[] queryList) {
            SQLiteConnection m_dbConnection = OpenConnection();
            using (SQLiteTransaction transaction = m_dbConnection.BeginTransaction()) {
                String lastQuery = "";
                try {

                    foreach (String query in queryList) {
                        lastQuery = query;
                        SQLiteCommand command = new SQLiteCommand(query, m_dbConnection);
                        command.ExecuteNonQuery();

                    }
                    transaction.Commit();
                }
                catch (Exception e) {
                    transaction.Rollback();
                    Console.Error.WriteLine("Query Ausführung abgebrochen.", e);
                    Console.Error.WriteLine("Letzer Query=" + lastQuery);
                    //throw;
                }
            }
            m_dbConnection.Close();
        }
        private static void ExecuteQuery(string sql) {
            ExecuteQueryList(new string[] { sql });
        }



        ////////////////////////////////////////////////////////////////////////////////////
        /// Tables: Create and Delete + Insert Demodata
        ////////////////////////////////////////////////////////////////////////////////////
        
        /// <summary>
        /// Löschung der DB-Datei und Neuerstellung aller Tabellen
        /// </summary>
        public static void ResetDatabase() {
            //SQLiteConnection.CreateFile(databaseFile);
            DropAllTables();
            CreateAllTables();
        }
        private static void DropAllTables() {
            ExecuteQuery(SqliteScripts.DropAllTables());
        }
        private static void CreateAllTables() {
            ExecuteQueryList(SqliteScripts.CreateAllTables());
        }
        private static void InsertDemoData() {
            ExecuteQueryList(SqliteScripts.InsertDemoData());
        }

        ////////////////////////////////////////////////////////////////////////////////////
        /// Create, Update, Delete Specific Rows
        ////////////////////////////////////////////////////////////////////////////////////
    }
}
