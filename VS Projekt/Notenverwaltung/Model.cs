using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    internal class Model {
        public enum DbType {
            SQLITE,
            MYSQL
        }

        private static BuilderScripts scripts = new SqliteBuilderScripts();
        private static DatabaseBuilder database = new SqliteDatabaseBuilder();
        protected static BuilderScripts GetScripts() {
            return scripts;
        }
        protected static DatabaseBuilder GetDb() {
            return database;
        }


        public static void InitDatabase(DbType type, bool resetData) {
            if (type == DbType.SQLITE) {
                Model.scripts = new SqliteBuilderScripts();
                Model.database = new SqliteDatabaseBuilder();
            }
            if (type == DbType.MYSQL) {
                // to-do

            }
            database.InitCompleteDatabase(resetData);

        }
    }
}
