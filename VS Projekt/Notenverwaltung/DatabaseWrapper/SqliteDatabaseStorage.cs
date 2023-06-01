using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace Notenverwaltung {
    internal class SqliteDatabaseStorage : DatabaseStorage {
        private SqliteDatabaseBuilder builder = new SqliteDatabaseBuilder();

        public void UpdateSingleEntity(Entity entity) {
            string statement = CreateUpdateStatement(entity);
            TransactUpdateStatement(statement);
        }

        /// <summary>
        /// Create new Entitity via Insert-Statement
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
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
            #pragma warning disable CS0219 // Beispiel des erstellten Statements
            string exampleStatement =
                "INSERT INTO person(Vorname, Nachname, Geburtsdatum, Geburtsort, Benutzername, Passwort)" +
                "VALUES" +
                "('Max', 'Mustermann', '2022-01-01', 'Bielefeld', 'user', 'user');";
            #pragma warning restore CS0219

            // Tabellen-Name
            String insertQuery = "INSERT INTO " + entity.ToTableName() + "(";
            // Tabellen-Spalten
            foreach (KeyValue keyValuePair in entity.ToAttributeValueDescription().GetAttributes()) {
                insertQuery += keyValuePair.GetKey();
                insertQuery += ", ";
            }
            // INSERT INTO Lehre) Values);
            foreach (OneToXRelationKeyValue relation in entity.ToAttributeValueDescription().GetOneToXRelations()) {
                insertQuery += relation.GetOwnForeignColumnName();
                insertQuery += ", ";
            }
            insertQuery = insertQuery.Substring(0, insertQuery.Length - 2);
            insertQuery += ") ";
            // Werte der einzelnen Felder
            insertQuery += "Values (";
            foreach (KeyValue keyValuePair in entity.ToAttributeValueDescription().GetAttributes()) {
                bool useDefault = true;
                if (keyValuePair.GetValue().GetType() == typeof(DateTime)) {
                    DateTime dateTime = (DateTime)keyValuePair.GetValue();
                    string textDate = (new SimpleDate(dateTime)).ToText;
                    Console.WriteLine(" Datum f. DB:" + textDate);
                    insertQuery += "'" + textDate + "'";
                    insertQuery += ", ";
                    useDefault = false;
                }
                if (keyValuePair.GetValue().GetType() == typeof(int)) {
                    int number = (int)keyValuePair.GetValue();
                    insertQuery += "'" + number + "'";
                    insertQuery += ", ";
                    useDefault = false;
                }
                if ( useDefault || 
                    keyValuePair.GetValue().GetType() == typeof(string)) {
                    insertQuery += "'" + keyValuePair.GetValueString() + "'";
                    insertQuery += ", ";
                }
            }
            foreach (OneToXRelationKeyValue relation in entity.ToAttributeValueDescription().GetOneToXRelations()) {
                insertQuery += "'" + relation.GetForeignId() + "'";
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
                "UPDATE Person " +
                "SET Vorname = 'neuer Vorname'," +
                "Nachname = 'neuer Nachname'" +
                "WHERE PersonId = 1;";
            #pragma warning restore CS0219

            // Tabellen-Name
            String updateStatement = "UPDATE " + entity.ToTableName() + " ";
            updateStatement += "SET ";

            // Neue Feld-Werte setzen
            foreach (KeyValue keyValuePair in entity.ToAttributeValueDescription().GetAttributes()) {
                updateStatement += keyValuePair.GetKey();
                updateStatement += " = ";
                object value = keyValuePair.GetValue();
                if(value.GetType() == typeof(string)) {

                }
                updateStatement += "'" + keyValuePair.GetValueString() + "'";
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
            TransactSimpleStatement(statement);
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
        private void TransactSimpleStatement(string deleteStatement) {
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

        public AttributeToValuesDescription FindById(int id, Entity entity) {
            string query = MakeFindByIdQuery(id, entity);
            AttributeToValuesDescription retrievedDescription =
                TransactFindQuery(query, entity);

            ////////// Eins-zu-Viele oder ähnliches Beziehungen!
            /// TODO
            

            ////////////////////// Viele-Zu-Viele Beziehungen
            ///Append Many To Many Related Entities with this Method (modular)
            retrievedDescription = SearchAndAppendManyToManyRelations(retrievedDescription, entity, id);
            
            return retrievedDescription;
        }

        private AttributeToValuesDescription SearchAndAppendManyToManyRelations(AttributeToValuesDescription retrievedDescription, Entity entity, int id) {
            if (entity.ToAttributeValueDescription().GetAllManyToManyRelations().Count > 0) {
                System.Console.WriteLine("################ Behandle Relationen");
                foreach (ManyToManyKeyValue relationDescription
                    in entity.ToAttributeValueDescription().GetAllManyToManyRelations()) {
                    string tablename = relationDescription.GetTablename();
                    string query = MakeManyToManySearchQuery(tablename, entity, id);
                    string ownColumn = relationDescription.GetOwnColumn();
                    string foreignColumn = relationDescription.GetForeignColumn();
                    retrievedDescription = TransactManyToManySearch(retrievedDescription, query,
                        tablename, ownColumn, foreignColumn);
                }
            }
            return retrievedDescription;
        }

        private AttributeToValuesDescription TransactManyToManySearch(
                        AttributeToValuesDescription retrievedDescription,
                        string query,
                        string tablename,
                        string ownColumn, 
                        string foreignColumn) {
            System.Console.WriteLine("query=" + query);
            System.Console.WriteLine("tablename=" + tablename);
            System.Console.WriteLine("primaryColumn=" + ownColumn);
            System.Console.WriteLine("secondaryColumn=" + foreignColumn);
            List<int> foreignKeys = new List<int>();
            SQLiteConnection dbConnection = builder.OpenConnection();
            
            builder.OpenConnection();
            using (SQLiteTransaction transaction = dbConnection.BeginTransaction()) {
                try {
                    SQLiteCommand command = new SQLiteCommand(query, dbConnection);

                    SQLiteDataReader rdr = command.ExecuteReader();
                    if (!rdr.HasRows) {
                        throw new InvalidDataException("Es wurde keine Datensätze gefunden");
                    }
                    while (rdr.Read()) {
                        int ownColumnId = -1;
                        int foreignColumnId = -1;
                        System.Console.WriteLine("1=" + rdr[ownColumn].GetType().ToString());
                        System.Console.WriteLine("2=" + rdr[foreignColumn].GetType().ToString());
                        if (rdr[ownColumn].GetType() == typeof(Int64)
                            && rdr[foreignColumn].GetType() == typeof(Int64)) {
                            ownColumnId = Convert.ToInt32(rdr[ownColumn]);
                            foreignColumnId = Convert.ToInt32(rdr[foreignColumn]);
                            foreignKeys.Add(foreignColumnId);
                        }
                        else {
                            throw new InvalidDataException("Many-To-Many Tabellen benötigen " +
                                "zwei Spalten jeweils mit Integer befüllt");
                        }
                        /// Okay Problem erkannt:
                        /// In der AttributeToValueDescription hätte ich auf keinen Fall
                        /// Model-Objekte ablegen dürfen! Dann fehlt nämlich der eindeutige
                        /// Tabellen/Spalten-Bezeichner.
                    }
                    transaction.Commit();

                    ManyToManyKeyValue relationDescription = new ManyToManyKeyValue(
                        tablename,
                        ownColumn,
                        foreignColumn,
                        foreignKeys
                        );
                    retrievedDescription.AddManyToManyRelation(relationDescription);
                }
                catch (System.FormatException e) {
                    transaction.Rollback();
                    Console.Error.WriteLine("Query Ausführung abgebrochen." + e);
                    Console.Error.WriteLine("Letzer Query=" + query);
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine("Es existert ein falscher Datumswert der Datenbank");
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
            return retrievedDescription;
        }

        private string MakeManyToManySearchQuery(string tablename, Entity entity, int id) {
            string query = "SELECT * FROM " + tablename
                + " WHERE " + entity.ToAttributeValueDescription().primaryKey
                + " = " + id
                + ";";
            return query;
        }
        private string MakeFindByIdQuery(int id, Entity entity) {
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

            ////////////////////// Viele-Zu-Viele Beziehungen
            ///Append Many To Many Related Entities with this Method (modular)
            retrievedDescription = SearchAndAppendManyToManyRelations(retrievedDescription, entity,
                retrievedDescription.primaryKeyValue);

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
                        foreach (KeyValue keyValuePair in entityDescription.GetAttributes()) {
                            string fieldContent = "";
                            object o = rdr[keyValuePair.GetKey()].GetType();
                            System.Console.WriteLine("Type = " + o.ToString());
                            if (rdr[keyValuePair.GetKey()].GetType() == typeof(string)) {
                                fieldContent = (string)rdr[keyValuePair.GetKey()];
                                retrievedDescription.AddStringAttribute(keyValuePair.GetKey(), fieldContent);
                            }
                            if (rdr[keyValuePair.GetKey()].GetType() == typeof(DateTime)) {
                                DateTime dateTime = (DateTime)rdr[keyValuePair.GetKey()];
                                fieldContent = dateTime.ToString("yyyy-MM-dd");
                                retrievedDescription.AddDateTimeAttribute(keyValuePair.GetKey(), dateTime);
                            }
                            if (rdr[keyValuePair.GetKey()].GetType() == typeof(Int64)) {                                
                                fieldContent = rdr[keyValuePair.GetKey()].ToString();
                                int number = int.Parse(fieldContent);
                                retrievedDescription.AddIntegerAttribute(keyValuePair.GetKey(), number);
                            }
                            debugContent.Add(fieldContent);
                        }
                        if (entity.ToAttributeValueDescription().GetOneToXRelations().Count > 0) {
                            //Console.WriteLine("### Relationen existieren!");
                            foreach (OneToXRelationKeyValue relationDescription 
                                        in entity.ToAttributeValueDescription().GetOneToXRelations()) {
                                //Console.WriteLine("Relationsdetails: " + relationDescription.GetOwnForeignColumnName() + " | " +
                                    //relationDescription.GetForeignId());
                                int foreignKey = int.Parse(rdr[relationDescription.GetOwnForeignColumnName()].ToString());
                                OneToXRelationKeyValue updatedDescription = new OneToXRelationKeyValue(
                                    relationDescription.GetForeignTable(),
                                    relationDescription.GetForeignPrimaryColumnName(),
                                    relationDescription.GetOwnForeignColumnName(),
                                    foreignKey
                                    );
                                //Console.WriteLine("### Gefundener foreign Key=" + foreignKey);
                                retrievedDescription.AddOneToXRelation(updatedDescription);
                            }
                        }

                    }
                    transaction.Commit();
                }
                catch (System.FormatException e) {
                    transaction.Rollback();
                    Console.Error.WriteLine("Query Ausführung abgebrochen." + e);
                    Console.Error.WriteLine("Letzer Query=" + query);
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.Error.WriteLine("Es existert ein falscher Datumswert der Datenbank");
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
                + " LIKE '" + keyValuePair.GetValueString()
                + "';";
            return query;
        }

        public void InsertManyToManyRelationsIfMissing(Entity entity) {
            AttributeToValuesDescription description = entity.ToAttributeValueDescription();
            if (description.GetAllManyToManyRelations().Count > 0) {
                foreach (ManyToManyKeyValue relationDescription in description.GetAllManyToManyRelations()) {
                    int primaryKey = entity.id;
                    HandleManyToManyRelation(relationDescription, primaryKey);
                }
            }
        }

        private void HandleManyToManyRelation(ManyToManyKeyValue relationDescription, int primaryKey) {
            string tablename = relationDescription.GetTablename();
            string ownColumn = relationDescription.GetOwnColumn();
            string foreignColumn = relationDescription.GetForeignColumn();
            foreach (int foreignKey in relationDescription.GetForeignIds()) {
                if (IsManyToManyEntryMissing(tablename, ownColumn, foreignColumn, primaryKey, foreignKey)) {
                    string statement = 
                        MakeManyToManyInsertStatement(tablename, ownColumn, foreignColumn, primaryKey, foreignKey);
                    Console.WriteLine("manyToMany Statement =" + statement);
                    TransactSimpleStatement(statement);
                }
            }
        }

        private string MakeManyToManyInsertStatement(string tablename, string ownColumn, string foreignColumn, int primaryKey, int foreignKey) {
            string exampleQuery = "INSERT 	INTO Schueler_Hat_Klasse (SchuelerId, KlasseId) " +
                                  "VALUES " +
                                  "(2, 2); ";
            string statement = "INSERT 	INTO " + tablename
                + " ("
                + ownColumn + ", "
                + foreignColumn
                + ") VALUES ";
                statement += "(" + primaryKey + ", " + foreignKey + ");";
            return statement;
        }

        private bool IsManyToManyEntryMissing(string tablename, string ownColumn, 
            string foreignColumn, int primaryKey, int foreignKey) {
            string exampleQuery = "SELECT * FROM Schueler_Hat_Klasse " +
                                  "WHERE SchuelerId = 2 AND KlasseId = 2; ";
            string query = "SELECT * FROM " + tablename
                + " WHERE " 
                + foreignColumn + " = " + foreignKey
                + " AND "
                + ownColumn + " = " + primaryKey + ";";
            bool result = false;
            SQLiteConnection dbConnection = builder.OpenConnection();
            using (SQLiteTransaction transaction = dbConnection.BeginTransaction()) {
                try {
                    SQLiteCommand command = new SQLiteCommand(query, dbConnection);
                    SQLiteDataReader rdr = command.ExecuteReader();
                    if (!rdr.HasRows) {
                        Console.WriteLine("Erfolgreich festgestellt, dass Beziehung nicht gesetzt ist");
                        result = true;
                    }
                    while (rdr.Read()) {
                        string ownColumnText = (string)rdr[ownColumn];
                        string foreignColumnText = (string)rdr[foreignColumn];
                        System.Console.WriteLine("Gefundene N-M-Beziehung:" + ownColumn + "=" + ownColumnText
                            + foreignColumn + "=" + foreignColumnText);
                    }
                    transaction.Commit();
                }
                catch (Exception e) {
                    transaction.Rollback();
                    Console.Error.WriteLine("Query Ausführung abgebrochen." + e);
                    Console.Error.WriteLine("Letzer Query=" + query);
                    //throw;
                }
            }
            dbConnection.Close();
            return result;
        }
    }
}