using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace Notenverwaltung {
    internal class SqliteDatabaseStorage : DatabaseStorage {
        private SqliteDatabaseBuilder builder = new SqliteDatabaseBuilder();

                private string GetValue(List<KeyValue> keyValueList, string firstname) {
            throw new NotImplementedException();
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

        public void UpdateSingleEntity(Entity entity) {
            string statement = CreateUpdateStatement(entity);
            TransactUpdateStatement(statement);
        }

        public int InsertSingleEntity(Entity entity) {
            string insertStatement = CreateInsertStatementByContainer(entity);
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
            #pragma warning disable CS0219 // Beispiel des erstellten Statements
            string exampleStatement = 
                "INSERT INTO person(Vorname, Nachname, Geburtsdatum, Geburtsort, Benutzername, Passwort)" +
                "VALUES" +
                "('Max', 'Mustermann', '2022-01-01', 'Bielefeld', 'user', 'user');";
            #pragma warning restore CS0219

            // Tabellen-Name
            String insertQuery = "INSERT INTO " + entity.ToTableName() + "(";
            // Tabellen-Spalten
            foreach (KeyValue keyValuePair in entity.ToAttributeValueDescription().GetList()) {
                insertQuery += keyValuePair.GetKey();
                insertQuery += ", ";
            }
            insertQuery = insertQuery.Substring(0, insertQuery.Length - 2);
            insertQuery += ") ";
            // Werte der einzelnen Felder
            insertQuery += "Values (";
            foreach (KeyValue keyValuePair in entity.ToAttributeValueDescription().GetList()) {
                insertQuery += "'" + keyValuePair.GetValue() + "'";
                insertQuery += ", ";
            }
            insertQuery = insertQuery.Substring(0, insertQuery.Length - 2);
            insertQuery += ");";

            return insertQuery;
        }
        private static string CreateInsertStatementByContainer(Entity entity) {
            #pragma warning disable CS0219 // Beispiel des erstellten Statements
            string exampleStatement =
                "INSERT INTO person(Vorname, Nachname, Geburtsdatum, Geburtsort, Benutzername, Passwort)" +
                "VALUES" +
                "('Max', 'Mustermann', '2022-01-01', 'Bielefeld', 'user', 'user');";
            #pragma warning restore CS0219

            // Tabellen-Name
            String insertQuery = "INSERT INTO " + entity.ToTableName() + "(";
            // Tabellen-Spalten
            foreach (KeyValue keyValuePair in entity.ToAttributeValueDescription().GetList()) {
                insertQuery += keyValuePair.GetKey();
                insertQuery += ", ";
            }
            insertQuery = insertQuery.Substring(0, insertQuery.Length - 2);
            insertQuery += ") ";
            // Werte der einzelnen Felder
            insertQuery += "Values (";
            foreach (KeyValue keyValuePair in entity.ToAttributeValueDescription().GetList()) {
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
            #pragma warning disable CS0219 // Beispiel des erstellten Statements
            string exampleStatement =
                "UPDATE Person" +
                "SET Vorname = 'neuer Vorname'," +
                "Nachname = 'neuer Nachname'" +
                "WHERE PersonId = 1;";
            #pragma warning restore CS0219

            // Tabellen-Name
            String updateStatement = "UPDATE " + entity.ToTableName() + " ";
            updateStatement += "SET ";

            // Neue Feld-Werte setzen
            foreach (KeyValue keyValuePair in entity.ToAttributeValueDescription().GetList()) {
                updateStatement += keyValuePair.GetKey();
                updateStatement += " = ";
                updateStatement += "'" + keyValuePair.GetValue() + "'";
                updateStatement += ", ";
            }
            updateStatement = updateStatement.Substring(0, updateStatement.Length - 2);

            // WHERE
            updateStatement += " WHERE ";

            updateStatement += entity.ToAttributeValueDescription().primaryKey
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
            statement += entity.ToAttributeValueDescription().primaryKey;
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

        /// <summary>
        /// Erstellt eine Liste an Query-Antworten die das Resultat der
        /// Datenbank-Abfrage enthalten.
        /// "Select * From Lehrer Join Person ON Lehrer.PersonId = Person.PersonId;"
        /// 
        /// Nur als Referenz hier belassen (deprecated)
        /// </summary>
        private List<string> GetQueryReaderList(string query) {
            List<string> readerContent = new List<string>();
            SQLiteConnection dbConnection = builder.OpenConnection();
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

        public AttributeToValuesDescription FindById(int id, Entity entity) {
            string query = CreateFindByIdQuery(id, entity);
            AttributeToValuesDescription retrievedDescription =
                TransactFindQuery(query, entity);
            return retrievedDescription;
        }

        private string CreateFindByIdQuery(int id, Entity entity) {
            //string exampleQueryRelation = "Select * From Lehrer Join Person ON Lehrer.PersonId = Person.PersonId;";
            //string exampleQuery = "Select * From Person WHERE PersonId =99;";
            string query 
                = "SELECT * FROM " + entity.ToTableName() 
                + " WHERE " + entity.ToAttributeValueDescription().primaryKey
                + " = " + id
                + ";";
            return query;
        }

        public AttributeToValuesDescription FindByKeyValue(KeyValue keyValueParam, Entity entity) {
            string query = CreateFindByStringAttributeQuery(keyValueParam, entity);
            AttributeToValuesDescription retrievedDescription =
                TransactFindQuery(query, entity);
            return retrievedDescription;
        }

        private AttributeToValuesDescription TransactFindQuery(string query, Entity entity) {
            System.Console.WriteLine("query = " + query);
            AttributeToValuesDescription entityDescription = entity.ToAttributeValueDescription();
            AttributeToValuesDescription retrievedDescription
                = new AttributeToValuesDescription(
                    entityDescription.primaryKey,
                    entityDescription.primaryKeyValue);

            List<string> debugContent = new List<string>();
            SQLiteConnection dbConnection = builder.OpenConnection();
            using (SQLiteTransaction transaction = dbConnection.BeginTransaction()) {
                try {
                    SQLiteCommand command = new SQLiteCommand(query, dbConnection);

                    SQLiteDataReader rdr = command.ExecuteReader();
                    if (!rdr.HasRows) {
                        Console.WriteLine("Datensatz exisitert nicht für ID = " + entityDescription.primaryKeyValue);
                        return null;
                    }
                    while (rdr.Read()) {
                        foreach (KeyValue keyValuePair in entityDescription.GetList()) {
                            string fieldContent = "";
                            object o = rdr[keyValuePair.GetKey()].GetType();
                            System.Console.WriteLine("Type = " + o.ToString());
                            if (rdr[keyValuePair.GetKey()].GetType().ToString().Equals("System.String")) {
                                fieldContent = (string)rdr[keyValuePair.GetKey()];
                            }
                            if (rdr[keyValuePair.GetKey()].GetType().ToString().Equals("System.DateTime")) {
                                DateTime dateTime = (DateTime)rdr[keyValuePair.GetKey()];
                                fieldContent = dateTime.ToString("yyyy-MM-dd");
                            }
                            if (rdr[keyValuePair.GetKey()].GetType().ToString().Equals("System.Integer")) {
                                fieldContent = rdr[keyValuePair.GetKey()].ToString();
                            }
                            retrievedDescription.Add(keyValuePair.GetKey(), fieldContent);
                            debugContent.Add(fieldContent);
                        }
                    }
                    transaction.Commit();
                }
                catch (System.FormatException e) {
                    transaction.Rollback();
                    Console.Error.WriteLine("Query Ausführung abgebrochen." + e);
                    Console.Error.WriteLine("Letzer Query=" + query);
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine("Es wurde ein Datumswert falsch in die Datenbank geschrieben!");
                    Console.BackgroundColor = ConsoleColor.Black;

                }
                catch (System.InvalidCastException e) {
                    transaction.Rollback();
                    Console.Error.WriteLine("Query Ausführung abgebrochen." + e);
                    Console.Error.WriteLine("Letzer Query=" + query);
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine("Format wurde falsch konvertiert (z.B. Date nicht in String umgewandelt)");
                    Console.BackgroundColor = ConsoleColor.Black;

                }
                catch (Exception e) {
                    transaction.Rollback();
                    Console.Error.WriteLine("Query Ausführung abgebrochen." + e);
                    Console.Error.WriteLine("Letzer Query=" + query);
                    //throw;
                }
            }
            dbConnection.Close();

            for (int i = 0; i < debugContent.Count; i++) {
                string sql = debugContent[i];
                System.Console.WriteLine("FieldContents=" + sql);
            }
            return retrievedDescription;
        }

        private string CreateFindByStringAttributeQuery(KeyValue keyValuePair, Entity entity) {
            //string exampleQuery = "Select * From Person WHERE Vorname = Hermine;";
            string query
                = "SELECT * FROM " + entity.ToTableName()
                + " WHERE " + keyValuePair.GetKey()
                + " LIKE '" + keyValuePair.GetValue()
                + "';";
            return query;
        }
    }
}