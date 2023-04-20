﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Notenverwaltung {
    internal abstract class DatabaseBuilder {
        public static String type;
        public static DatabaseBuilder retrieveCurrentDb() {
            if(type == "lite") {
                return new SqliteDatabaseBuilder();
            }
            return null;
        }

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

        //cd.buy(10);
        //store.buy(cd,10);
    }
}