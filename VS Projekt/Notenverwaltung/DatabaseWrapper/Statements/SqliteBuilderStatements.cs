using System;

// https://learn.microsoft.com/en-us/visualstudio/ide/default-keyboard-shortcuts-in-visual-studio?view=vs-2022
namespace Notenverwaltung {
    internal class SqliteBuilderStatements: BuilderStatements {

        public string[] CreateAllTables() {
            // the (20) will be ignored
            // see https://www.sqlite.org/datatype3.html#affinity_name_examples
            // ExecuteQuery("Create Table if not exists highscores (name varchar(20), score int)");
            string createPerson =
                "CREATE TABLE IF NOT EXISTS Person (" +
                "PersonId INTEGER PRIMARY KEY AUTOINCREMENT," +
                "Vorname VARCHAR(50)," +
                "Nachname VARCHAR(50)," +
                "Geburtsdatum DATE," +
                "Geburtsort VARCHAR(50)," +
                "Benutzername VARCHAR(50) NOT NULL UNIQUE," +
                "Passwort VARCHAR(50)" +
                "); ";
            string createTeacher =
                "CREATE TABLE IF NOT EXISTS Lehrer(" +
                "LehrerId INTEGER PRIMARY KEY AUTOINCREMENT," +
                "PersonId INTEGER NOT NULL," +
                "FOREIGN KEY(PersonId) REFERENCES Person(PersonId)" +
                "); ";
            string createStudent = "CREATE TABLE IF NOT EXISTS Schueler(" +
                "SchuelerId INTEGER PRIMARY KEY AUTOINCREMENT," +
                "PersonId INTEGER NOT NULL," +
                "FOREIGN KEY(PersonId) REFERENCES Person(PersonId)" +
                "); ";
            string createBranchOfStudy = "CREATE TABLE IF NOT EXISTS Bildungsgang(" +
                "BildungsgangId INTEGER PRIMARY KEY AUTOINCREMENT," +
                "Bezeichnung VARCHAR(50)" +
                "); ";
            string createClass =
                "CREATE TABLE IF NOT EXISTS Klasse(" +
                "KlasseId INTEGER PRIMARY KEY AUTOINCREMENT," +
                "Bezeichnung VARCHAR(50) NOT NULL," +
                "StartDatum DATE NOT NULL," +
                "EndDatum DATE," +
                "BildungsgangId INT NOT NULL," +
                "FOREIGN KEY(BildungsgangId) REFERENCES Bildungsgang(BildungsgangId)" +
                "); ";
            string createStudentHasClass =
                "CREATE TABLE IF NOT EXISTS Schueler_Hat_Klasse(" +
                "SchuelerId INTEGER NOT NULL," +
                "KlasseId INTEGER NOT NULL," +
                "FOREIGN KEY(SchuelerId) REFERENCES Schueler(SchuelerId)," +
                "FOREIGN KEY(KlasseId) REFERENCES Klasse(KlasseId)); ";
            string createSubject =
                "CREATE TABLE IF NOT EXISTS Fach (" +
                "FachId INTEGER PRIMARY KEY AUTOINCREMENT," +
                "Bezeichnung VARCHAR(50)," +
                "Akronym VARCHAR(50)" +
                "); ";
            string createCourse =
                "CREATE TABLE IF NOT EXISTS Kurs (" +
                "KursId INTEGER PRIMARY KEY AUTOINCREMENT," +
                "Bezeichnung VARCHAR(50)," +
                "StartDatum DATE NOT NULL," +
                "EndDatum DATE," +
                "FachId INTEGER NOT NULL," +
                "FOREIGN KEY(FachId) REFERENCES Fach(FachId)); ";
            string createParticipant =
                "CREATE TABLE IF NOT EXISTS Teilnehmer(" +
                "TeilnehmerId INTEGER PRIMARY KEY AUTOINCREMENT," +
                "SchuelerId INTEGER," +
                "KursId INTEGER," +
                "EndNote VARCHAR(50)," +
                "FOREIGN KEY(SchuelerId) REFERENCES schueler(SchuelerId)," +
                "FOREIGN KEY(KursId) REFERENCES Kurs(KursId));  ";
            string createLecturer =
                "CREATE TABLE IF NOT EXISTS Dozent(" +
                "DozentId INTEGER PRIMARY KEY AUTOINCREMENT," +
                "LehrerId INTEGER," +
                "KursId INTEGER," +
                "FOREIGN KEY(LehrerId) REFERENCES Lehrer(LehrerId)," +
                "FOREIGN KEY(KursId) REFERENCES Kurs(KursId));";
            string createGradeTyp =
                "CREATE TABLE IF NOT EXISTS NotenTyp(" +
                "NotenTypId INTEGER PRIMARY KEY AUTOINCREMENT," +
                "Bezeichnung VARCHAR(50)," +
                "Akronym VARCHAR(50)); ";
            string createGrade =
                "CREATE TABLE IF NOT EXISTS Note(" +
                "NoteId INTEGER PRIMARY KEY AUTOINCREMENT," +
                "TeilnehmerId INTEGER NOT NULL," +
                "DozentId INTEGER NOT NULL," +
                "Bemerkung VARCHAR(50)," +
                "Datum DATE," +
                "NotenTypId INTEGER," +
                "WertInProzent INTEGER," +
                "FOREIGN KEY(TeilnehmerId) REFERENCES Teilnehmer(TeilnehmerId)," +
                "FOREIGN KEY(NotenTypId) REFERENCES NotenTyp(NotenTypId)," +
                "FOREIGN KEY(DozentId) REFERENCES Dozent(DozentId)); ";
            return new string[]{
                createPerson,
                createTeacher,
                createStudent,
                createBranchOfStudy,
                createClass,
                createStudentHasClass,
                createSubject,
                createCourse,
                createParticipant,
                createLecturer,
                createGradeTyp,
                createGrade
            };
        }

