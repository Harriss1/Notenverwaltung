using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    internal class TableNames {
        public const string teacher = "Lehrer";
        public const string person = "Person";
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
