using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Notenverwaltung {
    internal class Person : Entity {
        
        public string firstname { get; set; }
        public string lastname;

        public string birthdate;
        public string birthplace;

        public string username;
        public string password;

        public Person(string firstname, string lastname, string birthdate, string birthplace, string username, string password) {
            this.firstname = firstname;
            this.lastname = lastname;

            this.birthdate = birthdate;
            this.birthplace = birthplace;

            this.username = username;
            this.password = password;
        }
        public override List<KeyValue> ToKeyValue() {
            List<KeyValue> keyValue = new List<KeyValue>();
            keyValue.Add(new KeyValue(TableNames.PersonAttr.firstname, firstname));
            keyValue.Add(new KeyValue(TableNames.PersonAttr.lastname, lastname));

            keyValue.Add(new KeyValue(TableNames.PersonAttr.birthplace, birthplace));
            keyValue.Add(new KeyValue(TableNames.PersonAttr.birthdate, birthdate));

            keyValue.Add(new KeyValue(TableNames.PersonAttr.username, username));
            keyValue.Add(new KeyValue(TableNames.PersonAttr.password, password));
            return keyValue;
        }

        public override string ToTableName() {
            return TableNames.person;
        }
        public override string ToPrimaryKeyColumnName() {
            return TableNames.PersonAttr.personId;
        }
    }
}
