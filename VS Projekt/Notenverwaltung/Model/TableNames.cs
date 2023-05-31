using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    internal class TableNames {
        public const string teacher = "Lehrer";
        public const string person = "Person";
        public const string student = "Schueler";
        public const string class_tablename = "Klasse";
        public const string branchOfStudy = "Bildungsgang";

        public class TeacherAttr {
            public const string teacherId = "LehrerId";
            public const string personId = "PersonId";
        }
        public class StudentAttr {
            public const string studentId = "SchuelerId";
            public const string personId = "PersonId";
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


    }
}
