using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    internal class Model {
        public enum Type {
            SQLITE,
            MYSQL
        }

        private static Scripts scripts = new SqliteScripts();
        private static Database database = new SqliteDatabase();
        protected static Scripts GetScripts() {
            return scripts;
        }
        protected static Database GetDb() {
            return database;
        }

        public static void InitDatabase(Type type, bool resetData) {
            if (type == Type.SQLITE) {
                Model.scripts = new SqliteScripts();
                Model.database = new SqliteDatabase();
            }
            if (type == Type.MYSQL) {
                // to-do

            }
            database.InitCompleteDatabase(resetData);

        }
    }
}
