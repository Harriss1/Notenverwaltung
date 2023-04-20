using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

// 9.3.: 1 Stunde Create Skripte, 1 Stunde SQLite installieren als Bibliothek.

namespace Notenverwaltung {
    internal class Program {
        static void Main(string[] args) {
            System.Console.WriteLine("Datenbank Anbindung:");
            DatabaseBuilder db = new SqliteDatabaseBuilder();
            //Database database = new Database();
            //database.CreateDatabaseFile();
            Model.InitDatabase(Model.DbType.SQLITE, true);
            Person.InitDatabase(Model.DbType.SQLITE, true);
            System.Console.ReadLine();
        }
    }
}
