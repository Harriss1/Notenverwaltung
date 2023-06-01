using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    /// <summary>
    /// Tabellennamen als Hauptattribute und Attributnamen einzelner Tabellen
    /// 
    /// TODO Sollte ein Struct sein?
    /// </summary>
    internal class TableNotation {
        
        public const string teacher = "Lehrer";
        public const string person = "Person";
        public const string student = "Schueler";
        public const string class_tablename = "Klasse";
        public const string branchOfStudy = "Bildungsgang";
        public const string studentHasClass = "Schueler_Hat_Klasse";
        public const string course = "Kurs";
        public const string lecturer = "Dozent";
        public const string subject = "Fach";
        public const string participant = "Teilnehmer";

        /////////////////////////////////////////////////////////////
        /// Attribute einzelner Tabellen
        /// 
        public class PersonAttr {
            //Vorname, Nachname, Geburtsdatum, Geburtsort, Benutzername, Passwort)" +
            public const string personId = "PersonId";
            public const string firstname = "Vorname";
            public const string lastname = "Nachname";
            public const string birthdate = "Geburtsdatum";
            public const string birthplace = "Geburtsort";
            public const string username = "Benutzername";
            public const string password = "Passwort";
        }
        public class TeacherAttr {
            public const string teacherId = "LehrerId";
            public const string personId = "PersonId";
        }
        public class LecturerAttr {
            public const string teacherId = "LehrerId";
            public const string courseId = "KursId";
        }
        public class StudentAttr {
            public const string studentId = "SchuelerId";
            public const string personId = "PersonId";
        }
        public class StudentHasClassAttr {
            public const string studentId = "SchuelerId";
            public const string classId = "KlasseId";
        }
        public class BranchOfStudyAttr {
            public const string branchOfStudyId = "BildungsgangId";
            public const string label = "Bezeichnung";
        }

        public class ClassAttr {
            public const string classId = "KlasseId";
            public const string branchOfStudyId = "BildungsgangId";
            public const string label = "Bezeichnung";
            public const string startDate = "StartDatum";
            public const string endDate = "EndDatum";
        }

        public class CourseAttr {
            public const string courseId = "KursId";
            public const string subjectId = "FachId";
            public const string label = "Bezeichnung";
            public const string startDate = "StartDatum";
            public const string endDate = "EndDatum";

        }
        public class SubjectAttr {
            public const string subjectId = "FachId";
            public const string label = "Bezeichnung";
            public const string acronym = "Akronym";

        }
        public class ParticipantAttr {
            public const string participantId = "TeilnehmerId";
            public const string courseId = "KursId";
            public const string studentId = "SchuelerId";
        }


    }
}
