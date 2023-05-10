using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace Notenverwaltung {
    internal class SqliteDatabaseStorage : DatabaseStorage {
        private SqliteDatabaseBuilder builder = new SqliteDatabaseBuilder();

        public void Create(EntityMapperDeprecated entity) {
            string tablename = entity.ToTableName();
            //string person =
            //    "INSERT INTO person(Vorname, Nachname, Geburtsdatum, Geburtsort, Benutzername, Passwort)" +
            //    "VALUES" +
            //    "('Max', 'Mustermann', '2022-01-01', 'Bielefeld', 'user', 'user')," +
            //    "('Paula', 'Pause', '1990-12-05', 'Münchhausen', 'guest', 'guest')," +
            //    "('Admin', 'Nistrator', '1900-01-01', 'Münchhausen', 'admin', 'admin')," +
            //    "('Herbert', 'Herbtraube', '1940-03-19', 'Münchhausen', 'teacher', 'teacher'); ";
            string firstname ="";
            string lastname = "";
            string birthdate = "";
            string birthplace = "";
            string username = "";
            string password = "";

            if(tablename.Equals(TableNames.person)) {
                firstname = GetValue(entity.ToKeyValue(), TableNames.PersonAttr.firstname);
                lastname = GetValue(entity.ToKeyValue(), TableNames.PersonAttr.lastname);
                birthplace = GetValue(entity.ToKeyValue(), TableNames.PersonAttr.birthplace);
                username = GetValue(entity.ToKeyValue(), TableNames.PersonAttr.username);
                password = GetValue(entity.ToKeyValue(), TableNames.PersonAttr.password);
                birthdate = GetValue(entity.ToKeyValue(), TableNames.PersonAttr.birthdate);
                string query = "INSERT INTO person(Vorname, Nachname, Geburtsdatum, Geburtsort, Benutzername, Passwort)" +
                "VALUES" +
                "('" 
                + firstname +
                "', '" +
                lastname +
                "', '" +
                birthdate +
                "', '" +
                birthplace +
                "', '" +
                username +
                "', '" +
                password +
                "')";
            }

            //string teacher =
            //    "INSERT INTO Lehrer (PersonId)" +
            //    "VALUES" +
            //    "(3)," +
            //    "(4); ";
            

            SQLiteConnection dbConnection = builder.OpenConnection();
            using (SQLiteTransaction transaction = dbConnection.BeginTransaction()) {
                string query = "";
                try {
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
                    }
                    transaction.Commit();
                }
                catch (Exception e) {
                    transaction.Rollback();
                    Console.Error.WriteLine("Query Ausführung abgebrochen.", e);
                    //throw;
                }
            }
            dbConnection.Close();
            throw new System.NotImplementedException();
        }

        private string GetValue(List<KeyValue> keyValueList, string firstname) {
            throw new NotImplementedException();
        }

        public void Delete(EntityMapperDeprecated entity) {
            throw new System.NotImplementedException();
        }

        public object FindTeacherById(string tableName, int id) {
            throw new System.NotImplementedException();
            //return null;
        }

        public object FindTeacherByLastname(string tableName, string lastname) {
            throw new System.NotImplementedException();
        }

        public string GetInfo() {
            return "Info on this object";
        }

        public EntityMapperDeprecated Update(EntityMapperDeprecated entity) {
            throw new System.NotImplementedException();
        }

        public void UpdateSingleEntity(Entity entity) {
            string statement = CreateUpdateStatement(entity);
            TransactUpdateStatement(statement);
        }

        public int InsertSingleEntity(Entity entity) {
            string insertStatement = CreateInsertStatement(entity);
            int lastRowId = TransactInsertStatement(insertStatement);
            System.Console.WriteLine("LastInsertId=" + lastRowId.ToString());
            return lastRowId;
        }

        /// <summary>
        /// Erstellt ein Insert-Statement anhand der Key-Value-Paare welche
        /// für die übergebene Entity-Instanz festegelegt wurden, und
        /// nutzt alle Attributswerte zur Bestimmung des einzufügenden
        /// Datensatzinhalts.
        /// </summary>
        /// <param name="entity">Konkretisierung des Entity-Interfaces</param>
        /// <returns>Id der neu eingefügten Zeile</returns>
        private static string CreateInsertStatement(Entity entity) {
            String exampleStatement = 
                "INSERT INTO person(Vorname, Nachname, Geburtsdatum, Geburtsort, Benutzername, Passwort)" +
                "VALUES" +
                "('Max', 'Mustermann', '2022-01-01', 'Bielefeld', 'user', 'user');";

            // Tabellen-Name
            String insertQuery = "INSERT INTO " + entity.ToTableName() + "(";
            // Tabellen-Spalten
            foreach (KeyValue keyValuePair in entity.ToKeyValue()) {
                insertQuery += keyValuePair.GetKey();
                insertQuery += ", ";
            }
            insertQuery = insertQuery.Substring(0, insertQuery.Length - 2);
            insertQuery += ") ";
            // Werte der einzelnen Felder
            insertQuery += "Values (";
            foreach (KeyValue keyValuePair in entity.ToKeyValue()) {
                insertQuery += "'" + keyValuePair.GetValue() + "'";
                insertQuery += ", ";
            }
            insertQuery = insertQuery.Substring(0, insertQuery.Length - 2);
            insertQuery += ");";

            return insertQuery;
        }

        /// <summary>
        /// Erstellt ein Insert-Statement anhand der Key-Value-Paare welche
        /// für die übergebene Entity-Instanz festegelegt wurden, und
        /// nutzt alle Attributswerte zur Bestimmung des einzufügenden
        /// Datensatzinhalts.
        /// </summary>
        /// <param name="entity">Konkretisierung des Entity-Interfaces</param>
        /// <returns>Id der neu eingefügten Zeile</returns>
        private static string CreateUpdateStatement(Entity entity) {
            String exampleStatement =
                "UPDATE Person" +
                "SET Vorname = 'neuer Vorname'," +
                "Nachname = 'neuer Nachname'" +
                "WHERE PersonId = 1;";

            // Tabellen-Name
            String updateStatement = "UPDATE " + entity.ToTableName() + " ";
            updateStatement += "SET ";

            // Neue Feld-Werte setzen
            foreach (KeyValue keyValuePair in entity.ToKeyValue()) {
                updateStatement += keyValuePair.GetKey();
                updateStatement += " = ";
                updateStatement += "'" + keyValuePair.GetValue() + "'";
                updateStatement += ", ";
            }
            updateStatement = updateStatement.Substring(0, updateStatement.Length - 2);

            // WHERE
            updateStatement += " WHERE ";
            updateStatement += entity.ToPrimaryKeyColumnName() 
                + " = " + entity.id
                + ";";
            return updateStatement;
        }

        /// <summary>
        /// Insert Statement für eine einzelne neue Zeile ausführen
        /// </summary>
        /// <param name="insertStatement">Sqlite Insert Statement</param>
        /// <returns>Id der neu eingefügten Zeile, oder -1 falls Fehler</returns>
        private int TransactInsertStatement(string insertStatement) {
            int lastRowId = -1;
            SQLiteConnection dbConnection = builder.OpenConnection();
            using (SQLiteTransaction transaction = dbConnection.BeginTransaction()) {
                try {
                    // Ausführung Insert-Statement
                    SQLiteCommand commandInsertQuery = new SQLiteCommand(insertStatement, dbConnection);
                    commandInsertQuery.ExecuteNonQuery();
                    
                    // Ermittlte Id der neu eingefügten Zeile
                    SQLiteCommand commandLastRowId = new SQLiteCommand("SELECT last_insert_rowid();", dbConnection);
                    Int64 LastRowID64 = (Int64)commandLastRowId.ExecuteScalar();
                    lastRowId = (int)LastRowID64;
                    transaction.Commit();
                }
                catch (Exception e) {
                    transaction.Rollback();
                    Console.WriteLine("SQL-Transaction abgebrochen.", e);
                    Console.WriteLine("Letzes Statement=" + insertStatement);
                    //throw;
                }
            }
            dbConnection.Close();
            return lastRowId;
        }

        /// <summary>
        /// Update Statement für eine einzelne neue Zeile ausführen
        /// </summary>
        /// <param name="updateStatement">Sqlite Insert Statement</param>
        private void TransactUpdateStatement(string updateStatement) {
            SQLiteConnection dbConnection = builder.OpenConnection();
            using (SQLiteTransaction transaction = dbConnection.BeginTransaction()) {
                try {
                    // Ausführung Insert-Statement
                    SQLiteCommand commandInsertQuery = new SQLiteCommand(updateStatement, dbConnection);
                    commandInsertQuery.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception e) {
                    transaction.Rollback();
                    Console.WriteLine("SQL-Transaction abgebrochen.", e);
                    Console.WriteLine("Letzes Statement=" + updateStatement);
                    //throw;
                }
            }
            dbConnection.Close();
        }

        private void GetLastId() {
            // get last id
            string content = "";
            string query = "SELECT last_insert_rowid();";
            List<string> readerContent = new List<string>();
            SQLiteConnection dbConnection = builder.OpenConnection();
            using (SQLiteTransaction transaction = dbConnection.BeginTransaction()) {
                try {
                    SQLiteCommand command = new SQLiteCommand(query, dbConnection);

                    Int64 LastRowID64 = (Int64)command.ExecuteScalar();
                    int LastRowID = (int)LastRowID64;
                    content = LastRowID.ToString();
                    transaction.Commit();
                }
                catch (Exception e) {
                    transaction.Rollback();
                    Console.Error.WriteLine("Query Ausführung abgebrochen.", e);
                    Console.Error.WriteLine("Letzer Query=" + query);
                    //throw;
                }
            }
            dbConnection.Close();
            System.Console.WriteLine("LastInsertId=" + content);
        }

        public void Delete(Entity entity) {
            string statement = CreateDeleteStatement(entity);
            TransactDeleteStatement(statement);
            System.Console.WriteLine("stmnt=" + statement);
        }

        private string CreateDeleteStatement(Entity entity) {
            //DELETE FROM Person
            //WHERE
            //PersonId = 5;
            string statement = "DELETE FROM ";
            statement += entity.ToTableName() + " ";
            statement += "WHERE ";
            statement += entity.ToPrimaryKeyColumnName();
            statement += " = " + entity.id + ";";
            return statement;
        }
        private void TransactDeleteStatement(string deleteStatement) {
            SQLiteConnection dbConnection = builder.OpenConnection();
            using (SQLiteTransaction transaction = dbConnection.BeginTransaction()) {
                try {
                    // Ausführung Insert-Statement
                    SQLiteCommand commandInsertQuery = new SQLiteCommand(deleteStatement, dbConnection);
                    commandInsertQuery.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception e) {
                    transaction.Rollback();
                    Console.WriteLine("SQL-Transaction abgebrochen.", e);
                    Console.WriteLine("Letzes Statement=" + deleteStatement);
                    //throw;
                }
            }
            dbConnection.Close();
        }
    }
}