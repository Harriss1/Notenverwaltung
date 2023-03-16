using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Notenverwaltung {
    internal abstract class Database {

        public abstract void InitCompleteDatabase(bool resetDatabase = true);
        public abstract object FindFirstById(string tableName, int id);

        ////////////////////////////////////////////////////////////////////////////////////
        /// Verbindungsaufbau und Query-Ausführung
        ////////////////////////////////////////////////////////////////////////////////////

        protected abstract void CreateDatabaseFile();

        protected abstract void ExecuteQueries(string[] queryList);
        protected abstract void ExecuteQuerySolo(string sql);

        protected abstract SQLiteConnection OpenConnection();
        public abstract void ResetDatabase();

        protected abstract void DropAllTables();
        protected abstract void CreateAllTables();
        protected abstract void InsertDemoData();
    }
}
