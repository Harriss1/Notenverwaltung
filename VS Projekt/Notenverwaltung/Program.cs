﻿using System;
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

            Person rolf = new Person("Rolf", "Müller", "2022-12-18", "Bielefeld", "blastermaxxer", "123");
            rolf.Create();
            Person hermine = new Person("Hermine", "Ganges", "1800-12-15", "Pattern", "trabbi", "123");
            hermine.Create();
            System.Console.WriteLine("Created: " + hermine.ToText());
            Person gunnar = new Person("Gunnar", "Mülli", "2001-04-12", "Kölle", "zander", "123");
            gunnar.Create();
            System.Console.WriteLine("Created: " + gunnar.ToText());

            System.Console.WriteLine("Created: Name=" + rolf.firstname + " id=" + rolf.id);
            rolf.firstname = "Lutz";
            rolf.lastname = "Mataica";
            rolf.Update();

            System.Console.WriteLine("Updated: Name=" + rolf.firstname + ", " +rolf.lastname + " id=" + rolf.id);
            System.Console.WriteLine("Updated: " + rolf.ToText());
            rolf.Delete();
            System.Console.WriteLine("Deleted: " + rolf.ToText());
            
            System.Console.WriteLine("Suche Hermine");
            Person searchHermine = new Person();
            searchHermine.FindById(6);
            searchHermine.Print();
            searchHermine.FindFirstByStringAttribute(new KeyValue(TableNames.PersonAttr.firstname, "Hermine"));
            searchHermine.Print();


            // TODO Find by ID (done)
            // TODO Find By KeyValue - String (done)
            // TODO Beziehungen
            // TODO restliche Tabellen + Demodaten erstellen
            // TODO Infos ausgeben zu jeden Objekt als ganze Zeile
            // TODO zweite Person erstellen, beide ändern, ausgeben, löschen
            System.Console.ReadLine();

        }
    }
}
