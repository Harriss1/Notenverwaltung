using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

// 9.3.: 1 Stunde Create Skripte, 1 Stunde SQLite installieren als Bibliothek.
/// <summary>
/// Achtung: Projekt nutzt Dependency Injection als Pattern
/// </summary>
namespace Notenverwaltung {
    internal class Program {
        static void Main(string[] args) {
            System.Console.WriteLine("Datenbank Anbindung:");

            DatabaseBuilder databaseBuilder = new SqliteDatabaseBuilder();
            //TODO refractor Reset DB as default is not great
            databaseBuilder.InitCompleteDatabase();
            DatabaseStorage databaseStorageObject = new SqliteDatabaseStorage();
            GlobalObjects.Add(databaseStorageObject, InterfaceListing.DatabaseStorage);
            
            DatabaseStorage storage = (DatabaseStorage) GlobalObjects.
                Get(InterfaceListing.DatabaseStorage);
            System.Console.WriteLine("ObjectInfo = " + storage.GetInfo());

            Person rolf = new Person("Rolf", "Müller", "18-12-2022", "Bielefeld", "blastermaxxer", "123");
            rolf.Create();

            System.Console.WriteLine("Created: Name=" + rolf.firstname + " id=" + rolf.id);
            rolf.firstname = "Lutz";
            rolf.lastname = "Mataica";
            rolf.Update();

            System.Console.WriteLine("Updated: Name=" + rolf.firstname + ", " +rolf.lastname + " id=" + rolf.id);

            // TODO Read (Find) und Delete
            // TODO Infos ausgeben zu jeden Objekt als ganze Zeile
            // TODO zweite Person erstellen, beide ändern, ausgeben, löschen
            System.Console.ReadLine();

        }
    }
}
