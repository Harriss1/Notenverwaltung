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
        /// <summary>
        /// Demonstration des ORM für eine Notenverwaltung
        /// Datenbanktyp ist austauschbar, hier wurde SQLite konkretisiert.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args) {
            // Festlegen welche Art von Nachrichten ausgegeben werden.
            DirectWriter.SetShowLevel(DirectWriter.ShowLevel.MESSAGE);
            DirectWriter.Debug("Datenbank Anbindung:");

            // Datenbank Erstellung
            DatabaseBuilder databaseBuilder = new SqliteDatabaseBuilder();
            databaseBuilder.InitCompleteDatabase();

            // Dependency Injection: Objekt Instanziieren und Global verfügbar machen
            DatabaseStorage databaseStorageObject = new SqliteDatabaseStorage();
            GlobalObjects.Add(databaseStorageObject, InterfaceListing.DatabaseStorage);
            DatabaseStorage storage = (DatabaseStorage) GlobalObjects.
                Get(InterfaceListing.DatabaseStorage);

            // Demonstrationsdaten Anzeigen
            DirectWriter.Msg("############## Prototyp Notenverwaltung ################################");
            DirectWriter.Msg("");
            DirectWriter.Msg("technischer Prototyp zum Anlegen, Auslesen und Aktualisieren von Datensätzen in einer Notenverwaltung");

            DirectWriter.Msg("Für Starten der ersten Demonstration bitte beliebige Taste drücken");
            System.Console.ReadKey();

            DirectWriter.Msg("");
            DirectWriter.Msg("Demonstration: Erstellung von Personen, Studenten, sowie Aktualisierung und Löschung.\n");
            DirectWriter.Msg("---------------------");
            DirectWriter.Msg("");
            DirectWriter.Msg("(bitte Taste drücken)");
            System.Console.ReadKey();


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

            DirectWriter.Msg("Für nächste Demonstration bitte beliebige Taste drücken");
            System.Console.ReadKey();

            DirectWriter.Msg("");
            DirectWriter.Msg("Demonstration: Suche von einer Person, dieses Objekt wird dann verwendet um aus dieser einen Schüler zu erstellen, aus der zweiten und dritten Person wird ein Lehrer erstellt.\n");
            DirectWriter.Msg("---------------------");
            DirectWriter.Msg("");
            DirectWriter.Msg("(bitte Taste drücken)");
            System.Console.ReadKey();


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

            DirectWriter.Msg("Für nächste Demonstration bitte beliebige Taste drücken");
            System.Console.ReadKey();

            DirectWriter.Msg("");
            DirectWriter.Msg("Demonstration: Erstellung eines Bildungsbereiches: Kunstpädagogik. Diesem Bildungsbereich wird eine Klasse zugeordnet.\n" +
                "Im Anschluss wird der vorherig erstellte Student 'Hermine Ganges' dieser Klasse hinzugefügt.\n");
            DirectWriter.Msg("---------------------");
            DirectWriter.Msg("");
            DirectWriter.Msg("(bitte Taste drücken)\n");
            System.Console.ReadKey();

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
            student1.classes.ForEach(x => x.Print());

            DirectWriter.Msg("Für nächste Demonstration bitte beliebige Taste drücken");
            System.Console.ReadKey();

            DirectWriter.Msg("");
            DirectWriter.Msg("Demonstration: ein weiteren Student der Klasse hinzufügen.\n");
            DirectWriter.Msg("---------------------");
            DirectWriter.Msg("");
            DirectWriter.Msg("(bitte Taste drücken)\n");
            System.Console.ReadKey();

            student1.Update();
            icd13.AddClassMember(student1);
            icd13.AddClassMember(hermineStudent);
            icd13.Update();
            icd13.Print();
            System.Console.WriteLine("### Alle Klassenmitglieder ausgeben: ###");
            //System.Console.ReadKey();
            icd13.enrolledStudents.ForEach(student => student.Print());
            Student findTheFirstOne = new Student();
            findTheFirstOne.FindById(student1.id);
            findTheFirstOne.Print();
            findTheFirstOne.person.Print();
            foreach(Class hisClass in findTheFirstOne.classes) {
                hisClass.Print();
            }


            DirectWriter.Msg("Für nächste Demonstration bitte beliebige Taste drücken");
            System.Console.ReadKey();

            DirectWriter.Msg("");
            DirectWriter.Msg("Demonstration: Ein Fach erstellen, diesem Fach Teilnehmer hinzufügen.\n");
            DirectWriter.Msg("---------------------");
            DirectWriter.Msg("");
            DirectWriter.Msg("(bitte Taste drücken)\n");
            System.Console.ReadKey();

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

            DirectWriter.Msg("Für nächste Demonstration bitte beliebige Taste drücken");
            System.Console.ReadKey();

            DirectWriter.Msg("");
            DirectWriter.Msg("Demonstration: Ein Teilnehmer in einem Fach erhalten Noten, es werden alle Noten angezeigt die ein Teilnehmer erhalten hat.\n");
            DirectWriter.Msg("---------------------");
            DirectWriter.Msg("");
            DirectWriter.Msg("(bitte Taste drücken)\n");
            System.Console.ReadKey();

            Grade gradeForTeilnehmer1 = new Grade(teilnehmer1, "nix zu sagen", "2022-10-10", 98);
            Grade grade2ForTeilnehmer1 = new Grade(teilnehmer1, "", "2022-10-10", 40);
            Grade grade3ForTeilnehmer1 = new Grade(teilnehmer1, "Hausaufgabe vergessen", "2022-10-10", 0);
            Grade grade4ForTeilnehmer1 = new Grade(teilnehmer1, "Klausur", "2022-10-10", 30);
            gradeForTeilnehmer1.Create();
            grade2ForTeilnehmer1.Create();
            grade3ForTeilnehmer1.Create();
            grade4ForTeilnehmer1.Create();

            DirectWriter.Msg("\n# Kursname:");
            gradeForTeilnehmer1.participant.course.Print();

            DirectWriter.Msg("# Noten und Namen der einzelnen Teilnehmer:");
            gradeForTeilnehmer1.Print();
            gradeForTeilnehmer1.participant.student.person.Print();
            grade2ForTeilnehmer1.Print();
            grade2ForTeilnehmer1.participant.student.person.Print();
            grade3ForTeilnehmer1.Print();
            grade3ForTeilnehmer1.participant.student.person.Print();
            grade4ForTeilnehmer1.Print();
            grade4ForTeilnehmer1.participant.student.person.Print();
            

            DirectWriter.Msg("");
            DirectWriter.Msg("######### Ende der Demonstration #####################.\n");

            DirectWriter.Msg("Zum Beenden bitte beliebige Taste drücken");
            System.Console.ReadKey();


        }
    }
}
