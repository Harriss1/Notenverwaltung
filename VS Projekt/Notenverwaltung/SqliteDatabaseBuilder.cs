using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// https://learn.microsoft.com/en-us/visualstudio/ide/default-keyboard-shortcuts-in-visual-studio?view=vs-2022
namespace Notenverwaltung {
    internal class SqliteDatabaseBuilder : DatabaseBuilder {
        public SqliteDatabaseBuilder() {
            DatabaseBuilder.type = "lite";
        }

        private BuilderScripts scripts = new SqliteBuilderScripts();

        private const string databaseFile = "GradeDb.sqlite";
        public override void InitCompleteDatabase(bool resetDatabase = true) {
            if (resetDatabase) {
                ResetDatabase();
            }
            // erstellt die Datenbank-Datei falls diese noch nicht existiert
            CreateDatabaseFile();
            // erstellt alle verwendeten Tabellen falls diese noch nicht existieren
            CreateAllTables();
            // Fügt Beispieldaten ein um grundlegende Funktionalitäten zu testen.
            InsertDemoData();

            List<string> responseList = GetQueryReaderList("Select * From Lehrer Join Person ON Lehrer.PersonId = Person.PersonId;");
            for (int i = 0; i < responseList.Count; i++) {
                string sql = responseList[i];
                System.Console.WriteLine("Querycontent=" + sql);
            }
            //GetQueryReaderList((new Person).?);
        }

        ////////////////////////////////////////////////////////////////////////////////////
        /// Verbindungsaufbau und Query-Ausführung
        ////////////////////////////////////////////////////////////////////////////////////

        protected override void CreateDatabaseFile() {
            // Abfrage um ein versehentliches Löschen zu vermeiden
            if (!File.Exists(databaseFile)) {
                // this creates a zero-byte file
                SQLiteConnection.CreateFile(databaseFile);
            }

        }

        protected override void ExecuteQueries(string[] queryList) {
            SQLiteConnection dbConnection = OpenConnection();
            using (SQLiteTransaction transaction = dbConnection.BeginTransaction()) {
                string currentQuery = "";
                try {

                    foreach (string query in queryList) {
                        currentQuery = query;
                        SQLiteCommand command = new SQLiteCommand(query, dbConnection);
                        command.ExecuteNonQuery();

                    }
                    transaction.Commit();
                }
                catch (Exception e) {
                    transaction.Rollback();
                    Console.Error.WriteLine("Query Ausführung abgebrochen.", e);
                    Console.Error.WriteLine("Letzer Query=" + currentQuery);
                    //throw;
                }
            }
            dbConnection.Close();
        }
        protected override void ExecuteQuerySolo(string sql) {
            ExecuteQueries(new string[] { sql });
        }

        protected override SQLiteConnection OpenConnection() {
            string connectionString = "Data Source=" + databaseFile + ";Version=3;";
            SQLiteConnection m_dbConnection = new SQLiteConnection(connectionString);
            m_dbConnection.Open();
            return m_dbConnection;
        }



        ////////////////////////////////////////////////////////////////////////////////////
        /// Tables: Create and Delete + Insert Demodata
        ////////////////////////////////////////////////////////////////////////////////////
        

        ///<summary>
        /// Erstellt eine Liste an Query-Antworten die das Resultat der
        /// Datenbank-Abfrage enthalten.
        /// </summary>
        private List<string> GetQueryReaderList(string query) {
            List<string> readerContent = new List<string>();
            SQLiteConnection dbConnection = OpenConnection();
            using (SQLiteTransaction transaction = dbConnection.BeginTransaction()) {
                string currentQuery = "";
                try {
                        currentQuery = query;
                        SQLiteCommand command = new SQLiteCommand(query, dbConnection);

                        SQLiteDataReader rdr = command.ExecuteReader();
                    while (rdr.Read()) {
                        string foreName = (string)rdr["Vorname"];
                        List<string> columnContent = new List<string>();
                        for (int i = 0; i < rdr.FieldCount; i++) {
                            columnContent.Add(rdr.GetValue(i).ToString());
                        }
                        Console.WriteLine("Response for query " + query + " = " + foreName);
                        foreach (string column in columnContent) {
                            Console.WriteLine("Alle Columns=" + column);
                        }
                        readerContent.Add(foreName);
                    }
                    transaction.Commit();
                }
                catch (Exception e) {
                    transaction.Rollback();
                    Console.Error.WriteLine("Query Ausführung abgebrochen.", e);
                    Console.Error.WriteLine("Letzer Query=" + currentQuery);
                    //throw;
                }
            }
            dbConnection.Close();
            return readerContent;
        }


        /// <summary>
        /// Löschung der DB-Datei und Neuerstellung aller Tabellen
        /// </summary>
        public override void ResetDatabase() {
            //SQLiteConnection.CreateFile(databaseFile);
            DropAllTables();
            CreateAllTables();
        }
        protected override void DropAllTables() {
            ExecuteQuerySolo(scripts.DropAllTables());
        }
        protected override void CreateAllTables() {
            ExecuteQueries(scripts.CreateAllTables());
        }
        protected override void InsertDemoData() {
            ExecuteQueries(scripts.InsertDemoData());
        }

        ////////////////////////////////////////////////////////////////////////////////////
        /// Create, Update, Delete Specific Rows
        ////////////////////////////////////////////////////////////////////////////////////
        ///
        private void Insert(string sql) {

        }

        private string Get(string sql) {
            return "";
        }

        private void Delete(string sql) {

        }

        public override object FindFirstById(string tableName, int id) {
            return null;
        }

        private string SearchAll(string sql) {
            return "";
        }
    }
}