        public string DropAllTables() {
            return 
                //"#Verbindungstabellen " +
                "DROP TABLE if EXISTS Schueler_Hat_Klasse;" +
                //"# Haupttabellen " +
                "DROP TABLE if EXISTS Note;" +
                "DROP TABLE if EXISTS Dozent;" +
                "DROP TABLE if EXISTS lehrer;" +
                "DROP TABLE if EXISTS Teilnehmer;" +
                "DROP TABLE if EXISTS schueler;" +
                "DROP TABLE if EXISTS person; " +
                //"# Von den Haupttabellen abhängige Haupttabellen " +
                "DROP TABLE if EXISTS Klasse;" +
                "DROP TABLE if EXISTS Bildungsgang;" +
                "DROP TABLE if EXISTS Kurs;" +
                "DROP TABLE if EXISTS fach;" +
                "DROP TABLE if EXISTS NotenTyp; ";
        }

        public string[] InsertDemoData() {
            string person =
                "INSERT INTO person(Vorname, Nachname, Geburtsdatum, Geburtsort, Benutzername, Passwort)" +
                "VALUES" +
                "('Max', 'Mustermann', '2022-01-01', 'Bielefeld', 'user', 'user')," +
                "('Paula', 'Pause', '1990-12-05', 'Münchhausen', 'guest', 'guest')," +
                "('Admin', 'Nistrator', '1900-01-01', 'Münchhausen', 'admin', 'admin')," +
                "('Herbert', 'Herbtraube', '1940-03-19', 'Münchhausen', 'teacher', 'teacher'); ";
            
            string teacher =
                "INSERT INTO Lehrer (PersonId)" +
                "VALUES" +
                "(3)," +
                "(4); ";
            string student =
                "INSERT INTO Schueler (PersonId)" +
                "VALUES" +
                "(1)," +
                "(2); ";
            string branchOfStudy =
                "INSERT INTO Bildungsgang(Bezeichnung)" +
                "VALUES" +
                "('Systemintegration')," +
                "('Anwendungsentwicklung'); ";
            string insertClass =
                "INSERT INTO Klasse(Bezeichnung, StartDatum, BildungsgangId) " +
                "VALUES " +
                "('IA121', '2021-08-01', 1)," +
                "('IS322', '2019-08-01', 2); ";
            string studentHasClass =
                "INSERT INTO Schueler_Hat_Klasse (SchuelerId, KlasseId)" +
                "VALUES" +
                "(1, 1)," +
                "(1, 2), " + // # Schueler 1 geht in zwei Klassen
                "(2, 2); ";
            string subject =
                "INSERT 	INTO Fach (Bezeichnung, Akronym)" +
                "VALUES" +
                "('Mathematik', 'MA')," +
                "('Deutsch', 'DEU'); ";
            string course =
                "INSERT 	INTO Kurs (Bezeichnung, StartDatum, FachId)" +
                "VALUES" +
                "('Mathe 1. LJ', '2022-08-31', 1)," +
                "('Deutsch 2. HJ', '2022-03-31', 2); ";
            string participant =
                "INSERT 	INTO Teilnehmer (SchuelerId, KursId, Endnote)" +
                "VALUES" +
                "(1, 1, null)," +
                "(2, 1, '3,0')," +
                "(2, 2, '2+'); ";
            string lecturer =
                "INSERT 	INTO Dozent (LehrerId, KursId)" +
                "VALUES" +
                "(1, 1)," +
                "(2, 1),(2, 2); ";
            string gradeType =
                "INSERT 	INTO NotenTyp (Bezeichnung, Akronym)" +
                "VALUES" +
                "('Klausur', 'KA')," +
                "('Sonstige Mitarbeit', 'SoMi')," +
                "('Projekt', 'PR'); ";
            string grade =
                "INSERT 	INTO Note (TeilnehmerId, DozentId, Bemerkung, Datum, NotenTypId, WertInProzent)" +
                "VALUES" +
                "(1, 1, 'logisch über Algebra Aufgabe argumentiert', '2022-10-03', 2, '98')," +
                "(1, 1, '', '2022-11-23', 1, '83')," +
                "(1, 2, '', '2023-01-05', 3, '75')," +
                "(2, 3, 'Fragen wiederholt nicht beantwortet', '2022-10-14', 2, '65')," +
                "(2, 3, '', '2022-11-23', 1, '90')," +
                "(2, 3, 'Abgabe drei Tage zu spät am 8.1.2023', '2023-01-05', 3, '0'); ";
            return new string[] {
                person,
                teacher,
                student,
                branchOfStudy,
                insertClass,
                studentHasClass,
                subject,
                course,
                participant,
                lecturer,
                gradeType,
                grade
            };
        }
    }
}
