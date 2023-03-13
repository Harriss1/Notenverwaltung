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
        public static void InitCompleteDatabase(bool createDemoData = true) {
            CreateDatabaseFile();
            CreateAllTables();
            CreateAdmin();
        }

        private static void CreateAdmin() {
            ExecuteQuery("");
        }

        public static void CreateDatabaseFile() {
            // this creates a zero-byte file
            // Abfrage um ein versehentliches Löschen zu vermeiden
            if (!File.Exists(databaseFile)) {
                SQLiteConnection.CreateFile(databaseFile);
            }

        }
        
        /// <summary>
        /// Löschung der DB-Datei und Neuerstellung aller Tabellen
        /// </summary>
        public static void ResetDatabase() {
            SQLiteConnection.CreateFile(databaseFile);
            CreateAllTables();
        }

        private static void DropAllTables() {
            ExecuteQuery("" +
                "#Verbindungstabellen" +
                "DROP TABLE if EXISTS Schueler_Hat_Klasse;" +
                "# Haupttabellen" +
                "DROP TABLE if EXISTS Lehrer;" +
                "DROP TABLE if EXISTS Schueler;" +
                "DROP TABLE if EXISTS person;" +
                "# Von den Haupttabellen abhängige Haupttabellen" +
                "DROP TABLE if EXISTS Klasse;");
        }
        private static void CreateAllTables() {
            String createPerson =
                "CREATE TABLE IF NOT EXISTS Person (" +
                "PersonId INT NOT NULL AUTOINCREMENT," +
                "Vorname VARCHAR(50)," +
                "Nachname VARCHAR(50)," +
                "Geburtsdatum DATE," +
                "Geburtsort VARCHAR(50)," +
                "Benutzername VARCHAR(50) NOT NULL UNIQUE," +
                "Passwort VARCHAR(50)," +
                "PRIMARY KEY (`PersonId`)); ";
            String createTeacher =
                "CREATE TABLE IF NOT EXISTS Lehrer(" +
                "LehrerId INT NOT NULL AUTOINCREMENT," +
                "PersonId INT NOT NULL," +
                "FOREIGN KEY(PersonId) REFERENCES Person(PersonId)," +
                "PRIMARY KEY(LehrerId)); ";
            String createStudent = "CREATE TABLE IF NOT EXISTS Schueler(" +
                "SchuelerId INT NOT NULL AUTOINCREMENT," +
                "PersonId INT NOT NULL," +
                "FOREIGN KEY(PersonId) REFERENCES Person(PersonId)," +
                "PRIMARY KEY(SChuelerId)); ";
            String createClass =
                "CREATE TABLE IF NOT EXISTS Klasse(" +
                "KlasseId INT NOT NULL AUTOINCREMENT," +
                "Bezeichnung VARCHAR(50) NOT NULL," +
                "StartDatum DATE NOT NULL," +
                "EndDatum DATE," +
                "BildungsgangId INT NOT NULL," +
                "PRIMARY KEY(KlasseId)); ";
            String studentHasClass =
                "CREATE TABLE IF NOT EXISTS Schueler_Hat_Klasse(" +
                "SchuelerId INT NOT NULL," +
                "KlasseId INT NOT NULL," +
                "FOREIGN KEY(SchuelerId) REFERENCES Schueler(SchuelerId)," +
                "FOREIGN KEY(KlasseId) REFERENCES Klasse(KlasseId)); ";

            ExecuteQueryList(new string[]{
                createPerson,
                createTeacher,
                createStudent,
                createClass,
                studentHasClass
            });
        }

        private static void ExecuteQuery(string sql) {
            // the (20) will be ignored
            // see https://www.sqlite.org/datatype3.html#affinity_name_examples
            // ExecuteQuery("Create Table if not exists highscores (name varchar(20), score int)");
            ExecuteQueryList(new string[] { sql });
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
                } catch(Exception e) {
                    transaction.Rollback();
                    Console.Error.WriteLine("Query Ausführung abgebrochen.", e);
                    Console.Error.WriteLine("Letzer Query=" + lastQuery);
                    //throw;
                }
            }
            m_dbConnection.Close();
        }


        private static SQLiteConnection OpenConnection() {
            string connectionString = "Data Source=" + databaseFile + ";Version=3;";
            SQLiteConnection m_dbConnection = new SQLiteConnection(connectionString);
            m_dbConnection.Open();
            return m_dbConnection;
        }
    }
}
