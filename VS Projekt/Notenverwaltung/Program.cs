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
            databaseBuilder.InitCompleteDatabase();
            DatabaseStorage databaseStorageObject = new SqliteDatabaseStorage();
            GlobalObjects.Add(databaseStorageObject, InterfaceListing.DatabaseStorage);
            
            DatabaseStorage storage = (DatabaseStorage) GlobalObjects.
                Get(InterfaceListing.DatabaseStorage);

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
            searchHermine.FindFirstByStringAttribute(new KeyValue(TableNotation.PersonAttr.firstname, "Hermine"));
            searchHermine.Print();

            Person newTeacher = new Person("Hanz", "Simmer", "1985-03-28", "Okthausen", "okto2000", "123");
            newTeacher.Create();
            Teacher teacher = new Teacher(newTeacher);
            teacher.Print();
            teacher.Create();
            teacher.Print();

            // TODO
            //Teacher searchHanz = new Teacher();
            //searchHanz.FindById(2);
            //searchHanz.Print();

            BranchOfStudy kunstPaedagogik = new BranchOfStudy("Kunstpädagogik");
            kunstPaedagogik.Create();
            kunstPaedagogik.Print();

            Class icd13 = new Class("ICD 13", (new SimpleDate("2022-09-01")).ToDateTime, (new SimpleDate("2024-08-31")).ToDateTime, kunstPaedagogik);
            icd13.Create();
            icd13.Print();
            Student student1 = new Student(searchHermine);
            student1.Create();
            // TODO
            //icd13.enrolledStudents.Add(student1);
            //icd13.Update();

            //foreach (Student student in icd13.enrolledStudents) {
            //    student.Print();
            //}


            // xTODO Find by ID (done)
            // xTODO Find By KeyValue - String (done)
            // xTODO Beziehungen (done)
            // nicht Suchfunktion verfeinern, sie ist bereits ausreichend/nicht Tiel der Aufgabe!
            // xTODO Create + Demodaten Bildungsgang
            // xTODO Attribute: DateTime und Integer!
            // xTODO Schüler hat viele Klassen
            // xTODO Klasse hat viele Schüler
            // xTODO FindByOtherATtribute relatiert Many To Manybeziehungen.
            // xTODO Schüler einen Bildungsgang, Kurs, Klasse zuweisen
            // xTODO xKurs,  xDozent,
            // TODO Dozent und Schülerhatklasse testen
            // TODO Teilnehmer
            // TODO Note, Notentyp
            // TODO Infos ausgeben zu jeden Objekt als ganze Zeile
            // TODO Einzelne Schüler ansteuern/hinzufügen usw...
            // Fach nicht machen
            System.Console.ReadLine();

        }
    }
}
