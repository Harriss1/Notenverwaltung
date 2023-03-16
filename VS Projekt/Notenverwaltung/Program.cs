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
            //Database database = new Database();
            //database.CreateDatabaseFile();
            Model.InitDatabase(Model.Type.SQLITE, true);
            System.Console.ReadLine();
        }
    }
}
