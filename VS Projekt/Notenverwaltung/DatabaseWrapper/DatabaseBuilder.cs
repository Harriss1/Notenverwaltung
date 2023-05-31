using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Notenverwaltung {
    /// <summary>
    /// Zuständig um eine Datenbank zu erstellen und deren
    /// Tabellen, einschließlich Beziehungen zu kreiieren sowie zu löschen.
    /// 
    /// Desweitern werden einige Beispieldatensätze eingefügt.
    /// </summary>
    internal abstract class DatabaseBuilder {
        public static String type;
        public static DatabaseBuilder retrieveCurrentDb() {
            if(type == "lite") {
                return new SqliteDatabaseBuilder();
            }
            return null;
        }

        public abstract void InitCompleteDatabase(bool resetDatabase = true);

        ////////////////////////////////////////////////////////////////////////////////////
        /// Verbindungsaufbau und Query-Ausführung
        ////////////////////////////////////////////////////////////////////////////////////

        protected abstract void CreateDatabaseFile();

        protected abstract void ExecuteStatements(string[] queryList);
        protected abstract void ExecuteSoloStatement(string sql);

        public abstract SQLiteConnection OpenConnection();
        public abstract void ResetDatabase();

        protected abstract void DropAllTables();
        protected abstract void CreateAllTables();
        protected abstract void InsertDemoData();

        //cd.buy(10);
        //store.buy(cd,10);
    }
}
