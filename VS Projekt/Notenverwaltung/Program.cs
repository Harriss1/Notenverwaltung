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
            // Festlegen welche Art von Nachrichten ausgegeben werden.
            DirectWriter.SetShowLevel(DirectWriter.ShowLevel.MESSAGE);
            DirectWriter.Debug("Datenbank Anbindung:");

            DatabaseBuilder databaseBuilder = new SqliteDatabaseBuilder();
            databaseBuilder.InitCompleteDatabase();
            DatabaseStorage databaseStorageObject = new SqliteDatabaseStorage();
            GlobalObjects.Add(databaseStorageObject, InterfaceListing.DatabaseStorage);
            
            DatabaseStorage storage = (DatabaseStorage) GlobalObjects.
                Get(InterfaceListing.DatabaseStorage);
            DirectWriter.Msg("############## Prototyp Notenverwaltung ################################");
            DirectWriter.Msg("");
            DirectWriter.Msg("technischer Prototyp zum Anlegen, Auslesen und Aktualisieren von Datensätzen in einer Notenverwaltung");

            DirectWriter.Msg("Zum Fortfahren bitte beliebige Taste drücken");
            System.Console.ReadKey();

            DirectWriter.Msg("");
            DirectWriter.Msg("Demonstration: Erstellung von Personen, Studenten, sowie Aktualisierung und Löschung.\n");

            Person rolf = new Person("Rolf", "Müller", "2022-12-18", "Bielefeld", "blastermaxxer", "123");
            rolf.Create();
            Person hermine = new Person("Hermine", "Ganges", "1800-12-15", "Pattern", "trabbi", "123");
            hermine.Create();
            DirectWriter.Msg("Created: " + hermine.ToText());
            Person gunnar = new Person("Gunnar", "Mülli", "2001-04-12", "Kölle", "zander", "123");
            gunnar.Create();
            DirectWriter.Msg("Created: " + gunnar.ToText());

            DirectWriter.Msg("Created: Name=" + rolf.firstname + " id=" + rolf.id);
            rolf.firstname = "Lutz";
            rolf.lastname = "Mataica";
            rolf.Update();

            DirectWriter.Msg("Updated: Name=" + rolf.firstname + ", " +rolf.lastname + " id=" + rolf.id);
            DirectWriter.Msg("Updated: " + rolf.ToText());
            rolf.Delete();
            DirectWriter.Msg("Deleted: " + rolf.ToText());

            DirectWriter.Msg("Zum Fortfahren bitte beliebige Taste drücken");
            System.Console.ReadKey();

            DirectWriter.Msg("");
            DirectWriter.Msg("Demonstration: Suche von einer Person, dieses Objekt wird dann verwendet um aus dieser einen Schüler zu erstellen, aus der zweiten und dritten Person wird ein Lehrer erstellt.\n");

            DirectWriter.Msg("Suche Hermine");
            Person searchHermine = new Person();
            searchHermine.FindById(6);
            searchHermine.Print();
            //searchHermine.FindFirstByStringAttribute(new KeyValue(TableNotation.PersonAttr.firstname, "Hermine"));
            searchHermine.Print();
            Person klaus = new Person("Klaus", "Müller", "2005-04-23", "Ulm", "knasti", "admin");
            klaus.Create();
            klaus.Print();
            Student hermineStudent = new Student(klaus);
            hermineStudent.Create();
            hermineStudent.Print();
            Student findStudent = new Student();
            findStudent.FindById(hermineStudent.id);
            findStudent.Print();

            Person newTeacher = new Person("Hanz", "Simmer", "1985-03-28", "Okthausen", "okto2000", "123");
            newTeacher.Create();
            Teacher teacher = new Teacher(newTeacher);
            teacher.Print();
            teacher.Create();
            teacher.Print();

            Teacher searchHanz = new Teacher();
            searchHanz.FindById(2);
            searchHanz.Print();

            DirectWriter.Msg("Zum Fortfahren bitte beliebige Taste drücken");
            System.Console.ReadKey();

            DirectWriter.Msg("");
            DirectWriter.Msg("Demonstration: Erstellung eines Bildungsbereiches: Kunstpädagogik. Diesem Bildungsbereich wird eine Klasse zugeordnet.\n" +
                "Im Anschluss wird der vorherig erstellte Student 'Hermine Ganges' dieser Klasse hinzugefügt.\n");
            BranchOfStudy kunstPaedagogik = new BranchOfStudy("Kunstpädagogik");
            kunstPaedagogik.Create();
            kunstPaedagogik.Print();

            Class icd13 = new Class("ICD 13", (new SimpleDate("2022-09-01")).ToDateTime, (new SimpleDate("2024-08-31")).ToDateTime, kunstPaedagogik);
            icd13.Create();
            icd13.Print();
            //System.Console.ReadKey();
            System.Console.WriteLine("########################### Klassenerstellung Ende###");
            Student student1 = new Student(searchHermine);
            student1.Create();
            student1.Print();
            student1.AddToClass(icd13); 
            student1.Print();

            //System.Console.ReadKey();
            System.Console.WriteLine("Kein Loop##################");
            student1.Update();
            //System.Console.ReadKey();
            System.Console.WriteLine("Loop##################");
            icd13.AddClassMember(student1);
            icd13.AddClassMember(hermineStudent);
            icd13.Update();
            icd13.Print();
            System.Console.WriteLine("########################### foreach classmember ###");
            //System.Console.ReadKey();
            icd13.enrolledStudents.ForEach(student => student.Print());
            Student findTheFirstOne = new Student();
            findTheFirstOne.FindById(student1.id);
            findTheFirstOne.Print();
            findTheFirstOne.person.Print();
            foreach(Class hisClass in findTheFirstOne.classes) {
                hisClass.Print();
            }
            Subject mathSubject = new Subject("Mathematik Grundlagen", "MA");
            mathSubject.Create();
            mathSubject.Print();
            Course math = new Course("Mathe 2", "2022-09-01", "2023-03-31", mathSubject);
            math.Create();
            math.Print();
            math.subject.Print();

            Participant teilnehmer1 = new Participant(findTheFirstOne, math);
            teilnehmer1.Create();
            teilnehmer1.Print();
            teilnehmer1.student.person.Print();
            Grade gradeForTeilnehmer1 = new Grade(teilnehmer1, "nix zu sagen", "2022-10-10", 98);
            Grade grade2ForTeilnehmer1 = new Grade(teilnehmer1, "", "2022-10-10", 40);
            Grade grade3ForTeilnehmer1 = new Grade(teilnehmer1, "Hausaufgabe vergessen", "2022-10-10", 0);
            Grade grade4ForTeilnehmer1 = new Grade(teilnehmer1, "Klausur", "2022-10-10", 30);
            gradeForTeilnehmer1.Create();
            grade2ForTeilnehmer1.Create();
            grade3ForTeilnehmer1.Create();
            grade4ForTeilnehmer1.Create();
            gradeForTeilnehmer1.Print();
            gradeForTeilnehmer1.participant.student.person.Print();
            grade2ForTeilnehmer1.Print();
            grade2ForTeilnehmer1.participant.student.person.Print();
            grade3ForTeilnehmer1.Print();
            grade3ForTeilnehmer1.participant.student.person.Print();
            grade4ForTeilnehmer1.Print();
            grade4ForTeilnehmer1.participant.student.person.Print();


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
